using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace Helpers 
{
    #pragma warning disable format
    #region State Enums
    public enum BuildState
    {
        Build,           //0
        Bulldoze         //1
    }
    public enum OverwriteState
    {
        True,            //0
        False            //1
    }
    public enum AltState
    {
        False,           //0
        FirstSelected,   //1
        SecondSelected,  //2
        Confirmed        //3
    }
    public enum SelectedBuilding
    {
        TownCentre,      //0
        House,           //1
        Library,         //2
        Factory,         //3
        MTower,          //4
        Road,            //5
        Blockade,        //6
        Wonder,          //7
        Grassland        //8
    }
    #endregion
    #pragma warning restore format

    public static class BuilderHelper
    {

        #region Constructor

        static BuilderHelper() //hard code intialise for dev testing - playerprefs.ini
        {
            BuildState = BuildState.Build;
            OverwriteState = OverwriteState.False;
            AltState = AltState.False;
            SelectedBuildingState = SelectedBuilding.TownCentre;
            //ensure overwrite checkbox defaults to false
        }

        #endregion

        #region Actions

        private static int n_Id_1_Alt;
        private static int n_Id_2_Alt;
        private static int[] nodeArray;
        public static int N_Id_1_Alt { get { return n_Id_1_Alt; } set { n_Id_1_Alt = value; } }
        public static int N_Id_2_Alt { get { return n_Id_2_Alt; } set { n_Id_2_Alt = value; } }
        public static int[] AltPopulatedNodeArray { get { return nodeArray; } set { nodeArray = value; } }

        
        /// <summary>
        /// Returns True if current action is an Overwrite action
        /// </summary>
        /// <returns></returns>
        public static bool IsOverwriteAction(int n_Id)
        {
            bool isOverwrite = false;
            if (OverwriteState == OverwriteState.True && (!nameof(SelectedBuildingState).Equals(MapHelper.NodeMap[n_Id]) && MapHelper.NodeMap[n_Id] != "Grassland"))
            {
                isOverwrite = true;
            }
            return isOverwrite;
        }
        /// <summary>
        /// Returns False if building on existing building while OverwriteState == False
        /// </summary>
        /// <param name="n_Id"></param>
        /// <returns>Boolean</returns>
        public static bool InvalidOverwrite(int n_Id)
        {
            bool invalid = false;

            if (!MapHelper.NodeMap[n_Id].Contains("Grassland") && OverwriteState == OverwriteState.False && BuildState == BuildState.Build)
            {
                invalid = true; 
            }
            return invalid;
        }
        /// <summary>
        /// Returns True if Node selected == SelectedBuildingState
        /// </summary>
        /// <param name="n_Id"></param>
        /// <returns>Boolean</returns>
        public static bool InvalidBuild(int n_Id)
        {
            bool invalid = false;
            if (MapHelper.NodeMap[n_Id].Contains(nameof(SelectedBuildingState)))
            {
                invalid = true;
            }
            return invalid;
        }

        #endregion

        #region State Navigator

        private static BuildState buildState;
        private static OverwriteState overwriteState;
        private static AltState altState;
        private static SelectedBuilding selectedBuildingState;
        public static SelectedBuilding SelectedBuildingState { get { return selectedBuildingState; } set { selectedBuildingState = value; } }
        public static BuildState BuildState { get { return buildState; } set { buildState = value; } }
        public static OverwriteState OverwriteState { get { return overwriteState; } set { overwriteState = value; } }
        public static AltState AltState { get { return altState; } set { altState = value; } }

        /// <summary>
        /// state controller for build/bulldoze 
        /// </summary>
        /// <param name="UI_ObjName"></param>
        public static void NavigateBuildState(string UI_ObjName)
        {
            switch (UI_ObjName)
            {
                case "Build":
                    if (BuildState != BuildState.Build) BuildState = BuildState.Build;
                    break;

                case "Bulldoze":
                    if (BuildState != BuildState.Bulldoze) BuildState = BuildState.Bulldoze;
                    break;
            }
        }

        /// <summary>
        /// State Controller for alt key press detection. 
        /// 0 == no alt key pressed, 1 == first node, 2== second node, 3 == confirm and reset to false
        /// </summary>
        /// <param name="state"></param>
        public static void NavigateAltState(int state)
        {
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

        /// <summary>
        /// State Controller for overwrite toggler
        /// </summary>
        /// <param name="UI_ObjState"></param>
        public static void NavigateOverwriteState() //ensure checkbox defaults to false
        {
            switch (OverwriteState)
            {
                case OverwriteState.False:
                    OverwriteState = OverwriteState.True;
                    break;
                case OverwriteState.True:
                    OverwriteState = OverwriteState.False;
                    break;
                    /*
                    * If parameters are ever eventually required for Overwrite; Overwrite.House would only overwrite houses. 
                    * Add these conditions to this method.
                    */
            }
        }
        #endregion
        #region State Helper Methods
        public static int GetAltState()
        {
            int altState = (int)AltState;
            return altState;
        }
        public static int GetSelectedBuildingState()
        {
            int selectedBuildingState = (int)SelectedBuildingState;
            return selectedBuildingState;
        }
        public static int GetBuildState()
        {
            int buildState = (int)BuildState;
            return buildState;
        }
        public static int GetOverWriteState()
        {
            int overwriteState = (int)OverwriteState;
            return overwriteState;
        }

        public static void SetSelectedBuildState(int buildState)
        {
            BuildState = (BuildState)buildState;
        }
        public static void SetSelectedAltState(int altState)
        {
            AltState = (AltState)altState;
        }
        public static void SetSelectedOverwriteState(int overwriteState)
        {
            OverwriteState = (OverwriteState)overwriteState;
        }
        public static void SetSelectedBuildingState(int buildingState)
        {
            SelectedBuildingState = (SelectedBuilding)buildingState;
        }
        #endregion

    }
}

