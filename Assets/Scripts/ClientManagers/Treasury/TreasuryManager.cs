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
using MonoMonarchNetworkFramework.Game.Treasury;
using Assets.Scripts.ClientManagers.User;
using MonoMonarchGameFramework.Game.Treasury.GoldBag;
using MonoMonarchGameFramework.Game.Treasury;
using static System.Collections.Specialized.BitVector32;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using MonoMonarchGameFramework.Game.Kingdom.Nodes;
using MonoMonarchGameFramework.Game.Kingdom;

namespace Assets.Scripts.ClientManagers.Treasury
{
    public class TreasuryManager : MonoBehaviour
    {
        #region Treasury Singleton
        private static ITreasuryService _treasuryService { get; set; }
        private static TreasuryManager _instance;
        public static TreasuryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<TreasuryManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(TreasuryManager).Name);
                        _instance = singletonInstance.AddComponent<TreasuryManager>();
                        _treasuryService = new TreasuryService();
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


        #region Treasury Properties
        public TreasuryLoadResponse TreasuryLoadResponse { get; set; }
        public ErrorResponse TreasuryErrorResponse { get; set; }

        public TreasuryState TreasuryState { get => _treasuryState; set => _treasuryState = value; }
        private TreasuryState DeserialiseTreasuryState(string serialisedTreasuryState)
        {
            using (StringReader sr = new StringReader(serialisedTreasuryState))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                    return new JsonSerializer().Deserialize<TreasuryState>(reader);
            }
        }
        [SerializeField] private TreasuryState _treasuryState;
        #endregion


        public async Task<bool> TreasuryLoadAsync()
        {
            try
            {
                var response = await _treasuryService.TreasuryLoadAsync();
                if (response is TreasuryLoadResponse treasuryLoadResponse)
                {
                    TreasuryLoadResponse = treasuryLoadResponse;

                    TreasuryState = DeserialiseTreasuryState(TreasuryLoadResponse.TreasuryState);
                }
                else if (response is ErrorResponse errorResponse)
                {
                    TreasuryErrorResponse = errorResponse;
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

        public void ClearTreasuryCache()
        {
            TreasuryLoadResponse = null;
            TreasuryState = null;
            ZoningCost = 0;
        }


        #region Treasury Zoning
        [SerializeField] private BigInteger zoningCost;
        public BigInteger ZoningCost { get { return zoningCost; } set { zoningCost = value; } }
        public void AddZoningCost(List<BaseNode> nodeList)
        {
            foreach (BaseNode node in nodeList)
                ZoningCost += node.NodeCost;
        }
        public void SubtractZoningCost(List<BaseNode> nodeList)
        {
            foreach (BaseNode node in nodeList)
                ZoningCost -= node.NodeCost;
        }
        public void ZoningTextColourRed(MeshRenderer mr)
        {

        }
        #endregion


        #region Treasury Tools
        public static bool IsSufficientCoin(int[] nodeTypesTotalArray, BigInteger totalCoin) //NodeType int representations GL==0, TC==1, H==2, L==3, F==4, R==5, B==6, MT==7, W==8
        {
            BigInteger totalCost = 0;

            for (int i = 0; i < nodeTypesTotalArray.Length; i++)
            {
                totalCost += (int)((NodeCostEnum)i) * nodeTypesTotalArray[i];
            }

            //for (int i = 0; i < nodeTypesTotalArray.Length; i++)
            //{
            //    switch (i)
            //    {
            //        case 0: // grassland
            //            totalCost += (int)NodeCostEnum.Grassland * nodeTypesTotalArray[i];
            //            break;
            //        case 1: // towncentre
            //            totalCost += (int)NodeCostEnum.TownCentre * nodeTypesTotalArray[i];
            //            break;
            //        case 2: // house
            //            totalCost += (int)NodeCostEnum.House * nodeTypesTotalArray[i];
            //            break;
            //        case 3: // library
            //            totalCost += (int)NodeCostEnum.Library * nodeTypesTotalArray[i];
            //            break;
            //        case 4: // factory
            //            totalCost += (int)NodeCostEnum.Factory * nodeTypesTotalArray[i];
            //            break;
            //        case 5: // road
            //            totalCost += (int)NodeCostEnum.Road * nodeTypesTotalArray[i];
            //            break;
            //        case 6: // blockade
            //            totalCost += (int)NodeCostEnum.Blockade * nodeTypesTotalArray[i];
            //            break;
            //        case 7: // mtower
            //            totalCost += (int)NodeCostEnum.MTower * nodeTypesTotalArray[i];
            //            break;
            //        case 8: // wonder
            //            totalCost += (int)NodeCostEnum.Wonder * nodeTypesTotalArray[i];
            //            break;
            //    }
            //}

            return true ? totalCost <= totalCoin : false;
        }
        #endregion


    }
}
