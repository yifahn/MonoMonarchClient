
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

using Assets.Scripts.ClientManagers;
using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.ClientManagers.User;


namespace Assets.Scripts.Interactables
{
    public class AuthenticationInteractable : MonoBehaviour, IInteractable
    {
        public async void Interact()
        {
            Debug.Log($"INTERACT - {gameObject.name}");

            switch (gameObject.name)
            {
                case "BTN_REGISTER_SUBMIT_R":
                    //register logic
                    UserManager.Instance.UserEmail = GameObject.Find("INPUT_EMAIL_R").GetComponentInChildren<TMP_InputField>().text;
                    await UserManager.Instance.RegisterAsync(GameObject.Find("INPUT_PASSWORD_R").GetComponentInChildren<TMP_InputField>().text, GameObject.Find("INPUT_PASSWORD_CONFIRM_R").GetComponentInChildren<TMP_InputField>().text);
                    if (UserManager.Instance.UserErrorResponse != null)
                    {
                        //implement object to display error response messages
                    }
                    return;

                case "BTN_LOGIN_SUBMIT_L":
                    //login logic
                    UserManager.Instance.UserEmail = GameObject.Find("INPUT_EMAIL_L").GetComponentInChildren<TMP_InputField>().text;
                    string passwordLogin = GameObject.Find("INPUT_PASSWORD_L").GetComponentInChildren<TMP_InputField>().text;

                    await UserManager.Instance.LoginAsync(passwordLogin);
                    
                    //return doesn't execute if success - ends here
                    return;
                /*
                string email = GameObject.Find("INPUT_EMAIL_R").GetComponentInChildren<TMP_InputField>().text;
                string password = GameObject.Find("INPUT_PASSWORD_R").GetComponentInChildren<TMP_InputField>().text;
                string passwordConfirm = GameObject.Find("INPUT_PASSWORD_CONFIRM_R").GetComponentInChildren<TMP_InputField>().text;

                if (password.Equals(passwordConfirm)) await UserManager.Instance.RegisterUser(email, password);
                else
                {
                    Debug.Log(password + " " + passwordConfirm);
                    Debug.Log("Registration failed - _password mismatch");
                }
                return;*/
                case "btn_MainMenu_Scene": //??????????????????????????
                    await UserManager.Instance.LogoutAsync();
                    return;
            }
            //UI ADJUSTMENT
            UserManager.Instance.NavRegisterLoginElements(gameObject.name);
        }
    }
}
