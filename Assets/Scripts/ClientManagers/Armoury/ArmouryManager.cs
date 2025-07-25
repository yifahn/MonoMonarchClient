using MonoMonarchNetworkFramework.Game.Character;
using MonoMonarchNetworkFramework;
using System;
using UnityEngine;
using Assets.Scripts.ClientManagers.Game;
using System.Threading.Tasks;
using MonoMonarchNetworkFramework.Game.Armoury;

namespace Assets.Scripts.ClientManagers.Armoury
{
    public class ArmouryManager : MonoBehaviour
    {

        #region Armoury Singleton
        private static IArmouryService _armouryService { get; set; }
        private static ArmouryManager _instance;
        public static ArmouryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ArmouryManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(ArmouryManager).Name);
                        _instance = singletonInstance.AddComponent<ArmouryManager>();
                        _armouryService = new ArmouryService();
                    }
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        public static void ResetInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
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

        #region Armoury Properties
        public ArmouryLoadResponse ArmouryLoadResponse { get; set; }
        public ErrorResponse ArmouryErrorResponse { get; set; }

        // [SerializeField] private string _soupkitchenC_;
        //public string SoupkitchenC_ { get => _soupkitchenC_; set => _soupkitchenC_ = value; }

        #endregion

        public async Task<bool> ArmouryLoadAsync()
        {
            try
            {
                var response = await _armouryService.ArmouryLoadAsync();
                if (response is ArmouryLoadResponse armouryLoadResponse)
                {
                    ArmouryLoadResponse = armouryLoadResponse;

                    //TreasuryCoinBag = SoupkitchenLoadResponse.CoinbagArray;
                    //TreasuryTotalCoin = SoupkitchenLoadResponse.TotalCoin;
                }
                else if (response is ErrorResponse errorResponse)
                {
                    ArmouryErrorResponse = errorResponse;
                    await LoadingScreenExtensions.LoadLoadingSceneAsync();
                    await LoadingScreenExtensions.UnloadGameSceneAsync();
                    await LoadingScreenExtensions.LoadMainMenuSceneAsync();
                    await LoadingScreenExtensions.UnloadLoadingSceneAsync();
                    GameManager.Instance.ClearGameCache();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                await LoadingScreenExtensions.LoadLoadingSceneAsync();
                await LoadingScreenExtensions.UnloadGameSceneAsync();
                await LoadingScreenExtensions.LoadMainMenuSceneAsync();
                await LoadingScreenExtensions.UnloadLoadingSceneAsync();
                GameManager.Instance.ClearGameCache();
                return false;
            }
        }


        public void ClearArmouryCache()
        {
            ArmouryLoadResponse = null;
        }
    }
}
