using MonoMonarchGameFramework.Game.Treasury.GoldBag;
using MonoMonarchNetworkFramework.Game.Treasury;
using MonoMonarchNetworkFramework;
using System;
using UnityEngine;
using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.NEW.ClientManagers.Soupkitchen;
using MonoMonarchNetworkFramework.Game.Soupkitchen;
using System.Threading.Tasks;
using MonoMonarchGameFramework.Game.Soupkitchen;
using MonoMonarchGameFramework.Game.Treasury;
using Newtonsoft.Json;
using System.IO;

namespace Assets.Scripts.ClientManagers.Soupkitchen
{
    public class SoupkitchenManager : MonoBehaviour
    {
        #region Soupkitchen Singleton
        private static ISoupkitchenService _soupkitchenService { get; set; }
        private static SoupkitchenManager _instance;
        public static SoupkitchenManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<SoupkitchenManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(SoupkitchenManager).Name);
                        _instance = singletonInstance.AddComponent<SoupkitchenManager>();
                        _soupkitchenService = new SoupkitchenService();
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

        #region Soupkitchen Properties
        public SoupkitchenLoadResponse SoupkitchenLoadResponse { get; set; }
        public ErrorResponse TreasuryErrorResponse { get; set; }


        public SoupkitchenState SoupkitchenState { get => _soupkitchenState; set => _soupkitchenState = value; }
        private SoupkitchenState DeserialiseSoupkitchenState(string serialisedSoupkitchenState)
        {
            using (StringReader sr = new StringReader(serialisedSoupkitchenState))
            {
                using (JsonReader reader = new JsonTextReader(sr)) 
                    return new JsonSerializer().Deserialize<SoupkitchenState>(reader);
            }
        }
        [SerializeField] private SoupkitchenState _soupkitchenState;
        #endregion

        public async Task<bool> SoupkitchenLoadAsync()
        {
            try
            {
                var response = await _soupkitchenService.SoupkitchenLoadAsync();
                if (response is SoupkitchenLoadResponse soupkitchenLoadResponse)
                {
                    SoupkitchenLoadResponse = soupkitchenLoadResponse;

                    SoupkitchenState = DeserialiseSoupkitchenState(soupkitchenLoadResponse.SoupkitchenState);
                    
                }
                else if (response is ErrorResponse errorResponse)
                {
                    TreasuryErrorResponse = errorResponse;
                    GameManager.Instance.ClearGameCache();
                    GameManager.Instance.NavToScene("btn_MainMenu_Scene");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                GameManager.Instance.ClearGameCache();
                GameManager.Instance.NavToScene("btn_MainMenu_Scene");
                return false;
            }
        }


        public void ClearSoupkitchenCache()
        {
            SoupkitchenLoadResponse = null;
            SoupkitchenState = null;
        }
    }
}
//
//[SerializeField] private long _treasuryTotalCoin;

//public CoinBag[] TreasuryCoinBag { get => _treasuryCoinBag; set => _treasuryCoinBag = value; }
//public long TreasuryTotalCoin { get => _treasuryTotalCoin; set => _treasuryTotalCoin = value; }