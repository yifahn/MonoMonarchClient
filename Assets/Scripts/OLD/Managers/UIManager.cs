using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : ScriptableObject
    {
        public static UIManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        public int SceneSelector(int selected)
        {
            return 1;
        }

        /*private TMP_Text altBuild_1, altBuild_2;
        public void AltBuildController_UI()
        {

            if (BuilderHelper.AltState == AltState.FirstSelected)
            {
                altBuild_1.text = $"{MapHelper.CalculateNodePos(BuilderHelper.N_Id_1_Alt)[0]},{MapHelper.CalculateNodePos(BuilderHelper.N_Id_1_Alt)[1]}";
            }
            else if (BuilderHelper.AltState == AltState.SecondSelected)
            {
                altBuild_2.text = $"{MapHelper.CalculateNodePos(BuilderHelper.N_Id_2_Alt)[0]},{MapHelper.CalculateNodePos(BuilderHelper.N_Id_2_Alt)[1]}";
            }
            else
            {
                altBuild_1.text = "0,0";
                altBuild_2.text = "0,0";
            }
        }*/
        /*   public void RouteClickedElement_MapScene(string gameObjectName)
           {
               if (gameObjectName.Substring(0, 2).Equals("s_"))//scene
               {
                   if (gameObjectName.Contains("NavExitBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("MainMenu"); LoadScene(); }//prod will be this, testing requires navexit > saveexit || exit?
                   else if (gameObjectName.Contains("MapBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Map"); LoadScene(); }
                   else if (gameObjectName.Contains("CharacterBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Character"); LoadScene(); }
                   else if (gameObjectName.Contains("SoupKitchenBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("SoupKitchen"); LoadScene(); }
                   else if (gameObjectName.Contains("ArmouryBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Armoury"); LoadScene(); }
                   else if (gameObjectName.Contains("BazaarBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Bazaar"); LoadScene(); }
                   else if (gameObjectName.Contains("BattleBoardBtn")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Battleboard"); LoadScene(); }

                   else if (gameObjectName.Contains("BtnOK2")) { SceneHelper.SceneState = SceneHelper.SceneStateUpdate("Map"); LoadScene(); }
               }
               if (gameObjectName.Substring(0, 2).Equals("b_"))//building
               {
                   if (gameObjectName.Contains("TownCentreBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.TownCentre; }
                   else if (gameObjectName.Contains("HouseBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.House; }
                   else if (gameObjectName.Contains("LibraryBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Library; }
                   else if (gameObjectName.Contains("FactoryBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Factory; }
                   else if (gameObjectName.Contains("MTowerBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Tower; }
                   else if (gameObjectName.Contains("WonderBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Wonder; }
                   else if (gameObjectName.Contains("RoadBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Road; }
                   else if (gameObjectName.Contains("BlockadeBtn")) { BuilderHelper.SelectedBuildingState = SelectedBuilding.Blockade; }
               }
               if (gameObjectName.Substring(0, 2).Equals("bs"))//buildstate
               {
                   if (gameObjectName.Contains("BuildBtn")) { BuilderHelper.BuildState = BuildState.Build; }
                   else if (gameObjectName.Contains("BulldozeBtn")) { BuilderHelper.BuildState = BuildState.Bulldoze; }
               }
           }
         /*  public void RouteClickedElement_MainMenuScene(string gameObjectName)
           {

           }
           public void RouteClickedElement_MainMenuScene(string gameObjectName)
           {

           }
           public void RouteClickedElement_MainMenuScene(string gameObjectName)
           {

           }
           public void RouteClickedElement_MainMenuScene(string gameObjectName)
           {

           }
           public void RouteClickedElement_MainMenuScene(string gameObjectName)
           {

           }*/



    }


}
