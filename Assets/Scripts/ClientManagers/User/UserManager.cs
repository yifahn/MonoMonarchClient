using System;
using System.Threading.Tasks;

using UnityEngine;
using Unity.VisualScripting;

using Newtonsoft.Json;

using MonoMonarchNetworkFramework;
using MonoMonarchNetworkFramework.Authentication.Register;
using MonoMonarchNetworkFramework.Authentication.Login;
using MonoMonarchNetworkFramework.Authentication.Logout;
using MonoMonarchNetworkFramework.Authentication.RefreshToken;

using Assets.Scripts.ClientManagers.Game;
using System.Text.RegularExpressions;
using UnityEditor.PackageManager;
using Assets.Scripts.ClientManagers.Kingdom;

namespace Assets.Scripts.ClientManagers.User
{
    public class UserManager : MonoBehaviour
    {
        #region User Singleton
        private static IUserService _userService { get; set; }
        

        // Singleton instance
        private static UserManager _instance;
        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<UserManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(UserManager).Name);
                        _instance = singletonInstance.AddComponent<UserManager>();

                        _userService = new UserService();

                    }

                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
        #endregion

        #region Authentication Properties
        public RegistrationResponse UserRegistrationResponse { get; set; }
        public LoginResponse UserLoginResponse { get; set; }
        public RefreshTokenResponse UserRefreshTokenResponse { get; set; }
        public ErrorResponse UserErrorResponse { get; set; }

        [SerializeField] private string _userEmail;
        public string UserEmail { get => _userEmail; set => _userEmail = value; }

        #endregion

        public async Task RegisterAsync(string password, string passwordConfirm)
        {
            try
            {
                if (!password.Equals(passwordConfirm))
                {
                    UserErrorResponse = new ErrorResponse("Passwords do not match");
                    return;
                }
                if (!EmailValidator.IsEmailValid(UserEmail))
                {
                    UserErrorResponse = new ErrorResponse("Invalid email format");
                    return;
                }
                var response = await _userService.RegisterAsync(new RegistrationPayload { Email = UserEmail, Password = password });
                if (response is RegistrationResponse registrationResponse)
                {
                    Debug.Log("REGISTRATION-RESPONSE SUCCESS");
                    UserRegistrationResponse = registrationResponse;
                    //GameManager.Instance.NavToScene("BTN_NAV_LOGIN_R");
                    NavRegisterLoginElements("BTN_NAV_LOGIN_R");
                }
                else if (response is ErrorResponse errorResponse)
                {
                    UserErrorResponse = errorResponse;
                    Debug.Log("ERROR-RESPONSE FAILURE");
                    GameManager.Instance.ClearGameCache();
                }
            }
            catch (Exception ex)
            {
                //if register fails
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                GameManager.Instance.ClearGameCache();
            }
        }
        public async Task LoginAsync(string password)
        {
            try
            {
                var response = await _userService.LoginAsync(new LoginPayload { Email = UserEmail, Password = password });

                if (response is LoginResponse loginResponse)
                {
                    Debug.Log("LOGIN-RESPONSE SUCCESS");
                    UserLoginResponse = loginResponse;
                    if (await GameManager.Instance.LoadGameState())
                    {
                        GameManager.Instance.NavToScene("BTN_LOGIN_SUBMIT_L");
                        GameManager.Instance.LoginProcedure();
                    }
                        

                    else GameManager.Instance.ClearGameCache();
                }
                else if (response is ErrorResponse errorResponse)
                {
                    UserErrorResponse = errorResponse;
                    Debug.Log("ERROR-RESPONSE FAILURE");
                    GameManager.Instance.ClearGameCache();
                }
            }
            catch (Exception ex)
            {
                //if login fails 
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                GameManager.Instance.ClearGameCache();
               
            }
        }
        public async Task LogoutAsync()
        {
            try
            {
                var response = await _userService.LogoutAsync(new LogoutPayload { AuthToken = UserLoginResponse.AuthToken, RefreshToken = UserLoginResponse.RefreshToken });
                if (response is LogoutResponse)
                {
                    
                    UserErrorResponse = null;
                    GameManager.Instance.ClearGameCache();
                    GameManager.Instance.NavToScene("btn_MainMenu_Scene");
                }
                else if (response is ErrorResponse errorResponse)
                {
                    UserErrorResponse = errorResponse;
                    GameManager.Instance.ClearGameCache();
                    GameManager.Instance.NavToScene("btn_MainMenu_Scene");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                GameManager.Instance.ClearGameCache();
                GameManager.Instance.NavToScene("btn_MainMenu_Scene");
            }
        }
        public static int RefreshTokenAttempts { get; set; } = 0; //ceases infinite loop of requesting token pair from server if refreshtoken is expired
        public async Task RefreshTokenAsync()
        {
            try
            {
                var response = await _userService.RefreshTokenAsync(new RefreshTokenPayload { AuthToken = UserLoginResponse.AuthToken, RefreshToken = UserLoginResponse.RefreshToken });
                if (response is RefreshTokenResponse tokens)
                {
                    UserRefreshTokenResponse = tokens;
                    UserLoginResponse.AuthToken = UserRefreshTokenResponse.AuthToken;
                    UserLoginResponse.RefreshToken = UserRefreshTokenResponse.RefreshToken;
                    RefreshTokenAttempts = 0;
                }
                else if (response is ErrorResponse)
                {
                    if (RefreshTokenAttempts == 1)
                    {
                        UserErrorResponse = response as ErrorResponse;
                        GameManager.Instance.ClearGameCache();
                        GameManager.Instance.NavToScene("btn_MainMenu_Scene");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                GameManager.Instance.ClearGameCache();
                GameManager.Instance.NavToScene("btn_MainMenu_Scene");
            }
        }
        public void ClearUserCache()
        {
            RefreshTokenAttempts = 0;
            UserEmail = string.Empty;
            UserRegistrationResponse = null;
            UserLoginResponse = null;
        }
        public void NavRegisterLoginElements(string objectName)
        {
            switch (objectName)
            {
                case "BTN_NAV_REGISTER_L":
                    GameObject.Find("PANEL_MAINMENU_REGISTER").transform.localScale = new Vector3(1, 1, 1);
                    GameObject.Find("PANEL_MAINMENU_LOGIN").transform.localScale = new Vector3(0, 0, 0);
                    //obj.transform.parent.parent.Find("PANEL_MAINMENU_REGISTER").transform.localScale = new Vector3(1, 1, 1);
                    //obj.transform.parent.parent.Find("PANEL_MAINMENU_LOGIN").transform.localScale = new Vector3(0, 0, 0);
                    break;
                case "BTN_NAV_LOGIN_R":
                    GameObject.Find("PANEL_MAINMENU_REGISTER").transform.localScale = new Vector3(0, 0, 0);
                    GameObject.Find("PANEL_MAINMENU_LOGIN").transform.localScale = new Vector3(1, 1, 1);
                    //obj.transform.parent.parent.Find("PANEL_MAINMENU_LOGIN").transform.localScale = new Vector3(1, 1, 1);
                    //obj.transform.parent.parent.Find("PANEL_MAINMENU_REGISTER").transform.localScale = new Vector3(0, 0, 0);
                    break;//s
            }
        }
        public static bool IsPasswordConfirmed(string password, string passwordConfirm)
        {
            return true ? password.Equals(passwordConfirm) : false;
        }
    }
    public static class EmailValidator
    {
        private static readonly Regex EmailRegex =
            new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // false == null, empty, whitespace
            }
            return EmailRegex.IsMatch(email);
        }
    }
}