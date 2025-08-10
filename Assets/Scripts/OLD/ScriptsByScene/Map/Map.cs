using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Assets.Scripts.ClientManagers.Kingdom;

namespace Managers//required due to other script called Map, remove this once all scripts updated
{
    public class Map : MonoBehaviour
    {
        #region Object Pooling
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



        public void InitialiseNodePooling()
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
            NodeList = new List<Dictionary<int, GameObject>>
            {
                GrasslandDict, TownCentreDict, HouseDict, LibraryDict, FactoryDict, WonderDict, MTowerDict, RoadDict, BlockadeDict
            };

        }
        #region Load Node Prefabs
        private static GameObject grassland;

        private static GameObject townCentre;
        private static GameObject house;
        private static GameObject library;
        private static GameObject factory;
        private static GameObject wonder;
        private static GameObject mTower;
        private static GameObject road;
        private static GameObject blockade;

        public static GameObject Grassland { get { return grassland; } set { grassland = value; } }

        public static GameObject TownCentre { get { return townCentre; } set { townCentre = value; } }
        public static GameObject House { get { return house; } set { house = value; } }
        public static GameObject Library { get { return library; } set { library = value; } }
        public static GameObject Factory { get { return factory; } set { factory = value; } }
        public static GameObject Wonder { get { return wonder; } set { wonder = value; } }
        public static GameObject MTower { get { return mTower; } set { mTower = value; } }
        public static GameObject Road { get { return road; } set { road = value; } }
        public static GameObject Blockade { get { return blockade; } set { blockade = value; } }

        public void InitialiseNodePrefabs()
        {
            House = (GameObject)Resources.Load(@"Node/Buildings/House", typeof(GameObject));
            Library = (GameObject)Resources.Load(@"Node/Buildings/Library", typeof(GameObject));
            Factory = (GameObject)Resources.Load(@"Node/Buildings/Factory", typeof(GameObject));
            Wonder = (GameObject)Resources.Load(@"Node/Buildings/Wonder", typeof(GameObject));
            TownCentre = (GameObject)Resources.Load(@"Node/Buildings/TownCentre", typeof(GameObject));
            Grassland = (GameObject)Resources.Load(@"Node/Buildings/Grassland", typeof(GameObject));
            MTower = (GameObject)Resources.Load(@"Node/Buildings/Tower", typeof(GameObject));
            Road = (GameObject)Resources.Load(@"Node/Buildings/Road", typeof(GameObject));
            Blockade = (GameObject)Resources.Load(@"Node/Buildings/Blockade", typeof(GameObject));
        }
        #endregion
        public void NodePool()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 1980; j++)
                {
                    switch (i)
                    {
                        case 0:
                            GrasslandDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 1:
                            TownCentreDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 2:
                            HouseDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 3:
                            LibraryDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 4:
                            FactoryDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 5:
                            WonderDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 6:
                            MTowerDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 7:
                            RoadDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                        case 8:
                            BlockadeDict.Add(j, Instantiate(Grassland, new Vector3((float)MapHelper.CalculateNodePos(j)[0], (float)MapHelper.CalculateNodePos(j)[1], 1f), Quaternion.identity));
                            break;
                    }
                }
            }
        }

        public void ActivateGrasslands()
        {
            foreach (Dictionary<int, GameObject> nodeDict in NodeList)
            {
                foreach (GameObject node in nodeDict.Values)
                {
                    node.SetActive(false);
                }
            }
            foreach (GameObject node in GrasslandDict.Values)
            {
                node.SetActive(true);
            }
        }
        /// <summary>
        /// Alters material properties of input nodeArray to enable a degree of visual transparency
        /// </summary>
        /// <param name="opacity"></param>
        public void SetOpacitySelection(int[] nodeArray, float opacity)
        {
            foreach (int n_Id in nodeArray)
            {
                foreach (Dictionary<int, GameObject> nodeDict in NodeList)
                {
                    if (nodeDict[n_Id].name.Contains(MapHelper.NodeMap[n_Id]))
                    {
                        Color color = nodeDict[n_Id].gameObject.GetComponent<Color>();
                        color = new Color(color.r, color.g, color.b, opacity);
                        break;
                    }
                }
               // Color colour = NodeMap[n_Id].GetComponent<Material>().color;
                //Color highlight = new Color(Color.r, Color.g, Color.b, opacity);
              //  KingdomManager.Instance.NodeList[n_Id].GetComponent<Material>().SetColor("AdjustColor", highlight);
            }
        }
        /// <summary>
        /// 0:grassland-1:towncentre-2:house-3:library-4:factory-5:wonder-6:road-7:blockade
        /// </summary>
        /// <param name="n_Id"></param>
        /// <param name="n_Type"></param>
        /// <returns>int[] with count of 2, [0] == </returns>
        public void DeActivate(int n_Id, string n_Type)
        {
            switch (n_Type)
            {
                case "Grassland":
                    GrasslandDict[n_Id].SetActive(false);
                    break;
                case "TownCentre":
                    TownCentreDict[n_Id].SetActive(false);
                    break;
                case "House":
                    HouseDict[n_Id].SetActive(false);
                    break;
                case "Library":
                    LibraryDict[n_Id].SetActive(false);
                    break;
                case "Factory":
                    FactoryDict[n_Id].SetActive(false);
                    break;
                case "Wonder":
                    WonderDict[n_Id].SetActive(false);
                    break;
                case "Tower":
                    MTowerDict[n_Id].SetActive(false);
                    break;
                case "Road":
                    RoadDict[n_Id].SetActive(false);
                    break;
                case "Blockade":
                    BlockadeDict[n_Id].SetActive(false);
                    break;
            }
        }
        public void Activate(int n_Id, string n_Type)
        {
            switch (n_Type)
            {
                case "Grassland":
                    GrasslandDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 8);
                    break;
                case "TownCentre":
                    TownCentreDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 0);
                    break;
                case "House":
                    HouseDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 1);
                    break;
                case "Library":
                    LibraryDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 2);
                    break;
                case "Factory":
                    FactoryDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 3);
                    break;
                case "Tower":
                    MTowerDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 7);
                    break;
                case "Road":
                    RoadDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 5);
                    break;
                case "Blockade":
                    BlockadeDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 6); ;
                    break;
                case "Wonder":
                    WonderDict[n_Id].SetActive(true);
                    MapHelper.UpdateNodeDict(n_Id, 7);
                    break;
            }


        }
        public void Action()
        {
            for (int i = 0; i < BuilderHelper.AltPopulatedNodeArray.Length; i++)
            {
                int n_Id = BuilderHelper.AltPopulatedNodeArray[i];
                switch (BuilderHelper.BuildState)
                {
                    case Helpers.BuildState.Build:
                        if (!BuilderHelper.InvalidOverwrite(n_Id))
                        {
                            if (!BuilderHelper.InvalidBuild(n_Id))
                            {
                                ActionBuild(n_Id);
                            }
                        }
                        break;
                    case Helpers.BuildState.Bulldoze:
                        ActionBulldoze(n_Id);
                        break;
                }
            }
        }
        public void ActionBuild(int n_Id)
        {
            if (TreasuryHelper.IsSufficientCoin(nameof(BuilderHelper.SelectedBuildingState)))
            {
                ActionBulldoze(n_Id);
                TreasuryHelper.UpdateCoin(0, TreasuryHelper.BuyBuilding(nameof(BuilderHelper.SelectedBuildingState)));
                MapHelper.UpdateNodeDict(n_Id, BuilderHelper.GetSelectedBuildingState());
                Activate(n_Id, nameof(BuilderHelper.SelectedBuildingState));
            }
        }

        public void ActionBulldoze(int n_Id)
        {
            TreasuryHelper.UpdateCoin(1, TreasuryHelper.SellBuilding(MapHelper.NodeMap[n_Id]));
            MapHelper.UpdateNodeDict(n_Id, 8);
            DeActivate(n_Id, MapHelper.NodeMap[n_Id]);
            Activate(n_Id, "Grassland");
        }
        #endregion
    }
}
