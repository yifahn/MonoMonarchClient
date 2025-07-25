//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIController : MonoBehaviour
//{
//    public GameObject gameKeeper, exitPanel, buildBtn, exitBtn, battleBoardBtn,bazaarBtn,soupKitchenBtn, statsBtn;
//    public GameObject houseBuild, libraryBuild, cityCentreBuild, factoryBuild,
//        wonderBuild, roadBuild, blockadeBuild, towerMBuild, towerAABuild, removeBtn, backBtn;
//    public GameObject leftPanel, plane;
//    public GameObject confirmSaveExit, confirmExit, confirmBack;
//    //public GameObject[] buildComponents,MainComponents;
//    public List<GameObject> buildList, menuList, exitList,sceneList;

//    private enum stateUI
//    {
//        UIMainMenu,
//        Build,
//        UIConfirmExit
//    }
//    private stateUI state;

//    void Start()
//    {
//        plane = GameObject.Find("Plane");
//        gameKeeper = GameObject.Find("GameKeeper");
//        leftPanel = GameObject.Find("LeftPanel");
//        exitPanel = GameObject.Find("MenuPanel");

//        sceneList = new List<GameObject>()
//        {
//            (statsBtn = GameObject.Find("StatsBtn")),
//            (bazaarBtn = GameObject.Find("BazaarBtn")),
//            (soupKitchenBtn = GameObject.Find("SoupKitchenBtn")),
//            (battleBoardBtn = GameObject.Find("BattleBoardBtn"))
//        };

//        buildList = new List<GameObject>()
//        {
//            (houseBuild = GameObject.Find("HouseBuild")),
//            (libraryBuild = GameObject.Find("LibraryBuild")),
//            (factoryBuild = GameObject.Find("FactoryBuild")),
//            (cityCentreBuild = GameObject.Find("CityCentreBuild")),
//            (wonderBuild = GameObject.Find("WonderBuild")),
//            (blockadeBuild = GameObject.Find("BlockadeBuild")),
//            (roadBuild = GameObject.Find("RoadBuild")),
//            (towerAABuild = GameObject.Find("AATowerBuild")),
//            (towerMBuild = GameObject.Find("MTowerBuild")),
//            (removeBtn = GameObject.Find("RemoveBtn")),
//            (backBtn = GameObject.Find("BackToMainMenuBtn"))
            
//        };

//        menuList = new List<GameObject>()
//        {
//            (buildBtn = GameObject.Find("BuildBtn")),
//            (exitBtn = GameObject.Find("ExitBtn"))
//        };

//        exitList = new List<GameObject>()
//        {
//            (confirmBack = GameObject.Find("MenuPanelClose")),
//            (confirmExit =GameObject.Find("MainMenuBtn")),
//            (confirmSaveExit =GameObject.Find("SaveBtn"))
//        };

//        MainMenuUI();

//    }

//    public void OnClickBuild()
//    {
//        state = stateUI.Build;
//        if (exitPanel.activeSelf == true)
//        {
//            foreach (GameObject UIComponent in exitList)
//            {
//                UIComponent.SetActive(false);
//            }
//            exitPanel.SetActive(false);
//        }
//        foreach (GameObject UIComponent in menuList)
//        {
//            UIComponent.SetActive(false);
//        }
//        foreach (GameObject UIComponent in buildList)
//        {
//            UIComponent.SetActive(true);
//        }
//        exitPanel.SetActive(false);

//    }

//    public void MainMenuUI()
//    {
//        state = stateUI.UIMainMenu;
//        foreach (GameObject UIComponent in buildList)
//        {
//            UIComponent.SetActive(false);
//        }
//        foreach (GameObject UIComponent in exitList)
//        {
//            UIComponent.SetActive(false);
//        }
//        exitPanel.SetActive(false);
//        foreach (GameObject UIComponent in menuList)
//        {
//            UIComponent.SetActive(true);
//        }


//    }

//    public void OnClickBack()
//    {
//        switch (state)
//        {
//            case stateUI.UIMainMenu:
//                break;
//            case stateUI.Build:
//                MainMenuUI();
//                plane.GetComponent<Map>().SelectNone();
//                break;
//            case stateUI.UIConfirmExit:
//                OnClickConfirmBack();
//                break;
//        }
//    }
//    public void OnClickExit()
//    {
//        exitPanel.SetActive(true);
//        foreach (GameObject UIComponent in exitList)
//        {
//            UIComponent.SetActive(true);
//        }
//        Debug.Log("onClickExit");

//    }
//    public void OnClickConfirmSaveExit()
//    {
//        gameKeeper.GetComponent<SceneLoader>().SaveGameMenu();
//    }
//    public void OnClickConfirmExit()
//    {
//        gameKeeper.GetComponent<SceneLoader>().MainMenuExit();
//    }
//    public void OnClickConfirmBack()
//    {
//        exitPanel.SetActive(false);
//    }
//    public void OnClickGoToStatsScene()
//    {
//        Debug.Log(SaveAndLoad.saveFileDir + " " + SaveAndLoad.saveFileSelected);
//        gameKeeper.GetComponent<SceneLoader>().GoToStatsScene();
//    }

//}//sssssss
