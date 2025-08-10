using Managers;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

using static UnityEngine.UIElements.UxmlAttributeDescription;

using UnityEngine.UIElements.Experimental;

namespace Helpers
{
    public static class MapHelper
    {

        #region Constructor
        static MapHelper()
        {
            MapDimensions_X = 60; //set x and y via UIManager as a preset later, small/medium/large?
            MapDimensions_Y = 33;
        }
        #endregion

        #region Getters & Setters
        private static Dictionary<int, string> nodeMap;
        private static int mapDimensions_X;
        private static int mapDimensions_Y;
        private static Color color;
        private static int[] altNodeArray;

        public static Dictionary<int, string> NodeMap { get { return nodeMap; } set { nodeMap = value; } }
        public static int MapDimensions_X { get { return mapDimensions_X; } set { mapDimensions_X = value; } }
        public static int MapDimensions_Y { get { return mapDimensions_Y; } set { mapDimensions_Y = value; } }
        public static Color Color { get { return color; } set { color = value; } }
        public static int[] AltNodeArray { get { return altNodeArray; } set { altNodeArray = value; } }
        #endregion

        #region Map Manager Methods
        public static void FillNodeMapGrassland()//game manager - new game
        {
            for (int i = 0; i < 1980; i++)
            {
                NodeMap.Add(i, "Grassland");
            }
        }
        public static void InitialiseNodeMap()
        {
            NodeMap = new Dictionary<int, string>();
        }
        public static void SetMapDimensions(int x, int y)
        {
            MapDimensions_X = x;
            MapDimensions_Y = y;
        }
        /// <summary>
        /// 0:TownCentre_1:House_2:Library_3:Factory_4:MTower_5:Road_6:Blockade_7:Wonder_8:Grassland
        /// </summary>
        /// <param name="old_N_Id"></param>
        /// <param name="newSelectedBuilding"></param>
        public static void UpdateNodeDict(int old_N_Id, int newSelectedBuilding)
        {
            NodeMap[old_N_Id] = GetBuilding(newSelectedBuilding);
        }
        public static void LoadNodeMap()
        {
            //do load function
        }
        public static void SaveNodeMap()
        {
            //do save function
        }

        #endregion

        #region Map Helper Methods
        /// <summary>
        /// find id (id is 0 based) from x and y pos
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int CalculateNodeId(int x, int y)
        {
            int n_Id = (y - 1) * MapDimensions_X + (x - 1);
            return n_Id;
        }
        /// <summary>
        /// find x,y pos from id (id is 0 based)
        /// </summary>
        /// <returns></returns>
        public static int[] CalculateNodePos(int n_Id)
        {
            int[] nodePos = new int[2];
            nodePos[0] = (n_Id % 60) + 1;
            nodePos[1] = (n_Id / 60) + 1;
            return nodePos;
        }




        /// <summary>
        /// Provide n_Id of two nodeArray, returns array of n_Id within the bounds of n1 and n2 as rectangle
        /// </summary>
        public static int[] PopulateNodeArray(int n_1, int n_2)
        {
            int[] nodePos1 = CalculateNodePos(n_1);
            int[] nodePos2 = CalculateNodePos(n_2);
            int node_1X = nodePos1[0];
            int node_1Y = nodePos1[1];
            int node_2X = nodePos2[0];
            int node_2Y = nodePos2[1];
            List<int> nodeList = new List<int>();
            //-----------------------------------------------------------------------------------------
            if (node_1X < node_2X) //Orientation clarification: TopLeft == node1 is north west of node2.
            {
                if (node_1Y < node_2Y) nodeList = TopLeft(nodePos1, nodePos2);
                else if (node_1Y > node_2Y) nodeList = BottomLeft(nodePos1, nodePos2);
                else if (node_1Y == node_2Y) nodeList = Left(nodePos1, nodePos2);
            }
            else if (node_1X > node_2X)
            {
                if (node_1Y < node_2Y) nodeList = TopRight(nodePos1, nodePos2);
                else if (node_1Y > node_2Y) nodeList = BottomRight(nodePos1, nodePos2);
                else if (node_1Y == node_2Y) nodeList = Right(nodePos1, nodePos2);
            }
            else if (node_1X == node_2X)
            {
                if (node_1Y < node_2Y) nodeList = Top(nodePos1, nodePos2);
                else if (node_1Y > node_2Y) nodeList = Bottom(nodePos1, nodePos2);
                else if (node_1Y == node_2Y) nodeList = Middle(nodePos1, nodePos2);
            }

            AltNodeArray = new int[nodeList.Count];
            AltNodeArray = nodeList.ToArray();
            return AltNodeArray;
            //-----------------------------------------------------------------------------------------


            ////CHATGPT~
            /////chatgpt interpretation of my above code, it first attempted strings, i said enums.
            //// Determine orientation
            //Orientation orientation =
            //    node_1X < node_2X ? Orientation.Left :
            //    node_1X > node_2X ? Orientation.Right :
            //    node_1Y < node_2Y ? Orientation.Top :
            //    node_1Y > node_2Y ? Orientation.Bottom :
            //    Orientation.Middle;

            //// Choose the appropriate function based on orientation
            //switch (orientation)
            //{
            //    case Orientation.Left:
            //        nodeList = node_1Y < node_2Y ? TopLeft(nodePos1, nodePos2) :
            //                   node_1Y > node_2Y ? BottomLeft(nodePos1, nodePos2) :
            //                   Left(nodePos1, nodePos2);
            //        break;
            //    case Orientation.Right:
            //        nodeList = node_1Y < node_2Y ? TopRight(nodePos1, nodePos2) :
            //                   node_1Y > node_2Y ? BottomRight(nodePos1, nodePos2) :
            //                   Right(nodePos1, nodePos2);
            //        break;
            //    case Orientation.Top:
            //        nodeList = Top(nodePos1, nodePos2);
            //        break;
            //    case Orientation.Bottom:
            //        nodeList = Bottom(nodePos1, nodePos2);
            //        break;
            //    case Orientation.Middle:
            //        nodeList = Middle(nodePos1, nodePos2);
            //        break;
            //}


            //AltNodeArray = new int[nodeList.Count];
            //AltNodeArray = nodeList.ToArray();
            //return AltNodeArray;
            ////CHATGPT~\\
        }
        //public enum Orientation
        //{
        //    Left,
        //    Right,
        //    Top,
        //    Bottom,
        //    Middle
        //}


        #endregion

        #region Load Node Helper Methods
        /// <summary>
        /// Returns building as GameObject, requires input as integer = 0:TownCentre_1:House_2:Library_3:Factory_4:MTower_5:Road_6:Blockade_7:Wonder_8:Grassland
        /// </summary>
        /// <param name="buildingStateEnum"></param>
        /// <returns></returns>
        public static string GetBuilding(int buildingStateEnum)
        {
            switch (buildingStateEnum)
            {
                case 0:
                    return "TownCentre";
                case 1:
                    return "House";
                case 2:
                    return "Library";
                case 3:
                    return "Factory";
                case 4:
                    return "Tower";
                case 5:
                    return "Road";
                case 6:
                    return "Blockade";
                case 7:
                    return "Wonder";
                case 8:
                    return "Grassland";
                default: System.Diagnostics.Debug.WriteLine("Unknown int given, only 0-8 accepted"); return null;
            }
        }
        #endregion
        #region Load Node Prefabs
        private static string grassland;

        private static string house;
        private static string library;
        private static string factory;
        private static string wonder;
        private static string townCentre;
        private static string mTower;
        private static string road;
        private static string blockade;

        public static string Grassland { get { return grassland; } set { grassland = value; } }

        public static string House { get { return house; } set { house = value; } }
        public static string Library { get { return library; } set { library = value; } }
        public static string Factory { get { return factory; } set { factory = value; } }
        public static string Wonder { get { return wonder; } set { wonder = value; } }
        public static string TownCentre { get { return townCentre; } set { townCentre = value; } }
        public static string MTower { get { return mTower; } set { mTower = value; } }
        public static string Road { get { return road; } set { road = value; } }
        public static string Blockade { get { return blockade; } set { blockade = value; } }


        #endregion

        #region Alt Bounds Helper
        private static List<int> TopLeft(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y <= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x <= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> BottomLeft(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y >= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x <= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> Left(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y == nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x <= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> TopRight(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y <= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x >= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> BottomRight(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y >= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x >= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> Right(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y == nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x >= nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> Top(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y <= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x == nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> Bottom(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y >= nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x == nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        private static List<int> Middle(int[] nodePos1, int[] nodePos2)
        {
            List<int> nodeList = new List<int>();
            for (int y = nodePos1[1]; y == nodePos2[1]; y++)
            {
                for (int x = nodePos1[0]; x == nodePos2[0]; x++)
                {
                    int id = CalculateNodeId(x, y);
                    nodeList.Add(id);
                }
            }
            return nodeList;
        }
        /*                   //CHATGPT~
        To make the code more performant, you can consider the following optimizations:

1. **Avoid unnecessary array lookups**: Instead of accessing `nodePos1[0]` and `nodePos1[1]` multiple times in the loop, you can cache these values before the loop starts.

2. **Reduce function call overhead**: If the `CalculateNodeId` function is lightweight, consider inlining it directly into the loop to avoid the overhead of function calls.

3. **Use `List<T>.Capacity`**: Preallocate the capacity of the `nodeList` to avoid resizing during the loop.

4. **Simplify loop conditions**: Instead of having complex loop conditions, simplify them to improve readability and potentially performance.

Here's the optimized version of the code:

```csharp
private static List<int> GenerateNodeList(int[] nodePos1, int[] nodePos2, int xStep, int yStep)
        {
            List<int> nodeList = new List<int>((Math.Abs(nodePos2[0] - nodePos1[0]) + 1) * (Math.Abs(nodePos2[1] - nodePos1[1]) + 1));

            int startX = nodePos1[0];
            int endX = nodePos2[0];
            int startY = nodePos1[1];
            int endY = nodePos2[1];

            for (int y = startY; yStep > 0 ? y <= endY : y >= endY; y += yStep)
            {
                for (int x = startX; xStep > 0 ? x <= endX : x >= endX; x += xStep)
                {
                    int id = (y * mapWidth) + x; // Assuming mapWidth is the width of the map
                    nodeList.Add(id);
                }
            }

            return nodeList;
        }

        // Usage
        private static List<int> TopLeft(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 1, 1);
        }

        private static List<int> BottomLeft(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 1, -1);
        }

        private static List<int> Left(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 1, 0);
        }

        private static List<int> TopRight(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, -1, 1);
        }

        private static List<int> BottomRight(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, -1, -1);
        }

        private static List<int> Right(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, -1, 0);
        }

        private static List<int> Top(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 0, 1);
        }

        private static List<int> Bottom(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 0, -1);
        }

        private static List<int> Middle(int[] nodePos1, int[] nodePos2)
        {
            return GenerateNodeList(nodePos1, nodePos2, 0, 0);
        }
```

These optimizations should improve the performance of the code by reducing unnecessary operations and optimizing memory usage.*/
        //CHATGPT~\\

        #endregion

    }
}