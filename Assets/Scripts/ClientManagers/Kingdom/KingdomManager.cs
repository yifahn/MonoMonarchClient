using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Unity.VisualScripting;

using Newtonsoft.Json;

using Assets.Scripts.ClientManagers.Game;

using MonoMonarchNetworkFramework;
using MonoMonarchNetworkFramework.Game.Kingdom;
using MonoMonarchNetworkFramework.Game.Soupkitchen;

using MonoMonarchGameFramework.Game;
using MonoMonarchGameFramework.Game.Kingdom;
using MonoMonarchGameFramework.Game.Kingdom.Nodes;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.TownCentre;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Grassland;

using MonoMonarchGameFramework.Game.Kingdom.Nodes.House;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Library;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Factory;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Road;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Blockade;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.MTower;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Wonder;
using Assets.Scripts.ClientManagers.Treasury;
using MonoMonarchNetworkFramework.Game.Kingdom.Map;


namespace Assets.Scripts.ClientManagers.Kingdom
{
    public class KingdomManager : MonoBehaviour
    {
        #region Kingdom Singleton
        private static IKingdomService _kingdomService { get; set; }
        private static KingdomManager _instance;
        public static KingdomManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<KingdomManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(KingdomManager).Name);
                        _instance = singletonInstance.AddComponent<KingdomManager>();
                        _kingdomService = new KingdomService();
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


        #region Kingdom Properties
        public KingdomMapUpdateResponse KingdomMapUpdateResponse { get; set; }
        public KingdomLoadResponse KingdomLoadResponse { get; set; }
        public ErrorResponse KingdomErrorResponse { get; set; }

        public BaseNode[] Map { get => map; set => map = value; }
        private BaseNode[] DeserialiseMap(string serialisedMap)
        {
            JsonSerializer serialiser = new JsonSerializer();
            serialiser.Converters.Add(new DeserialisationSupport());
            using (StringReader sr = new StringReader(serialisedMap))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return serialiser.Deserialize<BaseNode[]>(reader);
                }
            }
        }
        [SerializeField] private BaseNode[] map;

        public KingdomState KingdomState { get => kingdomState; set => kingdomState = value; }
        private KingdomState DeserialiseState(string serialisedState)
        {
            JsonSerializer serialiser = new JsonSerializer();
            using (StringReader sr = new StringReader(serialisedState))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return serialiser.Deserialize<KingdomState>(reader);
                }
            }
        }
        [SerializeField] private KingdomState kingdomState;
        #endregion


        public void ClearKingdomCache()
        {
            KingdomLoadResponse = null;
            KingdomState = null;
            ZonedMapDict = null;
            Map = null;
        }

        public async Task<bool> KingdomLoadAsync()
        {
            try
            {
                var response = await _kingdomService.KingdomLoadAsync();
                if (response is KingdomLoadResponse kingdomLoadResponse)
                {
                    KingdomLoadResponse = kingdomLoadResponse;
                    Map = DeserialiseMap(KingdomLoadResponse.KingdomMap);
                    KingdomState = DeserialiseState(KingdomLoadResponse.KingdomState);
                }
                else if (response is ErrorResponse errorResponse)
                {
                    KingdomErrorResponse = errorResponse;
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
        public async Task<bool> KingdomMapUpdateAsync(KingdomMapUpdatePayload payload)
        {
            try
            {
                var response = await _kingdomService.KingdomMapUpdateAsync(payload);
                if (response is KingdomMapUpdateResponse kingdomMapUpdateResponse)
                {
                    KingdomMapUpdateResponse = kingdomMapUpdateResponse;

                }
                else if (response is ErrorResponse errorResponse)
                {
                    KingdomErrorResponse = errorResponse;
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


        #region Map Zoning
        [SerializeField] private Dictionary<int, BaseNode> zonedMapDict; //index , BaseNode
        [SerializeField] private int[] zonedNumNodeTypes = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        [SerializeField] private Color[] nodeColours;
        [SerializeField] private bool isZoningMode;

        public Dictionary<int, BaseNode> ZonedMapDict { get => zonedMapDict; set => zonedMapDict = value; }
        public int[] ZonedNumNodeTypes { get => zonedNumNodeTypes; set => zonedNumNodeTypes = value; }
        public Color[] NodeColours { get => nodeColours; set => nodeColours = value; }
        public bool IsZoningMode { get => isZoningMode; set => isZoningMode = value; }

        private Dictionary<int, GameObject> flareDict;
        public Dictionary<int, GameObject> FlareDict { get { return flareDict; } set { flareDict = value; } }

        private GameObject flare;
        private Material flareMatRed, flareMatGreen;
        public GameObject Flare { get { return flare; } set { flare = value; } }
        public Material FlareMatRed { get { return flareMatRed; } set { flareMatRed = value; } }
        public Material FlareMatGreen { get { return flareMatGreen; } set { flareMatGreen = value; } }

        public void ToggleZoning(bool enableZoningMode)
        {
            if (enableZoningMode && !IsZoningMode) //on
            {
                IsZoningMode = true;

                //alter node colours to visually describe proposed zoning changes
                DistinguishZoningNodes(ZonedMapDict.Keys.ToArray());

                //set flare colour to opaque green if sufficient coin, else opaque red 
                Color flareMat = TreasuryManager.IsSufficientCoin(ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()) ? FlareMatGreen.GetComponent<Color>() : FlareMatRed.GetComponent<Color>();
                foreach (int nodeId in ZonedMapDict.Keys)
                    FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
            }
            else if (!enableZoningMode && IsZoningMode) //off
            {
                IsZoningMode = false;
                foreach (BaseNode node in ZonedMapDict.Values)
                {
                    //reset node colour to Map's origin
                    NodeList[Map[node.NodeIndex].NodeType][node.NodeIndex].GetComponent<MeshRenderer>().material.color = nodeColours.ElementAt(node.NodeType);

                    //reset flare colour to opaque red
                    Color redOpaque = new Color(FlareMatRed.color.r, FlareMatRed.color.g, FlareMatRed.color.b, 0);
                    FlareDict[node.NodeIndex].GetComponent<MeshRenderer>().material.color = redOpaque;
                }
            }
        }
        public BaseNode GetSelectedBaseNodeZoning(int nodeIndex)
        {

            switch (GetSelectedBuildingState())
            {
                case 1:
                    return new TownCentre { NodeCost = (int)NodeCostEnum.TownCentre, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.TownCentre };
                case 2:
                    return new House { NodeCost = (int)NodeCostEnum.House, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.House };
                case 3:
                    return new Library { NodeCost = (int)NodeCostEnum.Library, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Library };
                case 4:
                    return new Factory { NodeCost = (int)NodeCostEnum.Factory, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Factory };
                case 5:
                    return new Road { NodeCost = (int)NodeCostEnum.Road, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Road };
                case 6:
                    return new Blockade { NodeCost = (int)NodeCostEnum.Blockade, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Blockade };
                case 7:
                    return new MTower { NodeCost = (int)NodeCostEnum.MTower, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.MTower };
                case 8:
                    return new Wonder { NodeCost = (int)NodeCostEnum.Wonder, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Wonder };
                default:
                    return new Grassland { NodeCost = (int)NodeCostEnum.Grassland, NodeIndex = nodeIndex, NodeLevel = 0, NodeType = (int)NodeTypeEnum.Grassland };
            }
        }

        public void DistinguishZoningNodes(int[] nodeIdArray)
        {
            int[] nodeTypeArray = new int[nodeIdArray.Length];
            for (int i = 0; i < nodeIdArray.Length; i++)
            {
                nodeTypeArray[i] = ZonedMapDict.ElementAt(nodeIdArray[i]).Value.NodeType;
            }
            SetZonedOpacitySelection(nodeIdArray, 0.75f);
            SetZonedColourSelection(nodeIdArray, nodeTypeArray);
        }

        public void AddNodesZonedMap(List<BaseNode> zonedNodesList)
        {
            foreach (BaseNode zonedNode in zonedNodesList)
            {
                BaseNode node = GetSelectedBaseNodeZoning(zonedNode.NodeIndex);
                ZonedMapDict.Add(node.NodeIndex, node);
                ZonedNumNodeTypes[node.NodeType]++;
            }
        }

        public void RemoveNodesZonedMap(List<BaseNode> zonedNodesList)
        {
            foreach (BaseNode zonedNode in zonedNodesList)
            {
                BaseNode node = ZonedMapDict[zonedNode.NodeIndex];
                ZonedMapDict.Remove(node.NodeIndex);
                ZonedNumNodeTypes[node.NodeType]--;
            }
        }

        public void DiscardZonedMap()
        {
            if (IsZoningMode)
            {
                Color redOpaque = new Color(FlareMatRed.color.r, FlareMatRed.color.g, FlareMatRed.color.b, 0);

                int[] nodeIdArray = new int[ZonedMapDict.Count];
                for (int i = 0; i < ZonedMapDict.Count; i++)
                    nodeIdArray[i] = Map[i].NodeIndex;

                int[] nodeTypeArray = new int[ZonedMapDict.Count];
                for (int i = 0; i < ZonedMapDict.Count; i++)
                    nodeTypeArray[i] = Map[i].NodeType;

                foreach (BaseNode node in ZonedMapDict.Values)
                {
                    NodeList[Map[node.NodeIndex].NodeType][node.NodeIndex].gameObject.GetComponent<MeshRenderer>().material.color = nodeColours.ElementAt(node.NodeType);
                    FlareDict[node.NodeIndex].GetComponent<MeshRenderer>().material.color = redOpaque;
                }

                IsZoningMode = false;
            }

            TreasuryManager.Instance.ZoningCost = 0;
            ZonedNumNodeTypes = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ZonedMapDict = new Dictionary<int, BaseNode>();
        }

        public async Task CommitZonedMap()
        {
            int[] nodeIndexes = new int[ZonedMapDict.Count];
            int[] nodeTypes = new int[ZonedMapDict.Count];
            for (int i = 0; i < ZonedMapDict.Count; i++)
            {
                nodeIndexes[i] = ZonedMapDict.ElementAt(i).Value.NodeIndex;
                nodeTypes[i] = ZonedMapDict.ElementAt(i).Value.NodeType;
            }
            var result = await KingdomMapUpdateAsync(new KingdomMapUpdatePayload { NodeIndexes = nodeIndexes, NodeTypes = nodeTypes });

            if (result)
            {
                foreach (BaseNode node in ZonedMapDict.Values)
                {
                    KingdomState.NumNodeTypes[node.NodeType]++;
                    KingdomState.NumNodeTypes[Map[node.NodeIndex].NodeType]--;

                    Map[node.NodeIndex] = node;
                    NodeList[Map[node.NodeIndex].NodeType][node.NodeIndex].SetActive(false);
                    NodeList[node.NodeType][node.NodeIndex].SetActive(true);
                }
            }
            else //fail
            {
                Debug.Log($"CommitZonedMap Failure");
            }
        }
        #endregion


        #region Kingdom Map Initialisation
        public void KingdomMapGenerate()
        {
            InitialiseKingdomMapAssets();
            SeedNodePooling();
            ActivateMap();

        }
        public void ActivateMap()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 1980; j++)
                {
                    if (Map[j].NodeType != i)
                        DeActivate(j, i);
                }
            }
        }


        #region Node Pooling Properties
        private GameObject grassland, townCentre, house, library, factory, road, blockade, mTower, wonder;

        public GameObject Grassland { get { return grassland; } set { grassland = value; } }
        public GameObject TownCentre { get { return townCentre; } set { townCentre = value; } }
        public GameObject House { get { return house; } set { house = value; } }
        public GameObject Library { get { return library; } set { library = value; } }
        public GameObject Factory { get { return factory; } set { factory = value; } }
        public GameObject Road { get { return road; } set { road = value; } }
        public GameObject Blockade { get { return blockade; } set { blockade = value; } }
        public GameObject MTower { get { return mTower; } set { mTower = value; } }
        public GameObject Wonder { get { return wonder; } set { wonder = value; } }

        private Dictionary<int, GameObject> grasslandDict, townCentreDict, houseDict, libraryDict, factoryDict, wonderDict, mTowerDict, roadDict, blockadeDict;
        private List<Dictionary<int, GameObject>> nodeList;

        public Dictionary<int, GameObject> GrasslandDict { get { return grasslandDict; } set { grasslandDict = value; } }
        public Dictionary<int, GameObject> TownCentreDict { get { return townCentreDict; } set { townCentreDict = value; } }
        public Dictionary<int, GameObject> HouseDict { get { return houseDict; } set { houseDict = value; } }
        public Dictionary<int, GameObject> LibraryDict { get { return libraryDict; } set { libraryDict = value; } }
        public Dictionary<int, GameObject> FactoryDict { get { return factoryDict; } set { factoryDict = value; } }
        public Dictionary<int, GameObject> WonderDict { get { return wonderDict; } set { wonderDict = value; } }
        public Dictionary<int, GameObject> MTowerDict { get { return mTowerDict; } set { mTowerDict = value; } }
        public Dictionary<int, GameObject> RoadDict { get { return roadDict; } set { roadDict = value; } }
        public Dictionary<int, GameObject> BlockadeDict { get { return blockadeDict; } set { blockadeDict = value; } }
        public List<Dictionary<int, GameObject>> NodeList { get { return nodeList; } set { nodeList = value; } }
        #endregion


        public void InitialiseKingdomMapAssets()
        {
            Grassland = (GameObject)Resources.Load(@"Node/Buildings/Grassland", typeof(GameObject));
            TownCentre = (GameObject)Resources.Load(@"Node/Buildings/City Centre", typeof(GameObject));
            House = (GameObject)Resources.Load(@"Node/Buildings/House", typeof(GameObject));
            Library = (GameObject)Resources.Load(@"Node/Buildings/Library", typeof(GameObject));
            Factory = (GameObject)Resources.Load(@"Node/Buildings/Factory", typeof(GameObject));
            MTower = (GameObject)Resources.Load(@"Node/Buildings/TowerM", typeof(GameObject));
            Road = (GameObject)Resources.Load(@"Node/Buildings/Road", typeof(GameObject));
            Blockade = (GameObject)Resources.Load(@"Node/Buildings/Blockade", typeof(GameObject));
            Wonder = (GameObject)Resources.Load(@"Node/Buildings/Wonder", typeof(GameObject));

            IsZoningMode = false;
            Flare = (GameObject)Resources.Load(@"Node/Flares/Flare", typeof(GameObject));
            FlareMatRed = (Material)Resources.Load(@"Node/Flares/Error", typeof(Material));
            FlareMatGreen = (Material)Resources.Load(@"Node/Flares/Success", typeof(Material));
        }

        public void SeedNodePooling()
        {
            GrasslandDict = new Dictionary<int, GameObject>();
            TownCentreDict = new Dictionary<int, GameObject>();
            HouseDict = new Dictionary<int, GameObject>();
            LibraryDict = new Dictionary<int, GameObject>();
            FactoryDict = new Dictionary<int, GameObject>();
            WonderDict = new Dictionary<int, GameObject>();
            MTowerDict = new Dictionary<int, GameObject>();
            RoadDict = new Dictionary<int, GameObject>();
            BlockadeDict = new Dictionary<int, GameObject>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 1980; j++)
                {
                    switch (i)
                    {
                        case 0:
                            GrasslandDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 1:
                            TownCentreDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 2:
                            HouseDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 3:
                            LibraryDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 4:
                            FactoryDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 5:
                            RoadDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 6:
                            BlockadeDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 7:
                            MTowerDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 8:
                            WonderDict.Add(j, Instantiate(Grassland, new Vector3(KingdomState.CalculateNodePos(j)[0], KingdomState.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                    }
                    
                    if (i != 0)
                        continue;
                    else if (i == 0 && j == 0)
                    {
                        Color redOpaque = new Color(FlareMatRed.color.r, FlareMatRed.color.g, FlareMatRed.color.b, 0);
                        Flare.GetComponent<MeshRenderer>().sharedMaterial.color = redOpaque;
                    }
                    else if (i == 0)
                    {
                        int[] flarePos = KingdomState.CalculateNodePos(i);// 0 == x 1 == y
                        Flare.transform.position = new Vector3((float)flarePos[0], (float)flarePos[1], Flare.transform.position.z);
                        FlareDict.Add(i, Flare);
                    }
                }
            }
            NodeList = new List<Dictionary<int, GameObject>>
            {
                GrasslandDict, TownCentreDict, HouseDict, LibraryDict, FactoryDict, RoadDict, BlockadeDict, MTowerDict, WonderDict
            };
            NodeColours = new Color[]
            {
                GrasslandDict[0].GetComponent<MeshRenderer>().material.color,
                TownCentreDict[0].GetComponent<MeshRenderer>().material.color,
                HouseDict[0].GetComponent<MeshRenderer>().material.color,
                LibraryDict[0].GetComponent<MeshRenderer>().material.color,
                FactoryDict[0].GetComponent<MeshRenderer>().material.color,
                MTowerDict[0].GetComponent<MeshRenderer>().material.color,
                RoadDict[0].GetComponent<MeshRenderer>().material.color,
                BlockadeDict[0].GetComponent<MeshRenderer>().material.color,
                WonderDict[0].GetComponent<MeshRenderer>().material.color,
            };

        }
        #endregion


        #region Kingdom Map Tools
        public void SetZonedOpacitySelection(int[] nodeIdArray, float opacity)
        {
            Color colour = new Color(0f, 0f, 0f, 0f);
            foreach (int n_Id in nodeIdArray)
            {
                BaseNode node = Map[n_Id];
                switch (node.NodeType)
                {
                    case 0://grassland
                        colour = NodeList[0][n_Id].gameObject.GetComponent<Color>();
                        NodeList[0][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 1://towncentre
                        colour = NodeList[1][n_Id].gameObject.GetComponent<Color>();
                        NodeList[1][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 2://house
                        colour = NodeList[2][n_Id].gameObject.GetComponent<Color>();
                        NodeList[2][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 3://library
                        colour = NodeList[3][n_Id].gameObject.GetComponent<Color>();
                        NodeList[3][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 4://factory
                        colour = NodeList[4][n_Id].gameObject.GetComponent<Color>();
                        NodeList[4][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 5://road
                        colour = NodeList[5][n_Id].gameObject.GetComponent<Color>();
                        NodeList[5][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 6://blockade
                        colour = NodeList[6][n_Id].gameObject.GetComponent<Color>();
                        NodeList[6][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 7://mtower
                        colour = NodeList[7][n_Id].gameObject.GetComponent<Color>();
                        NodeList[7][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                    case 8://wonder
                        colour = NodeList[8][n_Id].gameObject.GetComponent<Color>();
                        NodeList[8][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(colour.r, colour.g, colour.b, opacity);
                        break;
                }
            }
        }

        public void SetZonedColourSelection(int[] nodeIdArray, int[] nodeTypeArray)
        {
            Color nodeZonedColour = new Color(0f, 0f, 0f, 0f);
            Color nodeColour = new Color(0f, 0f, 0f, 0f);
            int count = 0;
            foreach (int n_Id in nodeIdArray)
            {
                BaseNode node = Map[n_Id];
                switch (nodeTypeArray[count])
                {
                    case 0://grassland
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 1://towncentre
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 2://house
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 3://library
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 4://factory
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 5://road
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 6://blockade
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 7://mtower
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                    case 8://wonder
                        nodeColour = NodeList[node.NodeType][n_Id].gameObject.GetComponent<Color>();
                        nodeZonedColour = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<Color>();
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedColour.r, nodeZonedColour.g, nodeZonedColour.b, nodeColour.a);
                        break;
                }
                count++;
            }
        }

        public void DeActivate(int n_Id, int n_Type)
        {
            switch (n_Type)
            {
                case 0:
                    GrasslandDict[n_Id].SetActive(false);
                    break;
                case 1:
                    TownCentreDict[n_Id].SetActive(false);
                    break;
                case 2:
                    HouseDict[n_Id].SetActive(false);
                    break;
                case 3:
                    LibraryDict[n_Id].SetActive(false);
                    break;
                case 4:
                    FactoryDict[n_Id].SetActive(false);
                    break;
                case 5:
                    WonderDict[n_Id].SetActive(false);
                    break;
                case 6:
                    MTowerDict[n_Id].SetActive(false);
                    break;
                case 7:
                    RoadDict[n_Id].SetActive(false);
                    break;
                case 8:
                    BlockadeDict[n_Id].SetActive(false);
                    break;
            }
        }

        public void Activate(int n_Id, int n_Type)
        {
            switch (n_Type)
            {
                case 0:
                    GrasslandDict[n_Id].SetActive(true);
                    break;
                case 1:
                    TownCentreDict[n_Id].SetActive(true);
                    break;
                case 2:
                    HouseDict[n_Id].SetActive(true);
                    break;
                case 3:
                    LibraryDict[n_Id].SetActive(true);
                    break;
                case 4:
                    FactoryDict[n_Id].SetActive(true);
                    break;
                case 5:
                    MTowerDict[n_Id].SetActive(true);
                    break;
                case 6:
                    RoadDict[n_Id].SetActive(true);
                    break;
                case 7:
                    BlockadeDict[n_Id].SetActive(true);
                    break;
                case 8:
                    WonderDict[n_Id].SetActive(true);
                    break;
            }
        }


        public void Action()
        {

            //for (int i = 0; i < NodeIdAltArray.Length; i++)
            //{
            //    int n_Id = NodeIdAltArray[i];
            //    switch (BuildState)
            //    {
            //        case BuildState.Build:

            //            if (!IsInvalid(n_Id))
            //                ActionBuild(n_Id);
            //            break;
            //        case BuildState.Bulldoze:
            //            ActionBulldoze(n_Id);
            //            break;
            //    }
            //}
        }

        public void ActionBuild()//int n_Id)
        {

            //ActionBulldoze(n_Id);
            //UpdateNodeDict(n_Id, GetSelectedBuildingState());
            //Activate(n_Id, GetSelectedBuildingState());
        }

        public void ActionBulldoze()//int n_Id)
        {

            //DeActivate(n_Id, Map[n_Id].NodeType);
            //Activate(n_Id, 0);
            //Map[n_Id] = new Grassland() { NodeIndex = n_Id };
        }
        #endregion


        #region Build Options
        [SerializeField] private int nodeIdAlt1;
        [SerializeField] private int nodeIdAlt2;
        [SerializeField] private int[] nodeIdAltArray;
        public int N_Id_1_Alt { get { return nodeIdAlt1; } set { nodeIdAlt1 = value; } }
        public int N_Id_2_Alt { get { return nodeIdAlt2; } set { nodeIdAlt2 = value; } }
        public int[] NodeIdAltArray { get { return nodeIdAltArray; } set { nodeIdAltArray = value; } }

        public bool IsInvalid(int n_Id)
        {
            if (Map[n_Id].NodeType == GetSelectedBuildingState()) return true;
            return false;
        }

        #endregion


        #region State Navigator
        //[SerializeField] private BuildState buildState;
        [SerializeField] private AltState altState;
        [SerializeField] private SelectedBuilding selectedBuildingState;
        public SelectedBuilding SelectedBuildingState { get { return selectedBuildingState; } set { selectedBuildingState = value; } }
        //public BuildState BuildState { get { return buildState; } set { buildState = value; } }
        public AltState AltState { get { return altState; } set { altState = value; } }

        //public void NavigateBuildState(string UI_ObjName) //not required?
        //{
        //    switch (UI_ObjName)
        //    {
        //        case "Build":
        //            if (BuildState != BuildState.Build) BuildState = BuildState.Build;
        //            break;

        //        case "Bulldoze":
        //            if (BuildState != BuildState.Bulldoze) BuildState = BuildState.Bulldoze;
        //            break;
        //    }
        //}

        public void NavigateAltState(int state)
        {
            //if (AltState == AltState.Confirmed) AltState = 0;
            //else AltState++;
            switch (state)
            {
                case 0: //no alt key pressed
                    AltState = AltState.False;
                    break;
                case 1: // recording first node
                    AltState = AltState.FirstSelected;
                    break;
                case 2: // recorded both nodes, set to false on confirmation
                    AltState = AltState.SecondSelected;
                    break;
                case 3: //after confirmation of second node, set to false
                    AltState = AltState.Confirmed;
                    break;
            }
        }
        public void NavigateSelectedBuildingState(int state)
        {
            switch (state)
            {
                case 0:
                    SelectedBuildingState = SelectedBuilding.Grassland;
                    break;
                case 1:
                    SelectedBuildingState = SelectedBuilding.TownCentre;
                    break;
                case 2:
                    SelectedBuildingState = SelectedBuilding.House;
                    break;
                case 3:
                    SelectedBuildingState = SelectedBuilding.Library;
                    break;
                case 4:
                    SelectedBuildingState = SelectedBuilding.Factory;
                    break;
                case 5:
                    SelectedBuildingState = SelectedBuilding.Road;
                    break;
                case 6:
                    SelectedBuildingState = SelectedBuilding.Blockade;
                    break;
                case 7:
                    SelectedBuildingState = SelectedBuilding.MTower;
                    break;
                case 8:
                    SelectedBuildingState = SelectedBuilding.Wonder;
                    break;
            }
        }

        public int GetAltState()
        {
            int altState = (int)AltState;
            return altState;
        }

        public int GetSelectedBuildingState()
        {
            int selectedBuildingState = (int)SelectedBuildingState;
            return selectedBuildingState;
        }
        //public int GetBuildState()
        //{
        //    int buildState = (int)BuildState;
        //    return buildState;
        //}

        #endregion
    }
        #pragma warning disable format
        #region State Enums
        public enum BuildState
        {
            Build = 0,            //0
            Bulldoze = 1,         //1
        }

        public enum AltState
        {
            False = 0,            //0
            FirstSelected = 1,    //1
            SecondSelected = 2,   //2
            Confirmed = 3         //3
        }

        public enum SelectedBuilding
        {
            Grassland = 0,        //0
            TownCentre = 1,       //1
            House = 2,            //2
            Library = 3,          //3
            Factory = 4,          //4
            Road = 5,             //5
            Blockade = 6,         //6
            MTower = 7,           //7
            Wonder = 8,           //8
        }
        #endregion
        #pragma warning restore format
}
