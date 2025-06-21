using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;
using System;

using Assets.Scripts.ClientManagers.User;
using Assets.Scripts.ClientManagers.Treasury;
using Assets.Scripts.ClientManagers.Soupkitchen;
using Assets.Scripts.ClientManagers.Battleboard;
using Assets.Scripts.ClientManagers.Character;
using Assets.Scripts.ClientManagers.Kingdom;
using Assets.Scripts.ClientManagers.Armoury;
using System.Collections.Generic;
using MonoMonarchGameFramework.Game.Kingdom.Nodes;
using MonoMonarchGameFramework.Game.Kingdom;
using MonoMonarchNetworkFramework;
using Newtonsoft.Json.Linq;
using System.Linq;
using Unity.VisualScripting;
using MonoMonarchGameFramework.Game.Treasury;

using TMPro.EditorUtilities;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Grassland;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.ClientManagers.Game
{
    public class GameManager : MonoBehaviour
    {
        #region Game Singleton
        private static GameManager _instance;
        [SerializeField] private static IGameService _gameService { get; set; } //can't serialise interface - remove

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                        _instance = singletonObject.AddComponent<GameManager>();
                        _gameService = new GameService();
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


        #region Game Event Listeners
        private UnityEvent<int[]> buildEvent;
        public UnityEvent<int[]> BuildEvent { get => buildEvent; set => buildEvent = value; }
        // private UnityEvent serverUpdateEvent;
        // public UnityEvent ServerUpdateEvent { get => serverUpdateEvent; set => serverUpdateEvent = value; }
        public void InitialiseUnityEvents()
        {
            BuildEvent = new UnityEvent<int[]>();
            BuildEvent.AddListener(SignalZonedMapAddEvent);
            // ServerUpdateEvent = new UnityEvent();
            // ServerUpdateEvent.AddListener(SignalUpdateBuildUI);
        }

        void SignalZonedMapAddEvent(int[] nodeIndexes)
        {
            List<BaseNode> zonedNodesListForAdd = new List<BaseNode>();
            List<BaseNode> zonedNodesListForRemove = new List<BaseNode>();

            foreach (int i in nodeIndexes)
            {
                if (KingdomManager.Instance.Map[i].NodeType == KingdomManager.Instance.GetSelectedBuildingState())
                {//IF proposed node is EQUAL to Map node, remove it from ZonedMap, ELSE do nothing
                    if (KingdomManager.Instance.ZonedMapDict.TryGetValue(i, out var nodeToRemove))
                        zonedNodesListForRemove.Add(nodeToRemove);
                }
                else if (KingdomManager.Instance.ZonedMapDict.TryGetValue(i, out var nodeToReplace))
                {//IF proposed node is NOT EQUAL to ZonedMap node, remove it from ZonedMap and add the new one
                    zonedNodesListForRemove.Add(nodeToReplace);
                    zonedNodesListForAdd.Add(KingdomManager.Instance.GetSelectedBaseNodeZoning(i));
                }//IF proposed node is NOT EQUAL to ZonedMap node, AND ZonedMap does not contain it, add the new one
                else
                {
                    zonedNodesListForAdd.Add(KingdomManager.Instance.GetSelectedBaseNodeZoning(i));
                }

                //if ()
            }

            //actions removal and addition of zoned nodes in ZonedMapDict - also handles node type num tracking 
            KingdomManager.Instance.RemoveNodesZonedMap(zonedNodesListForRemove);
            KingdomManager.Instance.AddNodesZonedMap(zonedNodesListForAdd);


            if (!KingdomManager.Instance.IsZoningMode)
            {
                Debug.Log($"SZMAE #1");
                KingdomManager.Instance.ToggleZoning(true);
            }
            else
            {
                /// Only reset visuals for removed nodes that don't appear in zonedNodesListForAdd list
                var addNodeIndices = new HashSet<int>(zonedNodesListForAdd.Select(n => n.NodeIndex));
                foreach (var removedNode in zonedNodesListForRemove)
                {
                    if (!addNodeIndices.Contains(removedNode.NodeIndex))
                    {
                        var mapNode = KingdomManager.Instance.Map[removedNode.NodeIndex];
                        var nodeType = mapNode.NodeType;
                        KingdomManager.Instance.NodeList[nodeType][removedNode.NodeIndex].GetComponent<MeshRenderer>().material.color = KingdomManager.Instance.NodeColours[nodeType];
                    }
                }///

                //continue distinguishing remaining newly altered zoned nodes
                KingdomManager.Instance.DistinguishZoningNodes(zonedNodesListForAdd.Select(node => node.NodeIndex).ToArray());


                Color flareMat;
                if (TreasuryManager.IsSufficientCoin(KingdomManager.Instance.ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()))
                {
                    flareMat = KingdomManager.Instance.FlareMatGreen.GetComponent<Color>();
                    if (KingdomManager.Instance.FlareDict[nodeIndexes[0]].GetComponent<Color>() != flareMat)
                    {   //if flares were red, redraw them all as green
                        foreach (int nodeId in KingdomManager.Instance.ZonedMapDict.Keys)
                        {
                            KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
                        }
                        Debug.Log($"SZMAE #2");
                    }
                    else
                    {   //if the flare is already green, only redraw the added zoned node's flares
                        foreach (int nodeId in nodeIndexes)
                        {
                            KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
                        }

                        Debug.Log($"SZMAE #3");
                    }

                }
                else if (!TreasuryManager.IsSufficientCoin(KingdomManager.Instance.ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()))
                {
                    flareMat = KingdomManager.Instance.FlareMatRed.GetComponent<Color>();
                    if (KingdomManager.Instance.FlareDict[nodeIndexes[0]].GetComponent<Color>() != flareMat)
                    {   //if flares were green, redraw them all as red
                        foreach (int nodeId in KingdomManager.Instance.ZonedMapDict.Keys)
                        {
                            if (KingdomManager.Instance.FlareDict[nodeId].activeSelf == false)
                                KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
                        }

                        Debug.Log($"SZMAE #4");
                    }
                    else
                    {   //if the flare is already red, only redraw the added zoned node's flares
                        foreach (int nodeId in nodeIndexes)
                        {
                            if (KingdomManager.Instance.FlareDict[nodeId].activeSelf == false)
                                KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
                        }

                        Debug.Log($"SZMAE #5");
                    }
                }
            }
            Debug.Log($"Zoning cost prior to changes: {TreasuryManager.Instance.ZoningCost}");
            TreasuryManager.Instance.SubtractZoningCost(zonedNodesListForRemove);
            TreasuryManager.Instance.AddZoningCost(zonedNodesListForAdd);
            Debug.Log($"Zoning cost total: {TreasuryManager.Instance.ZoningCost}, Player coin total: {TreasuryManager.Instance.TreasuryState.GetTotalCoin()}");
        }


        #endregion


        public async Task<bool> LoadGameState()
        {
            if (!await TreasuryManager.Instance.TreasuryLoadAsync()) return false;
            if (!await SoupkitchenManager.Instance.SoupkitchenLoadAsync()) return false;
            if (!await CharacterManager.Instance.CharacterLoadAsync()) return false;
            if (!await KingdomManager.Instance.KingdomLoadAsync()) return false;
            if (!await ArmouryManager.Instance.ArmouryLoadAsync()) return false;
            // if (!await BattleboardManager.Instance.BattleboardLoadAsync()) return false;

            //KingdomManager.Instance.KingdomMapGenerate();

            Debug.Log("Game state loaded successfully");
            return true;
        }

        public void NavToScene(string sceneName)
        {
            SceneManager.LoadScene(_gameService.ResolveScene(sceneName));
            Debug.Log($"Navigated to scene: {sceneName}");
        }
        public void ClearGameCache()
        {
            ArmouryManager.Instance.ClearArmouryCache();
            //BattleboardManager.Instance.ClearBattleboardCache();
            CharacterManager.Instance.ClearCharacterCache();
            KingdomManager.Instance.ClearKingdomCache();
            SoupkitchenManager.Instance.ClearSoupkitchenCache();
            TreasuryManager.Instance.ClearTreasuryCache();
            UserManager.Instance.ClearUserCache();
        }
    }

    public static class NetworkConstants
    {
        private static readonly string MM_API_URL_DEV_HTTP = "http://localhost:5223";
        private static readonly string MM_API_URL_DEV_HTTPS = "https://localhost:7061"; // certs?
        private static readonly string MM_API_URL_PROD = "";

        public static string GetServerURLPrefix()
        {
            return MM_API_URL_DEV_HTTP;
        }
    }
}


//public static int CalculateNodeId(int x, int y)
//{
//    return (y - 1) * 60 + (x - 1);
//}

////public void TEST()
////{
////    BuildEvent.Invoke(new int[2] { 2, 2 });
////}

//public static (int[], int[]) CalculateNodePos(int[] iNodeArray)
//{
//    int[] xNodeArray = new int[iNodeArray.Length];
//    int[] yNodeArray = new int[iNodeArray.Length];
//    foreach (int i in iNodeArray)
//    {
//        xNodeArray[i] = i % 60 + 1;
//        yNodeArray[i] = i / 60 + 1;
//    }
//    return (xNodeArray, yNodeArray);
//}

//public static int[] CalculateNodeId(int[] xArray, int[] yArray)
//{
//    int[] iArray = new int[xArray.Length];
//    for (int i = 0; i < xArray.Length; i++)
//    {
//        iArray[i] = (yArray[i] - 1) * 60 + (xArray[i] - 1);
//    }
//    return iArray;
//}
/// <summary>
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>
/// <param name="nodeIndexes"></param>



// void SignalUpdateBuildUI()
// {
//     UpdateBuildUI();
// }
// private void UpdateBuildUI()
//  {
//     
//
// }



//switch (KingdomManager.Instance.Map[i].NodeType)
//   {
//       case 0://grassland

//           break;
//       case 1://citycentre

//           break;
//       case 2://house

//           break;
//       case 3://library

//           break;
//       case 4://factory

//           break;
//       case 5://mtower

//           break;
//       case 6://road

//           break;
//       case 7://blockade

//           break;
//       case 8://wonder

//           break;

//   }
//public void GameRulesCheck(int[] xPosition, int[] yPosition)
//{
//    for (int i = 0; i < xPosition.Length; i++)
//    {
//        if (KingdomManager.Instance.IsInvalid(KingdomState.CalculateNodeId(xPosition[i], yPosition[i])))
//            continue;
//        if (!KingdomState.ValidateBlockadeRoadRule(KingdomManager.Instance.Map[i], KingdomManager.Instance.GetSelectedBuildingState() ? )
//            continue;
//        //if (TreasuryManager.Instance.ZoningCost)
//    }
//}


// KingdomManager.Instance.ZonedNumNodeTypes[focusedNode.NodeType]--;



//TreasuryManager.Instance.SubtractZoningCost(zonedno)//KingdomManager.Instance.SubtractNumNodeTypes(nodeIndexes.Length);
//new Grassland { NodeIndex = i, NodeCost = 0, NodeLevel = 0, NodeType = 0 }

//KingdomManager.Instance.ZonedNumNodeTypes[KingdomManager.Instance.GetSelectedBuildingState()]++;

// KingdomManager.Instance.AddNodesZonedMap(zonedNodesListForBuy);
// KingdomManager.Instance.RemoveNodesZonedMap(zonedNodesListForBuy);
//sell existing node in ZonedMap
//zonedNodesListForBuy.Remove(zonedNode);

// KingdomManager.Instance.ZonedNumNodeTypes[zonedNode.NodeType]--;


//buy new node to add to ZonedMap
//foreach (int i in nodeIndexes)
//{
//    if (!GameRulesCheck(xNodePosArray, yNodePosArray))
//    {

//        continue;
//    }
//    if (TreasuryManager.Instance.IsSufficientCoin(TreasuryManager.Instance.ZoningCost))
//        continue;
//}
//public bool GameRulesCheckAbsolute(int[] xPosition, int[] yPosition)
//{
//    for (int i = 0; i < xPosition.Length; i++)
//    {
//        if (KingdomManager.Instance.IsInvalid(KingdomState.CalculateNodeId(xPosition[i], yPosition[i])))
//            continue;
//        if (!KingdomState.ValidateBlockadeRoadRule(KingdomManager.Instance.Map[i], KingdomManager.Instance.GetSelectedBuildingState() ? )
//            continue;
//        //if (TreasuryManager.Instance.ZoningCost)

//    }
//}

/// <summary>
/// 
/// </summary>
/// <param name="iArray"></param>
/// <returns>
/// (success?,errorcode = 0||1||2||3 == success,errorPos1,errorPos2,errorPos3,  corrospond to error code's placement in code
/// </returns>
/// 
//buy new node to add to ZonedMap

// KingdomManager.Instance.ZonedNumNodeTypes[KingdomManager.Instance.GetSelectedBuildingState()]++;
// TreasuryManager.Instance.ZoningCost -= KingdomManager.Instance.Map[i].NodeCost / 2; //removes prior refunds for ZoningCost calculation

//if (KingdomManager.Instance.GetSelectedBuildingState() is (int)NodeTypeEnum.Grassland)
//{

//}
//else if (KingdomManager.Instance.Map[i].NodeCost > 0)///accounting for Map's Node cost refund in ZonedMap 
//{


//}
//
//#nullable enable

//        public (bool, int?, Dictionary<int, int>?) GameRulesDecorator(int[] iArray)
//        {
//            Dictionary<int, (int, string)> indexErrorCodes = new Dictionary<int, (int, string)>();

//            foreach (int i in iArray)
//            {

//                if (KingdomManager.Instance.ZonedMapDict.key)
//                {

//                }
//                switch (KingdomManager.Instance.ZonedMapDict[i].NodeType)
//                {
//                    case 0://grassland

//                        break;
//                    case 1://citycentre

//                        break;
//                    case 2://house

//                        break;
//                    case 3://library

//                        break;
//                    case 4://factory

//                        break;
//                    case 5://mtower

//                        break;
//                    case 6://road

//                        break;
//                    case 7://blockade

//                        break;
//                    case 8://wonder

//                        break;

//                }

//                if (KingdomManager.Instance.IsInvalid(i))
//                    indexErrorCodes.Add(i, (1, "Invalid"));
//                if (!KingdomState.ValidateBlockadeRoadRule(KingdomManager.Instance.Map[i].NodeType, KingdomManager.Instance.GetSelectedBuildingState()))
//                    indexErrorCodes.Add(i, (2, "Blockade"));

//                if (i == KingdomManager.Instance.ZonedMapDict[i].NodeIndex)
//                    indexErrorCodes.Add(i, (3,));

//            }
//            if (!KingdomState.ValidateBuildActionByNumOfBuildings(KingdomManager.Instance.KingdomState.NumNodeTypes))
//            {

//            }
//            if ()
//            {

//            }
//            return (true, null, null);
//        }


//#nullable disable