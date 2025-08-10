using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helpers
{
    public static class TreasuryHelper
    {


        #region Getters & Setters
        public static double VALUE_TOWNCENTRE = 15;      //0
        public static double VALUE_HOUSE = 125;          //1
        public static double VALUE_LIBRARY = 775;        //2
        public static double VALUE_FACTORY = 775;        //3
        public static double VALUE_MTOWER = 1450;        //4
        public static double VALUE_ROAD = 65;            //5
        public static double VALUE_BLOCKADE = 125;       //6
        public static double VALUE_WONDER = 125000;      //7

        public static double VALUE_GRASSLAND = 0;        //8 (default)
                                                         //
        private static double p_Coin;
        private static double p_GainRate_Coin;
        private static double p_Multiplier_Coin;

        private static int p_PoliticalPoints;
        //             
        private static double gainRate_TownCentre;
        private static double gainRate_House;
        private static double gainRate_Library;
        private static double gainRate_Factory;
        private static double gainRate_MTower;
        private static double gainRate_Road;
        private static double gainRate_Blockade;
        private static double gainRate_Wonder;

        private static double gainRate_Grassland;
        //             
        private static double multiplier_TownCentre;
        private static double multiplier_House;
        private static double multiplier_Library;
        private static double multiplier_Factory;
        private static double multiplier_MTower;
        private static double multiplier_Road;
        private static double multiplier_Blockade;
        private static double multiplier_Wonder;

        private static double multiplier_Grassland;
        //
        private static int num_Wonder;
        private static int num_Max_Wonder;
        //--------------------------------------------------------------------------------------------------------------
        public static double P_Coin { get { return p_Coin; } set { p_Coin = value; } }
        public static double P_GainRate_Coin { get { return p_GainRate_Coin; } set { p_GainRate_Coin = value; } }
        public static double P_Multiplier_Coin { get { return p_Multiplier_Coin; } set { p_Multiplier_Coin = value; } }

        public static int P_PoliticalPoints { get { return p_PoliticalPoints; } set { p_PoliticalPoints = value; } }
        //            
        public static double GainRate_TownCentre { get { return gainRate_TownCentre; } set { gainRate_TownCentre = value; } }
        public static double GainRate_House { get { return gainRate_House; } set { gainRate_House = value; } }
        public static double GainRate_Library { get { return gainRate_Library; } set { gainRate_Library = value; } }
        public static double GainRate_Factory { get { return gainRate_Factory; } set { gainRate_Factory = value; } }
        public static double GainRate_MTower { get { return gainRate_MTower; } set { gainRate_MTower = value; } }
        public static double GainRate_Road { get { return gainRate_Road; } set { gainRate_Road = value; } }
        public static double GainRate_Blockade { get { return gainRate_Blockade; } set { gainRate_Blockade = value; } }
        public static double GainRate_Wonder { get { return gainRate_Wonder; } set { gainRate_Wonder = value; } }

        public static double GainRate_Grassland { get { return gainRate_Grassland; } set { gainRate_Grassland = value; } }
        //            
        public static double Multiplier_TownCentre { get { return multiplier_TownCentre; } set { multiplier_TownCentre = value; } }
        public static double Multiplier_House { get { return multiplier_House; } set { multiplier_House = value; } }
        public static double Multiplier_Library { get { return multiplier_Library; } set { multiplier_Library = value; } }
        public static double Multiplier_Factory { get { return multiplier_Factory; } set { multiplier_Factory = value; } }
        public static double Multiplier_MTower { get { return multiplier_MTower; } set { multiplier_MTower = value; } }
        public static double Multiplier_Road { get { return multiplier_Road; } set { multiplier_Road = value; } }
        public static double Multiplier_Blockade { get { return multiplier_Blockade; } set { multiplier_Blockade = value; } }
        public static double Multiplier_Wonder { get { return multiplier_Wonder; } set { multiplier_Wonder = value; } }

        public static double Multiplier_Grassland { get { return multiplier_Grassland; } set { multiplier_Grassland = value; } }
        //
        public static int Num_Wonder { get { return num_Wonder; } set { num_Wonder = value; } }
        public static int Num_Max_Wonder { get { return num_Max_Wonder; } set { num_Max_Wonder = value; } }
    //

    //
    #pragma warning disable format
    public static void InitialiseTreasury()
    {
        P_Coin                  = 1000;
        P_GainRate_Coin         = 5;
        P_Multiplier_Coin       = 1;  

        P_PoliticalPoints       = 0;
        //
        GainRate_TownCentre     = 0;
        GainRate_House          = 5;
        GainRate_Library        = 0;
        GainRate_Factory        = 0;
        GainRate_MTower         = 0;
        GainRate_Road           = 0;
        GainRate_Blockade       = 0;
        GainRate_Wonder         = 0;

        GainRate_Grassland      = 0;
        //
        Multiplier_TownCentre   = 0;
        Multiplier_House        = 0;
        Multiplier_Library      = 0.05;
        Multiplier_Factory      = 0.05;
        Multiplier_MTower       = 0;
        Multiplier_Road         = 0;
        Multiplier_Blockade     = 0;
        Multiplier_Wonder       = 0.75;

        Multiplier_Grassland    = 0;
        //
        Num_Wonder              = 0;
        Num_Max_Wonder          = 1;
    }
    //--------------------------------------------------------------------------------------------------------------
   #pragma warning restore format
    #endregion

#pragma warning disable format
    #region Helper Methods
    public static double SellBuilding(string building)
	{
		int building_Id = 111;
             if (building.Contains("TownCentre")) { building_Id = 0; P_GainRate_Coin -= GainRate_TownCentre; P_Multiplier_Coin -= Multiplier_TownCentre; }
        else if (building.Contains("House"))      { building_Id = 1; P_GainRate_Coin -= GainRate_House; P_Multiplier_Coin -= Multiplier_House; }
        else if (building.Contains("Library"))    { building_Id = 2; P_GainRate_Coin -= GainRate_Library; P_Multiplier_Coin -= Multiplier_Library; }
        else if (building.Contains("Factory"))    { building_Id = 3; P_GainRate_Coin -= GainRate_Factory; P_Multiplier_Coin -= Multiplier_Factory; }
        else if (building.Contains("Tower"))     { building_Id = 4; P_GainRate_Coin -= GainRate_MTower; P_Multiplier_Coin -= Multiplier_MTower; }
        else if (building.Contains("Road"))       { building_Id = 5; P_GainRate_Coin -= GainRate_Road; P_Multiplier_Coin -= Multiplier_Road; }
        else if (building.Contains("Blockade"))   { building_Id = 6; P_GainRate_Coin -= GainRate_Blockade; P_Multiplier_Coin -= Multiplier_Blockade; }
        else if (building.Contains("Wonder"))     { building_Id = 7; P_GainRate_Coin -= GainRate_Wonder; P_Multiplier_Coin -= Multiplier_Wonder; }
        else if (building.Contains("Grassland"))  { building_Id = 8; P_GainRate_Coin -= GainRate_Grassland; P_Multiplier_Coin -= Multiplier_Grassland; }

            const double SELLRATE = 3;//add public accessor and move this if i setup difficulty levels for single player mode?
            switch (building_Id)
            {
                case 0: return VALUE_TOWNCENTRE / SELLRATE;
                case 1: return VALUE_HOUSE / SELLRATE;
                case 2: return VALUE_LIBRARY / SELLRATE;
                case 3: return VALUE_FACTORY / SELLRATE;
                case 4: return VALUE_MTOWER / SELLRATE;
                case 5: return VALUE_ROAD / SELLRATE;
                case 6: return VALUE_BLOCKADE / SELLRATE;
                case 7: return VALUE_WONDER / SELLRATE;
                default: return VALUE_GRASSLAND / SELLRATE;
            }
        }
            public static double BuyBuilding(string building)
            {
                int building_Id = 0;
                     if (building.Contains("TownCentre"))  { building_Id = 0;  P_GainRate_Coin += GainRate_TownCentre; P_Multiplier_Coin += Multiplier_TownCentre; }
                else if (building.Contains("House"))       { building_Id = 1;  P_GainRate_Coin += GainRate_House; P_Multiplier_Coin += Multiplier_House; }
                else if (building.Contains("Library"))     { building_Id = 2;  P_GainRate_Coin += GainRate_Library; P_Multiplier_Coin += Multiplier_Library; }
                else if (building.Contains("Factory"))     { building_Id = 3;  P_GainRate_Coin += GainRate_Factory; P_Multiplier_Coin += Multiplier_Factory; }
                else if (building.Contains("Tower"))      { building_Id = 4;  P_GainRate_Coin += GainRate_MTower; P_Multiplier_Coin += Multiplier_MTower; }
                else if (building.Contains("Road"))        { building_Id = 5;  P_GainRate_Coin += GainRate_Road; P_Multiplier_Coin += Multiplier_Road; }
                else if (building.Contains("Blockade"))    { building_Id = 6;  P_GainRate_Coin += GainRate_Blockade; P_Multiplier_Coin += Multiplier_Blockade; }
                else if (building.Contains("Wonder"))      { building_Id = 7;  P_GainRate_Coin += GainRate_Wonder; P_Multiplier_Coin += Multiplier_Wonder; }
                else if (building.Contains("Grassland"))   { building_Id = 8;  P_GainRate_Coin += GainRate_Grassland; P_Multiplier_Coin += Multiplier_Grassland; }

                switch (building_Id)
                {
                    case 0: return VALUE_TOWNCENTRE;
                    case 1: return VALUE_HOUSE;
                    case 2: return VALUE_LIBRARY;
                    case 3: return VALUE_FACTORY;
                    case 4: return VALUE_MTOWER;
                    case 5: return VALUE_ROAD;
                    case 6: return VALUE_BLOCKADE;
                    case 7: return VALUE_WONDER;
                    default: return VALUE_GRASSLAND;
                }
            }
    public static bool IsSufficientCoin(string building)
    {
        switch (building)
        {
            case "TownCentre":
                if (VALUE_TOWNCENTRE >= P_Coin) return true;
                return false;
            case "House":
                if (VALUE_HOUSE >= P_Coin) return true;
                return false;
            case "Library": 
                if (VALUE_LIBRARY >= P_Coin) return true;
                    return false;
            case "Factory":
                if (VALUE_FACTORY >= P_Coin) return true;
                return false;
            case "Wonder":
                if (VALUE_WONDER >= P_Coin) return true;
                return false;
            case "Tower":
                if (VALUE_MTOWER >= P_Coin) return true;
                return false;
            case "Road":
                if (VALUE_ROAD >= P_Coin) return true;
                return false;
            case "Blockade":
                if (VALUE_BLOCKADE >= P_Coin) return true;
                return false;
            case "Grassland":
                return true;
                default:
                Debug.Log("TreasuryHelper.IsSufficientCoin() returned default: false");
                return false;

        }
    }

    #endregion
#pragma warning restore format

    #region Executor Methods
    public static void NewGame()
        {

        }
        /// <summary>
        /// Input: operation == 0 or 1; subtract or add.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="coin"></param>
        /// <returns></returns>
        public static void UpdateCoin(int operation, double coin)
        {
            switch (operation)
            {
                case 0: P_Coin -= coin; break;
                case 1: P_Coin += coin; break;
            }
        }

        /// <summary>
        /// Input: operation == 0 or 1; subtract or add.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="coin"></param>
        /// <returns></returns>
        public static void UpdatePoliticalPoints(int operation, int politicalPoints)
        {
            switch (operation)
            {
                case 0: P_PoliticalPoints -= politicalPoints; break;
                case 1: P_PoliticalPoints += politicalPoints; break;
            }
        }
        #endregion


    }
}