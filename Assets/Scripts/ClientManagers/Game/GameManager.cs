using Assets.Scripts.ClientManagers.Armoury;
using Assets.Scripts.ClientManagers.Battleboard;
using Assets.Scripts.ClientManagers.Character;
using Assets.Scripts.ClientManagers.Game.Loading;
using Assets.Scripts.ClientManagers.Kingdom;
using Assets.Scripts.ClientManagers.Soupkitchen;
using Assets.Scripts.ClientManagers.Treasury;
using Assets.Scripts.ClientManagers.User;
using Assets.Scripts.Interactables;
using JetBrains.Annotations;
using MonoMonarchGameFramework.Game.Kingdom;
using MonoMonarchGameFramework.Game.Kingdom.Nodes;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Grassland;
using MonoMonarchGameFramework.Game.Treasury;
using MonoMonarchNetworkFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



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

        public void InitialiseUnityEvents()
        {
            BuildEvent = new UnityEvent<int[]>();
            BuildEvent.AddListener(SignalZonedMapAddEvent);
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
                    {
                        zonedNodesListForRemove.Add(nodeToRemove);
                        int nodeTypeMap = KingdomManager.Instance.Map[i].NodeType;
                        KingdomManager.Instance.NodeList[nodeTypeMap][i].GetComponent<MeshRenderer>().material = KingdomManager.Instance.NodeMaterials[nodeTypeMap];
                    }
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

            //actions removal and addition of zoned nodeArray in ZonedMapDict - also handles node type num tracking 
            KingdomManager.Instance.RemoveNodesZonedMap(zonedNodesListForRemove);
            KingdomManager.Instance.AddNodesZonedMap(zonedNodesListForAdd);


            if (!KingdomManager.Instance.IsZoningMode)
            {
                Debug.Log($"SZMAE #1");
                KingdomManager.Instance.ToggleZoning();
            }
            else
            {
                //continue distinguishing remaining newly altered zoned nodeArray
                Debug.Log(KingdomManager.Instance.IsZoningMode);
                Debug.Log("Accessing DistinguishZoningNodes() 1");
                KingdomManager.Instance.DistinguishZoningNodes(zonedNodesListForAdd.Select(node => node.NodeIndex).ToArray(),0.75f);

                Material flareMat;
                if (TreasuryManager.IsSufficientCoin(KingdomManager.Instance.ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()))
                {
                    flareMat = KingdomManager.Instance.FlareMatGreen;//color;
                    if (KingdomManager.Instance.FlareDict[nodeIndexes[0]].GetComponent<MeshRenderer>().material != flareMat)
                    {   //if flares were red, redraw them all as green
                        foreach (int nodeId in KingdomManager.Instance.ZonedMapDict.Keys)
                        {
                            KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material = flareMat;
                        }
                        Debug.Log($"SZMAE #2");
                    }
                    else
                    {   //if the flare is already green, only redraw the added zoned node's flares
                        foreach (int nodeId in nodeIndexes)
                        {
                            KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material = flareMat;
                        }

                        Debug.Log($"SZMAE #3");
                    }

                }
                else if (!TreasuryManager.IsSufficientCoin(KingdomManager.Instance.ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()))
                {
                    flareMat = KingdomManager.Instance.FlareMatRed;
                    if (KingdomManager.Instance.FlareDict[nodeIndexes[0]].GetComponent<MeshRenderer>().material != flareMat)
                    {   //if flares were green, redraw them all as red
                        foreach (int nodeId in KingdomManager.Instance.ZonedMapDict.Keys)
                        {
                            if (KingdomManager.Instance.FlareDict[nodeId].activeSelf == false)
                                KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material = flareMat;
                        }

                        Debug.Log($"SZMAE #4");
                    }
                    else
                    {   //if the flare is already red, only redraw the added zoned node's flares
                        foreach (int nodeId in nodeIndexes)
                        {
                            if (KingdomManager.Instance.FlareDict[nodeId].activeSelf == false)
                                KingdomManager.Instance.FlareDict[nodeId].SetActive(true);
                            KingdomManager.Instance.FlareDict[nodeId].GetComponent<MeshRenderer>().material = flareMat;
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
        private int nodeIdSelected = -1;
        public int NodeIdSelected { get => nodeIdSelected; set => nodeIdSelected = value; }

        #endregion


        public async Task<bool> LoadGameState()
        {
            await LoadingScreenExtensions.LoadLoadingSceneAsync();

            await LoadingScreenExtensions.LoadGameSceneAsync();

            var loadingScreen = FindFirstObjectByType<LoadingScreen>().gameObject.GetComponent<LoadingScreen>();
            loadingScreen.StagesCompleted = new bool[6] { false, false, false, false, false, false }; // change to 6 after battleboard is implemented


            loadingScreen.UpdateInfo("Loading Assets From Server...");
            await Task.Delay(1000);

            if (!await TreasuryManager.Instance.TreasuryLoadAsync()) return false;
            else
            {
                loadingScreen.StagesCompleted[0] = true;
                loadingScreen.IncrementStagesCompleted();
                await Task.Delay(500);
            }
            if (!await SoupkitchenManager.Instance.SoupkitchenLoadAsync()) return false;
            else
            {
                loadingScreen.StagesCompleted[1] = true;
                loadingScreen.IncrementStagesCompleted();
                await Task.Delay(500);
            }
            if (!await CharacterManager.Instance.CharacterLoadAsync()) return false;
            else
            {
                loadingScreen.StagesCompleted[2] = true;
                loadingScreen.IncrementStagesCompleted();
                await Task.Delay(500);
            }
            if (!await KingdomManager.Instance.KingdomLoadAsync()) return false;
            else
            {
                loadingScreen.StagesCompleted[3] = true;
                loadingScreen.IncrementStagesCompleted();
                await Task.Delay(500);
            }
            if (!await ArmouryManager.Instance.ArmouryLoadAsync()) return false;
            else
            {
                loadingScreen.StagesCompleted[4] = true;
                loadingScreen.IncrementStagesCompleted();
                await Task.Delay(500);
            }

            loadingScreen.StagesCompleted[5] = true;
            loadingScreen.IncrementStagesCompleted();

            loadingScreen.UpdateInfo("Load State Success");

            Debug.Log("Game state loaded successfully");

            await Task.Delay(2000);
            await LoadingScreenExtensions.UnloadMainMenuSceneAsync();
            await LoadingScreenExtensions.UnloadLoadingSceneAsync();
            SceneTracker.CurrentScene = 1; //kingdom

            return true;
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
            SceneTracker.CurrentScene = 0; //reset to MainMenu
        }
        public GameObject MainMenu { get => menu; set => menu = value; }
        public GameObject Kingdom { get => map; set => map = value; }
        public GameObject Soupkitchen { get => soup; set => soup = value; }
        public GameObject Bazaar { get => bazaar; set => bazaar = value; }
        public GameObject Battleboard { get => battle; set => battle = value; }
        public GameObject Character { get => monarch; set => monarch = value; }
        private GameObject monarch, map, soup, bazaar, battle, menu;

        public GameObject PoliticalPoints { get => pp; set => pp = value; }
        public GameObject SoupTimer { get => timer; set => timer = value; }
        public GameObject Coin { get => coin; set => coin = value; }
        private GameObject coin, pp, timer;

        public void SetGameUICanvas()
        {
            SetBottomPanelUI();
            SetLeftPanelUI();
        }
        public void SetKingdomUICanvas()
        {
            KingdomManager.Instance.SetRightPanelUI();
            KingdomManager.Instance.SetTopPanelUI();
        }
        public void SetCharacterUICanvas()
        {
            //BazaarManager.Instance.SetRightPanelUI();
            //BazaarManager.Instance.SetTopPanelUI();
        }
        public void SetBazaarUICanvas()
        {
            //CharacterManager.Instance.SetRightPanelUI();
            //CharacterManager.Instance.SetTopPanelUI();
        }
        public void SetBattleboardUICanvas()
        {
            //CharacterManager.Instance.SetRightPanelUI();
            //CharacterManager.Instance.SetTopPanelUI();
        }
        public void SetSoupkitchenUICanvas()
        {
            //SoupkitchenManager.Instance.SetRightPanelUI();
            //SoupkitchenManager.Instance.SetTopPanelUI();
        }
        //CanvasGameComponents
        public GameObject CanvasGameComponents { get => canvasGameComponents; set => canvasGameComponents = value; }
        private GameObject canvasGameComponents;
        public GameObject CanvasGame { get => canvasGame; set => canvasGame = value; }
        private GameObject canvasGame;
        public GameObject CanvasKingdom { get => canvasKingdom; set => canvasKingdom = value; }
        public GameObject CanvasCharacter { get => canvasCharacter; set => canvasCharacter = value; }
        public GameObject CanvasSoupkitchen { get => canvasSoup; set => canvasSoup = value; }
        public GameObject CanvasBazaar { get => canvasbazaar; set => canvasbazaar = value; }
        public GameObject CanvasBattleboard { get => canvasbattle; set => canvasbattle = value; }
        private GameObject canvasKingdom,canvasCharacter,canvasSoup,canvasbazaar,canvasbattle;
        public void InitialiseGameCanvas()
        {
            CanvasGameComponents = GameObject.Find("CanvasGameComponents");
            CanvasGame = CanvasGameComponents.GetComponentInChildren<Transform>().Find("GameComponents").gameObject;
            CanvasKingdom = CanvasGameComponents.GetComponentInChildren<Transform>().Find("Kingdom").gameObject ;
            CanvasCharacter = CanvasGameComponents.GetComponentInChildren<Transform>().Find("Character").gameObject;
            CanvasSoupkitchen = CanvasGameComponents.GetComponentInChildren<Transform>().Find("Soupkitchen").gameObject;
            CanvasBazaar = CanvasGameComponents.GetComponentInChildren<Transform>().Find("Bazaar").gameObject;
            CanvasBattleboard = CanvasGameComponents.GetComponentInChildren<Transform>().Find("Battleboard").gameObject;
            CanvasGame.SetActive(true);
            CanvasKingdom.SetActive(true);
            SceneTracker.CurrentScene = 1;
            CanvasCharacter.SetActive(false);
            CanvasSoupkitchen.SetActive(false);
            CanvasBazaar.SetActive(false);
            CanvasBattleboard.SetActive(false);
        }
        /// <summary>
        /// SceneTracker.CurrentScene
        /// </summary>
        /// <param name="i"></param>
        public void ClearCanvas(int i)
        {
            switch (i)
            {
                case 1://kingdom
                    CanvasKingdom.SetActive(false);
                    break;
                case 2://character
                    CanvasCharacter.SetActive(false);
                    break;
                case 3://soup
                    CanvasSoupkitchen.SetActive(false);
                    break;
                case 4://bazaar
                    CanvasBazaar.SetActive(false);
                    break;
                case 5://battle
                    CanvasBattleboard.SetActive(false);
                    break;

                case 0://mainmenu

                    break;
            }
        }



        public void UpdateBottomPanelUI()
        {
            Coin.GetComponent<TextMeshPro>().text = TreasuryManager.Instance.TreasuryState.GetTotalCoin().ToString();
            PoliticalPoints.GetComponent<TextMeshPro>().text = CharacterManager.Instance.CharacterState.PoliticalPoints.ToString();
            // SoupTimer.GetComponent<TextMeshPro>().text = SoupkitchenManager.Instance;
        }
        public void SetBottomPanelUI()
        {
            PoliticalPoints = GameObject.Find("tmp_PoliticalPoints");
            SoupTimer = GameObject.Find("tmp_SoupTimer");
            Coin = GameObject.Find("tmp_Coin");
        }

        public void SetLeftPanelUI()
        {
            Kingdom = GameObject.Find("btn_KingdomScene");
            Character = GameObject.Find("btn_CharacterScene");
            Soupkitchen = GameObject.Find("btn_SoupkitchenScene");
            Bazaar = GameObject.Find("btn_BazaarScene");
            Battleboard = GameObject.Find("btn_BattleboardScene");
            MainMenu = GameObject.Find("btn_MainMenuScene");

            Kingdom.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(1));
            Character.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(2));
            Soupkitchen.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(3));
            Bazaar.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(4));
            Battleboard.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(5));
            MainMenu.GetComponent<Button>().onClick.AddListener(() => NavigateToUICanvas(0));
        }


        /// <summary>
        /// k,c,sk,b,b
        /// </summary>
        /// <param name="i"></param>
        public void NavigateToUICanvas(int i)
        {
            ClearCanvas(SceneTracker.CurrentScene);
            Debug.Log($"ui update from {SceneTracker.CurrentScene} to {i}");
            switch (i)
            {
                case 1://kingdom
                    CanvasKingdom.SetActive(true);
                    SceneTracker.CurrentScene = 1;
                    break;
                case 2://character
                    CanvasCharacter.SetActive(true);
                    SceneTracker.CurrentScene = 2;
                    break;
                case 3://soup
                    CanvasSoupkitchen.SetActive(true);
                    SceneTracker.CurrentScene = 3;
                    break;
                case 4://bazaar
                    CanvasBazaar.SetActive(true);
                    SceneTracker.CurrentScene = 4;
                    break;
                case 5://battle
                    CanvasBattleboard.SetActive(true);
                    SceneTracker.CurrentScene = 5;
                    break;
                case 0:
                    //mainmenu

                    break;
            }
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