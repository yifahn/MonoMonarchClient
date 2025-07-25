using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader2 : MonoBehaviour
{
    public GameObject menuKeeper, plane;
    public void Start()
    {
        menuKeeper = this.gameObject;
        plane = GameObject.Find("Plane");
    }

    public void NewGame()
    {
        SaveAndLoad.IsInGame = false;
        SaveAndLoad.IsLoad = false;
        SaveAndLoad.IsNew = true;//s

        CellState.MapListL1Allocate();
        CellState.MapListL2Allocate();

        SaveAndLoad.IsReadyLoadFunction = true;
        SaveAndLoad.IsReadyPlay = false;
        //s
        SceneManager.LoadScene(1);
    }
    public void CreatePlayer()
    {
        menuKeeper.GetComponent<MenuTransition>().isNewGameSelected = true;
        menuKeeper.GetComponent<MenuTransition>().ShowNewPlayerElement();
    }


    public void LoadGameViaOK()
    {
        if (menuKeeper.GetComponent<MenuTransition>().IsLoadDropdownPopulated() == true)
        {
            Debug.Log("load dropdown list is empty, start a new game instead");
        }
        else
        {
            SaveAndLoad.IsLoad = true;

            CellState.MapListL1Allocate();
            CellState.MapListL2Allocate();

            menuKeeper.GetComponent<MenuTransition>().SetFileRef();
            SaveAndLoad.IsReadyPlay = false;
            SceneManager.LoadScene(1);
        }
    }
    public void LoadGame()
    {
        menuKeeper.GetComponent<MenuTransition>().isLoadBtnSelected = true;
        menuKeeper.GetComponent<MenuTransition>().ShowLoadElement();
    }
    public void MainMenuExit()
    {
        plane.GetComponent<Map>().mapListL1.Clear();
        //ScoreState.RenewGame();
        SceneManager.LoadScene(0);
    }
    public void SaveGameMenu()
    {
        plane.GetComponent<Map>().PublishCity();
       // ScoreState.RenewGame();
        SceneManager.LoadScene(0);
    }
    public void GoToStatsScene()
    {
        plane.GetComponent<Map>().PublishCity();
        SceneManager.LoadScene(2);
    }


}
