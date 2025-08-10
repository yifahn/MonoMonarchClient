using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.ClientManagers.Treasury;
using MonoMonarchGameFramework.Game;
using MonoMonarchGameFramework.Game.Kingdom;
using MonoMonarchGameFramework.Game.Kingdom.Nodes;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Blockade;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Factory;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Grassland;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.House;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Library;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.MTower;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Road;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.TownCentre;
using MonoMonarchGameFramework.Game.Kingdom.Nodes.Wonder;
using MonoMonarchNetworkFramework;
using MonoMonarchNetworkFramework.Game.Kingdom;
using MonoMonarchNetworkFramework.Game.Kingdom.Map;
using MonoMonarchNetworkFramework.Game.Soupkitchen;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;



namespace Assets.Scripts.ClientManagers.Kingdom
{
    public class KingdomManager : MonoBehaviour
    {

        #region Kingdom Singleton
        public static int destructionCounter = 0; //for testing purposes only, remove later
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
                        Debug.Log("KINGDOMMANAGER SINGLETON HAS AWOKEN #1 - 1");
                    }
                    DontDestroyOnLoad(_instance.gameObject);
                }
                //Debug.Log("KINGDOMMANAGER SINGLETON HAS AWOKEN #1 - 2");
                return _instance;
            }
        }

        public static void ResetInstance()
        {
            if (_instance != null)
            {
                Debug.Log("KINGDOMMANAGER SINGLETON HAS BEEN RESET");
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                destructionCounter++;//for testing purposes only, remove later
                Debug.Log($"{destructionCounter}, ... DESTROYING KINGDOM INSTANCE");//for testing purposes only, remove later
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("KINGDOMMANAGER SINGLETON HAS AWOKEN #2");
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
                //Instance.KingdomMapGenerate(); //initialise kingdom map assets and seed node pooling
            }
        }
        #endregion


        #region Kingdom Properties
        public KingdomMapUpdateResponse KingdomMapUpdateResponse { get; set; }
        public KingdomLoadResponse KingdomLoadResponse { get; set; }
        public ErrorResponse KingdomErrorResponse { get; set; }

        public BaseNode[] Map { get => map; set => map = value; }
        //public Map Map { get => map; set => map = value; }
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
        private BaseNode[] map;
        //[SerializeField] private Map map;
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
        private KingdomState kingdomState;
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
                    Debug.Log("Load Kingdom Success");
                }
                else if (response is ErrorResponse errorResponse)
                {
                    KingdomErrorResponse = errorResponse;
                    await LoadingScreenExtensions.LoadLoadingSceneAsync();
                    await LoadingScreenExtensions.UnloadGameSceneAsync();
                    await LoadingScreenExtensions.LoadMainMenuSceneAsync();
                    await LoadingScreenExtensions.UnloadLoadingSceneAsync();
                    GameManager.Instance.ClearGameCache();
                    Debug.Log("Load Kingdom Failure");
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
                Debug.Log("Load Kingdom Exception");
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


        #region Map Zoning
        [SerializeField] private Dictionary<int, BaseNode> zonedMapDict; //index , BaseNode
        [SerializeField] private int[] zonedNumNodeTypes = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        [SerializeField] private Material[] nodeMaterials;
        [SerializeField] private bool isZoningMode;

        public Dictionary<int, BaseNode> ZonedMapDict { get => zonedMapDict; set => zonedMapDict = value; }
        public int[] ZonedNumNodeTypes { get => zonedNumNodeTypes; set => zonedNumNodeTypes = value; }
        public Material[] NodeMaterials { get => nodeMaterials; set => nodeMaterials = value; }
        public bool IsZoningMode { get => isZoningMode; set => isZoningMode = value; }

        private Dictionary<int, GameObject> flareDict;
        public Dictionary<int, GameObject> FlareDict { get { return flareDict; } set { flareDict = value; } }

        private GameObject flare;
        private Material flareMatRed, flareMatGreen, flareMatOpaque;
        public GameObject Flare { get { return flare; } set { flare = value; } }
        public Material FlareMatRed { get { return flareMatRed; } set { flareMatRed = value; } }
        public Material FlareMatGreen { get { return flareMatGreen; } set { flareMatGreen = value; } }
        public Material FlareMatOpaque { get { return flareMatOpaque; } set { flareMatOpaque = value; } }

        public void ToggleZoning()
        {
            if (!IsZoningMode)
            {
                IsZoningMode = true;


                //alter node colours to visually describe proposed zoning changes
                Debug.Log("Accessing DistinguishZoningNodes() 2");
                DistinguishZoningNodes(ZonedMapDict.Keys.ToArray(),0.75f);

                //set flare colour to opaque green if sufficient coin, else opaque red 
                Color flareMat = TreasuryManager.IsSufficientCoin(ZonedNumNodeTypes, TreasuryManager.Instance.TreasuryState.GetTotalCoin()) ? FlareMatGreen.color : FlareMatRed.color;//FlareMatRed.GetComponent<Color>();
                foreach (int nodeId in ZonedMapDict.Keys)
                {
                    if (FlareDict[nodeId].activeSelf == false)
                        FlareDict[nodeId].SetActive(true); //activate flare if not already active
                 // FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = flareMat;
                    FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = new Color(flareMat.r, flareMat.g, flareMat.b, flareMat.a);

                }
            }
            else if (IsZoningMode)
            {
                IsZoningMode = false;
                foreach (BaseNode node in ZonedMapDict.Values)
                {
                    DistinguishZoningNodes(ZonedMapDict.Keys.ToArray(), 1f);
                   // //reset node colour to Map's origin - inverse of DistinguishZoningNodes
                   // NodeList[Map[node.NodeIndex].NodeType][node.NodeIndex].GetComponent<MeshRenderer>().material.color = nodeMaterials.ElementAt(node.NodeType).color;

                    //deactivate all flares
                    if (FlareDict[node.NodeIndex].activeSelf == true)
                        FlareDict[node.NodeIndex].SetActive(false);
                }
            }

        }

        //Color redOpaque = new Color(FlareMatRed.color.r, FlareMatRed.color.g, FlareMatRed.color.b, 0);
        //FlareDict[node.NodeIndex].GetComponent<MeshRenderer>().material.color = redOpaque;

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

        public void DistinguishZoningNodes(int[] nodeIdArray, float opacity)
        {
            Debug.Log($"{IsZoningMode} isZoningMode");
            int[] nodeTypeArray = new int[nodeIdArray.Length];
            for (int i = 0; i < nodeIdArray.Length; i++)
            {
                //nodeTypeArray[i] = ZonedMapDict.ElementAt(nodeIdArray[i]).Value.NodeType;
                if (ZonedMapDict.TryGetValue(nodeIdArray[i], out var node))
                {
                    nodeTypeArray[i] = node.NodeType;
                }
            }
            SetZonedOpacitySelection(nodeIdArray, opacity);
            if (!IsZoningMode)
            {
                SetZonedColourSelectionOff(nodeIdArray, nodeTypeArray);
            }
            else
            {
                SetZonedColourSelectionOn(nodeIdArray, nodeTypeArray);
                
            }
            Debug.Log($"{IsZoningMode} isZoningMode");
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
        public bool IsFlaresActive()
        {
            foreach (GameObject flare in FlareDict.Values)
            {
                if (flare.activeSelf == true)
                    return true;
            }
            return false;
        }
        //public bool IsFlaresActive2()
        //{
        //    if (FlareDict.FirstOrDefault(x => x.Value.activeSelf == true).Value is not null)
        //        return true;
        //    else 
        //        return false;

        //}
        //move to GameManager
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
                    NodeList[Map[node.NodeIndex].NodeType][node.NodeIndex].gameObject.GetComponent<MeshRenderer>().material.color = new Color(NodeMaterials[node.NodeType].color.r, NodeMaterials[node.NodeType].color.g,NodeMaterials[node.NodeType].color.b, NodeMaterials[node.NodeType].color.a);
                    //FlareDict[nodeId].GetComponent<MeshRenderer>().material.color = new Color(flareMat.r, flareMat.g, flareMat.b, flareMat.a);
                    FlareDict[node.NodeIndex].GetComponent<MeshRenderer>().material.color = redOpaque;
                }

                IsZoningMode = false;
            }

            TreasuryManager.Instance.ZoningCost = 0;
            ZonedNumNodeTypes = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ZonedMapDict = new Dictionary<int, BaseNode>();
        }

        //move to GameManager
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
            Debug.Log("Kingdom Map Activation Complete");
        }


        #region Node Pooling Properties
        private GameObject grassland, townCentre, house, library, factory, road, blockade, tower, wonder;

        public GameObject Grassland { get { return grassland; } set { grassland = value; } }
        public GameObject TownCentre { get { return townCentre; } set { townCentre = value; } }
        public GameObject House { get { return house; } set { house = value; } }
        public GameObject Library { get { return library; } set { library = value; } }
        public GameObject Factory { get { return factory; } set { factory = value; } }
        public GameObject Road { get { return road; } set { road = value; } }
        public GameObject Blockade { get { return blockade; } set { blockade = value; } }
        public GameObject Tower { get { return tower; } set { tower = value; } }
        public GameObject Wonder { get { return wonder; } set { wonder = value; } }

        private Dictionary<int, GameObject> grasslandDict, townCentreDict, houseDict, libraryDict, factoryDict, wonderDict, mTowerDict, roadDict, blockadeDict;
        private List<Dictionary<int, GameObject>> nodeList;

        public Dictionary<int, GameObject> GrasslandDict { get { return grasslandDict; } set { grasslandDict = value; } }
        public Dictionary<int, GameObject> TownCentreDict { get { return townCentreDict; } set { townCentreDict = value; } }
        public Dictionary<int, GameObject> HouseDict { get { return houseDict; } set { houseDict = value; } }
        public Dictionary<int, GameObject> LibraryDict { get { return libraryDict; } set { libraryDict = value; } }
        public Dictionary<int, GameObject> FactoryDict { get { return factoryDict; } set { factoryDict = value; } }
        public Dictionary<int, GameObject> WonderDict { get { return wonderDict; } set { wonderDict = value; } }
        public Dictionary<int, GameObject> TowerDict { get { return mTowerDict; } set { mTowerDict = value; } }
        public Dictionary<int, GameObject> RoadDict { get { return roadDict; } set { roadDict = value; } }
        public Dictionary<int, GameObject> BlockadeDict { get { return blockadeDict; } set { blockadeDict = value; } }
        public List<Dictionary<int, GameObject>> NodeList { get { return nodeList; } set { nodeList = value; } }
        #endregion


        public void InitialiseKingdomMapAssets()
        {
            Grassland = (GameObject)Resources.Load(@"Node/Buildings/Grassland", typeof(GameObject));
            TownCentre = (GameObject)Resources.Load(@"Node/Buildings/TownCentre", typeof(GameObject));
            House = (GameObject)Resources.Load(@"Node/Buildings/House", typeof(GameObject));
            Library = (GameObject)Resources.Load(@"Node/Buildings/Library", typeof(GameObject));
            Factory = (GameObject)Resources.Load(@"Node/Buildings/Factory", typeof(GameObject));
            Tower = (GameObject)Resources.Load(@"Node/Buildings/Tower", typeof(GameObject));
            Road = (GameObject)Resources.Load(@"Node/Buildings/Road", typeof(GameObject));
            Blockade = (GameObject)Resources.Load(@"Node/Buildings/Blockade", typeof(GameObject));
            Wonder = (GameObject)Resources.Load(@"Node/Buildings/Wonder", typeof(GameObject));

            IsZoningMode = false;

            Flare = (GameObject)Resources.Load(@"Node/Flares/Flare", typeof(GameObject));
            FlareMatRed = (Material)Resources.Load(@"Node/Flares/Error", typeof(Material));
            FlareMatGreen = (Material)Resources.Load(@"Node/Flares/Success", typeof(Material));
            FlareMatOpaque = (Material)Resources.Load(@"Node/Flares/Opaque", typeof(Material));

            Debug.Log($"Initialising Kingdom Map Assets Completed");
        }

        public void SeedNodePooling()
        {
            GrasslandDict = new Dictionary<int, GameObject>();
            TownCentreDict = new Dictionary<int, GameObject>();
            HouseDict = new Dictionary<int, GameObject>();
            LibraryDict = new Dictionary<int, GameObject>();
            FactoryDict = new Dictionary<int, GameObject>();
            WonderDict = new Dictionary<int, GameObject>();
            TowerDict = new Dictionary<int, GameObject>();
            RoadDict = new Dictionary<int, GameObject>();
            BlockadeDict = new Dictionary<int, GameObject>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 1980; j++)
                {
                    //original scripts mention starts at pos 1, and offsets by 3

                    //camera birds eye view == z axis is on y axis, and y axis is on z axis
                    float x = KingdomState.CalculateNodePos(j)[0];
                    float y = KingdomState.CalculateNodePos(j)[1];
                    float z = 1f;
                    //each of the x,y,z coordinates need to be offset to allow a gap inbetween each instantiated node, otherwise they will overlap
                    float offset = 3f;
                    float xOffset = x * offset;
                    float yOffset = y * offset;


                    Vector3 v3 = new Vector3(xOffset, z, -yOffset); //negative yOffset
                    Quaternion q = Quaternion.identity;
                    switch (i)
                    {
                        case 0:
                            GrasslandDict.Add(j, Instantiate(Grassland, v3, q));
                            break;
                        case 1:
                            TownCentreDict.Add(j, Instantiate(TownCentre, v3, q));
                            break;
                        case 2:
                            HouseDict.Add(j, Instantiate(House, v3, q));
                            break;
                        case 3:
                            LibraryDict.Add(j, Instantiate(Library, v3, q));
                            break;
                        case 4:
                            FactoryDict.Add(j, Instantiate(Factory, v3, q));
                            break;
                        case 5:
                            RoadDict.Add(j, Instantiate(Road, v3, q));
                            break;
                        case 6:
                            BlockadeDict.Add(j, Instantiate(Blockade, v3, q));
                            break;
                        case 7:
                            TowerDict.Add(j, Instantiate(Tower, v3, q));
                            break;
                        case 8:
                            WonderDict.Add(j, Instantiate(Wonder, v3, q));
                            break;
                    }
                    if (i == 0 && j == 0)
                    {
                        FlareDict = new Dictionary<int, GameObject>();
                        Debug.Log("Initialising FlareDict");
                    }

                    if (i == 0)
                    {//instantiate flares for each node index
                        FlareDict.Add(j, Instantiate(Flare, new Vector3(xOffset, z + z, -yOffset), Flare.transform.rotation));
                        //FlareDict[j].GetComponent<MeshRenderer>().material = FlareMatOpaque;
                        FlareDict[j].SetActive(false); //better to deactivate than make material transparent as a transparent material may inturpt raycasts? - might be worth it to not be triggered by raycasts and be invisible, compute-wise. something to test
                    }




                }
            }
            NodeList = new List<Dictionary<int, GameObject>>
            {
                GrasslandDict, TownCentreDict, HouseDict, LibraryDict, FactoryDict, RoadDict, BlockadeDict, TowerDict, WonderDict
            };
            foreach (Dictionary<int, GameObject> dict in NodeList)
            {
                Debug.Log($"{dict[0].name}");
            }
            NodeMaterials = new Material[]
            {
                GrasslandDict[0].GetComponent<MeshRenderer>().material,
                TownCentreDict[0].GetComponent<MeshRenderer>().material,
                HouseDict[0].GetComponent<MeshRenderer>().material,
                LibraryDict[0].GetComponent<MeshRenderer>().material,
                FactoryDict[0].GetComponent<MeshRenderer>().material,
                TowerDict[0].GetComponent<MeshRenderer>().material,
                RoadDict[0].GetComponent<MeshRenderer>().material,
                BlockadeDict[0].GetComponent<MeshRenderer>().material,
                WonderDict[0].GetComponent<MeshRenderer>().material,
            };
            Debug.Log($"Seed Node Pooling Completed");
        }
        #endregion


        #region Kingdom Map Tools
        public void SetZonedOpacitySelection(int[] nodeIdArray, float opacity)
        {
            //Color colour = new Color(0f, 0f, 0f, 0f);
            Material material = null;
            foreach (int n_Id in nodeIdArray)
            {
                BaseNode node = Map[n_Id];
                switch (node.NodeType)
                {
                    case 0://grassland
                        material = NodeList[0][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[0][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 1://towncentre
                        material = NodeList[1][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[1][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 2://house
                        material = NodeList[2][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[2][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 3://library
                        material = NodeList[3][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[3][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 4://factory                                                               
                        material = NodeList[4][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[4][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 5://road                                                                  
                        material = NodeList[5][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[5][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 6://blockade                                                             
                        material = NodeList[6][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[6][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 7://mtower                                                               
                        material = NodeList[7][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[7][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                    case 8://wonder                                                                
                        material = NodeList[8][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[8][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, opacity);
                        break;
                }
            }
        }
        public void SetZonedColourSelectionOff(int[] nodeIdArray, int[] nodeTypeArray)
        {
            Material nodeZonedMaterial = null;
            Material nodeMaterial = null;
            int count = 0;
            foreach (int n_Id in nodeIdArray)
            {
                BaseNode node = Map[n_Id];
                switch (nodeTypeArray[count])
                {
                    case 0://grassland
                        nodeMaterial = NodeMaterials[node.NodeType];
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 1://towncentre
                        nodeMaterial = NodeMaterials[node.NodeType];
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 2://house
                        nodeMaterial = NodeMaterials[node.NodeType];
                       NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 3://library
                        nodeMaterial = NodeMaterials[node.NodeType];
                      NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 4://factory
                        nodeMaterial = NodeMaterials[node.NodeType];
                       NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 5://road
                        nodeMaterial = NodeMaterials[node.NodeType];
                       NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 6://blockade
                        nodeMaterial = NodeMaterials[node.NodeType];
                       NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 7://mtower
                        nodeMaterial = NodeMaterials[node.NodeType];
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 8://wonder
                        nodeMaterial = NodeMaterials[node.NodeType];
                       NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeMaterial.color.r, nodeMaterial.color.g, nodeMaterial.color.b, nodeMaterial.color.a);
                        break;
                }
                count++;
            }
        }
        public void SetZonedColourSelectionOn(int[] nodeIdArray, int[] nodeTypeArray)
        {
            Material nodeZonedMaterial = null;
            Material nodeMaterial = null;
            int count = 0;
            foreach (int n_Id in nodeIdArray)
            {
                BaseNode node = Map[n_Id];
                switch (nodeTypeArray[count])
                {
                    case 0://grassland
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 1://towncentre
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 2://house
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 3://library
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 4://factory
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 5://road
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 6://blockade
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 7://mtower
                        nodeMaterial = NodeMaterials[node.NodeType]; 
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
                        break;
                    case 8://wonder
                        nodeMaterial = NodeMaterials[node.NodeType];
                        nodeZonedMaterial = NodeList[nodeTypeArray[count]][n_Id].gameObject.GetComponent<MeshRenderer>().material;
                        NodeList[node.NodeType][n_Id].gameObject.GetComponent<MeshRenderer>().material.color = new Color(nodeZonedMaterial.color.r, nodeZonedMaterial.color.g, nodeZonedMaterial.color.b, nodeMaterial.color.a);
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
                    TowerDict[n_Id].SetActive(false);
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
                    TowerDict[n_Id].SetActive(true);
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

        #region UI Management
        #region Initialise Building Selector Buttons
        private GameObject btn_Grassland, btn_TownCentre, btn_House, btn_Library, btn_Factory, btn_Road, btn_Blockade, btn_Tower, btn_Wonder;
        public GameObject Btn_Grassland { get => btn_Grassland; set => btn_Grassland = value; }
        public GameObject Btn_TownCentre { get => btn_TownCentre; set => btn_TownCentre = value; }
        public GameObject Btn_House { get => btn_House; set => btn_House = value; }
        public GameObject Btn_Library { get => btn_Library; set => btn_Library = value; }
        public GameObject Btn_Factory { get => btn_Factory; set => btn_Factory = value; }
        public GameObject Btn_Road { get => btn_Road; set => btn_Road = value; }
        public GameObject Btn_Blockade { get => btn_Blockade; set => btn_Blockade = value; }
        public GameObject Btn_Tower { get => btn_Tower; set => btn_Tower = value; }
        public GameObject Btn_Wonder { get => btn_Wonder; set => btn_Wonder = value; }
        public void SetRightPanelUI()
        {
            GameManager.Instance.InitialiseUnityEvents();
            NavigateSelectedBuildingState(0); //initialise to grassland

            Btn_Grassland = GameObject.Find("btn_Grassland");
            Btn_Grassland.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(0));
            Btn_TownCentre = GameObject.Find("btn_TownCentre");
            Btn_TownCentre.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(1));
            Btn_House = GameObject.Find("btn_House");
            Btn_House.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(2));
            Btn_Library = GameObject.Find("btn_Library");
            Btn_Library.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(3));
            Btn_Factory = GameObject.Find("btn_Factory");
            Btn_Factory.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(4));
            Btn_Road = GameObject.Find("btn_Road");
            Btn_Road.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(5));
            Btn_Blockade = GameObject.Find("btn_Blockade");
            Btn_Blockade.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(6));
            Btn_Tower = GameObject.Find("btn_Tower");
            Btn_Tower.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(7));
            Btn_Wonder = GameObject.Find("btn_Wonder");
            Btn_Wonder.GetComponent<Button>().onClick.AddListener(() => NavigateSelectedBuildingState(8));
        }

        #endregion


        public GameObject ExpandNodePanelUI { get => expandNodePanelUI; set => expandNodePanelUI = value; }
        public GameObject NodePanel { get => nodeStatisticsUI; set => nodeStatisticsUI = value; }

        private GameObject nodeStatisticsUI, expandNodePanelUI;
        public GameObject tmp_MapName { get => mapName; set => mapName = value; }
        public GameObject btn_ZonedMode { get => zoned; set => zoned = value; }
        public GameObject btn_ConfirmMap { get => publish; set => publish = value; }
        public GameObject tmp_Node { get => node; set => node = value; }
        public GameObject tmp_NodeZoned { get => nodeZoned; set => nodeZoned = value; }

        private GameObject publish, zoned, mapName, nodeZoned, node;

        public void ToggleNodeStatisticsUI()
        {
            if (NodePanel.activeSelf)
                NodePanel.SetActive(false);
            else
                NodePanel.SetActive(true);
        }
        public void SetNodeStatistics()
        {
            ExpandNodePanelUI = GameObject.Find("btn_ExpandNodeStatistics");//btn_ExpandNodeStatistics
            NodePanel = GameObject.Find("NodePanel");

            ExpandNodePanelUI.GetComponent<Button>().onClick.AddListener(() => ToggleNodeStatisticsUI());

            tmp_NodeZoned = GameObject.Find("nodeZonedIcon");
            tmp_Node = GameObject.Find("nodeIcon");

            Debug.Log("NodeStatsUI enabled");

        }
        public void UpdateNodeStatistics(int nodeId)
        {
            Material mat1 = NodeMaterials[0];
            Material mat2 = NodeMaterials[0];
            switch (Map[nodeId].NodeType)
            {
                case 1:
                    mat1 = NodeMaterials[1];
                    break;
                case 2:
                    mat1 = NodeMaterials[2];
                    break;
                case 3:
                    mat1 = NodeMaterials[3];
                    break;
                case 4:
                    mat1 = NodeMaterials[4];
                    break; 
                case 5:    
                    mat1 = NodeMaterials[5];
                    break; 
                case 6:    
                    mat1 = NodeMaterials[6];
                    break; 
                case 7:    
                    mat1 = NodeMaterials[7];
                    break;
                case 8:   
                    mat1 = NodeMaterials[8];
                    break;
                default:
                    break;
            }
            if (ZonedMapDict.ContainsKey(nodeId))
            {
                switch (ZonedMapDict[nodeId].NodeType)
                {
                    case 1:
                        mat2 = NodeMaterials[1];
                        break;
                    case 2:
                        mat2 = NodeMaterials[2];
                        break;
                    case 3:
                        mat2 = NodeMaterials[3];
                        break;
                    case 4:
                        mat2 = NodeMaterials[4]; 
                        break;
                    case 5:
                        mat2 = NodeMaterials[5];
                        break;
                    case 6:
                        mat2 = NodeMaterials[6];
                        break;
                    case 7:
                        mat2 = NodeMaterials[7];
                        break;
                    case 8:
                        mat2 = NodeMaterials[8];
                        break;
                    default:
                        break;
                }
                tmp_NodeZoned.GetComponent<RawImage>().color = new Color(mat2.color.r, mat2.color.g, mat2.color.b, mat2.color.a);
            }
            tmp_Node.GetComponent<RawImage>().color = new Color(mat1.color.r, mat1.color.g, mat1.color.b, mat1.color.a);
        }
        public void SetTopPanelUI()
        {
            SetNodeStatistics();
            btn_ZonedMode = GameObject.Find("btn_ZonedMode");//tmp_MapName
            btn_ConfirmMap = GameObject.Find("btn_ConfirmMap");
            tmp_MapName = GameObject.Find("tmp_MapName");
            btn_ZonedMode.GetComponent<Button>().onClick.AddListener(() => ToggleZoning()); //?
            btn_ConfirmMap.GetComponent<Button>().onClick.AddListener(() => ConfirmMapChanges());
            Debug.Log(tmp_MapName.name);
            Debug.Log(kingdomState.KingdomName);
            tmp_MapName.GetComponent<TMP_Text>().text = KingdomState.KingdomName;
        }
        //public IEnumerator<int> UpdateMap()
        //{
        //    int[] nodeTypes = new int[ZonedMapDict.Count];
        //    int i = 0;
        //    foreach (BaseNode node in ZonedMapDict.Values)
        //    {
        //        nodeTypes[i] = node.NodeType;
        //    }
        //    var payload = new KingdomMapUpdatePayload
        //    {
        //        NodeIndexes = ZonedMapDict.Keys.ToArray<int>(),
        //        NodeTypes = nodeTypes
        //    };
        //    Task.
        //    await KingdomMapUpdateAsync(payload);
        //}
        public void ConfirmMapChanges()
        {
            if (TreasuryManager.Instance.TreasuryState.GetTotalCoin() < TreasuryManager.Instance.ZoningCost)
            {
                if (!IsZoningMode)
                    ToggleZoning();
            }
            else
                UpdateMap();


        }
        IEnumerator UpdateMap()
        {
            int[] nodeTypes = new int[ZonedMapDict.Count];
            int i = 0;
            foreach (BaseNode node in ZonedMapDict.Values)
            {
                nodeTypes[i] = node.NodeType;
            }
            var payload = new KingdomMapUpdatePayload
            {
                NodeIndexes = ZonedMapDict.Keys.ToArray<int>(),
                NodeTypes = nodeTypes
            };
            Task task = KingdomMapUpdateAsync(payload);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsCompletedSuccessfully)
            {
                DiscardZonedMap();
                ResetNodeStatisticsUI();
            }
        }

        public void ResetNodeStatisticsUI()
        {
            tmp_NodeZoned.GetComponentInChildren<TextMeshPro>().text = "zonedNode";
            tmp_NodeZoned.GetComponentInChildren<Image>().color = new Color(0, 0, 0);
            tmp_Node.GetComponentInChildren<TextMeshPro>().text = "mapNode";
            tmp_Node.GetComponentInChildren<Image>().color = new Color(0, 0, 0);
            //NodeStatisticsPanel
            //all NSCells NSCell0 -> NSCell 7
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
                case 2: // recorded both nodeArray, set to false on confirmation
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
                    SelectedBuildingState = SelectedBuilding.Tower;
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
            Tower = 7,            //7
            Wonder = 8,           //8
        }
        #endregion
        #pragma warning restore format
}
