using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Replace : MonoBehaviour
{
    public float buildingHeight = 1.2f;
    public GameObject plane, panel;
    GameObject score;



    public void Awake()
    {
        panel = GameObject.Find("LeftPanel");
        plane = GameObject.Find("Plane");
        score = GameObject.Find("Score");
        /*house = panel.GetComponent<Building>().buildingKeeper.House;
        library = panel.GetComponent<Building>().buildingKeeper.Library;
        factory = panel.GetComponent<Building>().buildingKeeper.Factory;
        cityCentre = panel.GetComponent<Building>().buildingKeeper.CityCentre;
        wonder = panel.GetComponent<Building>().buildingKeeper.Wonder;
        forest = panel.GetComponent<Building>().buildingKeeper.Forest;
        grassland = panel.GetComponent<Building>().buildingKeeper.Grassland;
        road = panel.GetComponent<Building>().buildingKeeper.Road;
        blockade = panel.GetComponent<Building>().buildingKeeper.Blockade;
        towerAA = panel.GetComponent<Building>().buildingKeeper.TowerAA;
        towerM = panel.GetComponent<Building>().buildingKeeper.Tower;
        farm = panel.GetComponent<Building>().buildingKeeper.Farm;//sss*/
    }
    public void callRemove(string buildingName)//ss
    {
        /*
        if (buildingName.Contains("House"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland,panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.House.name);
            Debug.Log(panel.GetComponent<Building>().buildingKeeper.House.name);
            Debug.Log(panel.GetComponent<Building>().buildingKeeper.Library.name);
            Debug.Log(panel.GetComponent<Building>().buildingKeeper.Factory.name);//ss
            Debug.Log(panel.GetComponent<Building>().buildingKeeper.CityCentre.name);
            Debug.Log("REMOVING HOUSE");
        }
        else if (buildingName.Contains("City Centre"))
        {
            Debug.Log("REMOVING CITY CENTRE");
           // string tempDebugString = string.Format("x={0} | y={1}", panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
           // Debug.Log(tempDebugString);
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            Debug.Log("BORAT SAYS SUCCESS");//ssss
            //plane = GameObject.Find("Plane");
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);//, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z
            //score = GameObject.Find("Score");
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.CityCentre.name);
        }
        else if (buildingName.Contains("Library"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Library.name);
        }
        else if (buildingName.Contains("Factory"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Factory.name);
        }
        else if (buildingName.Contains("Wonder"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Wonder.name);
        }
        else if (buildingName.Contains("Road"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Road.name);
        }
        else if (buildingName.Contains("Blockade"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Blockade.name);
        }
        else if (buildingName.Contains("Tower"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.Tower.name);
        }
        else if (buildingName.Contains("TowerAA"))
        {
            Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
            score.GetComponent<Score>().RemoveUpdate(panel.GetComponent<Building>().buildingKeeper.TowerAA.name);
        }
        */
    }


    public void callReplace()
    {
        /*
       
        if (CellState.createForestBool)
        {
            replaceCell("forest");
        }
        else if (CellState.createMTowerBool)
        {
            replaceCell("mtower");

        }
        else if (CellState.createAATowerBool)
        {
            replaceCell("aatower");

        }
        else if (CellState.createBlockadeBool)
        {
            replaceCell("blockade");

        }
        else if (CellState.createRoadBool)
        {
            replaceCell("road");

        }
        else if (CellState.createFarmBool)
        {
            replaceCell("farm");

        }
        else if (CellState.createGrasslandBool)
        {
            Debug.Log("CREATINGGRASSLANDFROMDESTROY");
            replaceCell("grassland");

        }
        else if (CellState.createHouseBool)
        {
            replaceCell("house");

        }
        else if (CellState.createLibraryBool)
        {
            replaceCell("library");
        }
        else if (CellState.createFactoryBool)
        {
            replaceCell("factory");
        }
        else if (CellState.createCityCentreBool)
        {
            Debug.Log("WORKING");
            replaceCell("cityCentre");
        }
        else if (CellState.createWonderBool)
        {
            replaceCell("wonder");
        }
        */
    }
    //Debug.Log(cityCentre.name + " " + "built");
    public void replaceCell(string buildingName)
    {
        /*
        //ss
        if (panel.GetComponent<Building>().buildingSelected)
        {
            switch (buildingName)
            {
                case "mtower":
                    if (SaveAndLoad.isReadyPlay == false)//ss panel.GetComponent<Building>().buildingKeeper.
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Tower, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Tower, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z); //RECENT UPDATE TO ALL LINES LIKE THIS IN THIS SCRIPT
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        
                        //  score.GetComponent<Score>().BuildScoreUpdate(BuildingKeeper.buildingKeeper.Tower.name);
                    }
                    if (ScoreState.score >= ScoreState.towerMPrice && SaveAndLoad.isReadyPlay == true)
                    {
                        if (SaveAndLoad.isReadyPlay == true)
                        {
                            Instantiate(panel.GetComponent<Building>().buildingKeeper.Tower, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Tower, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                            Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                            score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Tower.name);
                            Debug.Log(panel.GetComponent<Building>().buildingKeeper.Tower.name + " " + "built");
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }
                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                  
                    break;
                case "grassland":///////////////////////THIS FOR BOX REMOVE????
                    Debug.Log("DEBUG11");
                    Instantiate(panel.GetComponent<Building>().buildingKeeper.Grassland, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, 0, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                    plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Grassland, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                   
                   // Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);ss
                    break;
                case "aatower":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.TowerAA, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                    plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.TowerAA, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        // score.GetComponent<Score>().BuildScoreUpdate(BuildingKeeper.TowerAA.name);
                    }
                    if (ScoreState.score >= ScoreState.towerAAPrice && SaveAndLoad.isReadyPlay == true)
                    {
                        if (SaveAndLoad.isReadyPlay == true)
                        {
                            Instantiate(panel.GetComponent<Building>().buildingKeeper.TowerAA, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.TowerAA, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                            Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                            score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.TowerAA.name);
                            Debug.Log(panel.GetComponent<Building>().buildingKeeper.TowerAA.name + " " + "built");
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }
                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "forest":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Forest, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Forest, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        // score.GetComponent<Score>().BuildScoreUpdate(BuildingKeeper.Forest.name);
                    }
                    if (SaveAndLoad.isReadyPlay == true)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Forest, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Forest, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Forest.name);
                        Debug.Log(panel.GetComponent<Building>().buildingKeeper.Forest.name + " " + "built");
                    }
                    break;
                case "blockade":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Blockade, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Blockade, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        // score.GetComponent<Score>().BuildScoreUpdate(BuildingKeeper.Blockade.name);
                    }
                    if (ScoreState.score >= ScoreState.blockadePrice && SaveAndLoad.isReadyPlay == true)
                    {
                        if (SaveAndLoad.isReadyPlay == true)
                        {
                            Instantiate(panel.GetComponent<Building>().buildingKeeper.Blockade, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Blockade, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                            Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                            score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Blockade.name);
                            Debug.Log(panel.GetComponent<Building>().buildingKeeper.Blockade.name + " " + "built");
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }
                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "road":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Road, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Road, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        //  score.GetComponent<Score>().BuildScoreUpdate(BuildingKeeper.Road.name);
                    }
                    if (ScoreState.score >= ScoreState.roadPrice && SaveAndLoad.isReadyPlay == true)
                    {
                        if (SaveAndLoad.isReadyPlay == true)
                        {
                            Instantiate(panel.GetComponent<Building>().buildingKeeper.Road, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                            plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Road, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                            Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                            score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Road.name);
                            Debug.Log(panel.GetComponent<Building>().buildingKeeper.Road.name + " " + "built");
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }
                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "house":
                    Debug.Log(buildingName + " 1");//ss
                    Debug.Log(ScoreState.numHouses);
                    Debug.Log(ScoreState.maxNumHouses); //moved these out to see what these variables are
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.House, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.House, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.House.name);
                    }
                    if (ScoreState.numHouses < ScoreState.maxNumHouses && SaveAndLoad.isReadyPlay == true) //doesnt execute this code when it should, fails and goes here ///### && SaveAndLoad.isReadyPlay == true
                    {
                        Debug.Log("arrived here 1 house");
                        if (ScoreState.score >= ScoreState.housePrice)
                        {
                            Debug.Log("arrived here 2 house");
                            if (SaveAndLoad.isReadyPlay == true)
                            {
                                Debug.Log("arrived here 3 house");

                                Debug.Log("YESSSSSSS");
                                Debug.Log(buildingName + " 2");
                                Instantiate(panel.GetComponent<Building>().buildingKeeper.House, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                                plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.House, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                                Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                                score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.House.name);
                                Debug.Log(panel.GetComponent<Building>().buildingKeeper.House.name + " " + "built");
                            }
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }
                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "library":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Library, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Library, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Library.name);
                    }
                    Debug.Log(ScoreState.numFactories);
                    Debug.Log(ScoreState.maxNumFactories);
                    Debug.Log(ScoreState.numHouses);
                    Debug.Log(ScoreState.maxNumHouses);
                    Debug.Log(ScoreState.numLibraries);
                    Debug.Log(ScoreState.maxNumLibraries);
                    if (ScoreState.numLibraries < ScoreState.maxNumLibraries && SaveAndLoad.isReadyPlay == true)
                    {
                        Debug.Log("arrived here 1 lib");
                        if (ScoreState.score >= ScoreState.libraryPrice)
                        {
                            Debug.Log("arrived here 2 lib");
                            if (SaveAndLoad.isReadyPlay == true)
                            {
                                Debug.Log("arrived here 3 lib");
                                Instantiate(panel.GetComponent<Building>().buildingKeeper.Library, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                                plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Library, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                                Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                                score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Library.name);
                                Debug.Log(panel.GetComponent<Building>().buildingKeeper.Library.name + " " + "built");
                            }
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }

                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "cityCentre":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Debug.Log("reached this point");
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.CityCentre, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.CityCentre, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.CityCentre.name);
                    }
                    if (SaveAndLoad.isReadyPlay == true)
                    {
                        Debug.Log("reached this point");
                        Debug.Log(panel.GetComponent<Building>().buildingKeeper.CityCentre.name);
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.GetBuilding("City Centre"), new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.CityCentre, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.CityCentre.name);
                    }
                    break;//ssss

                case "factory":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Factory, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Factory, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Factory.name);
                    }
                    Debug.Log(ScoreState.numFactories);
                    Debug.Log(ScoreState.maxNumFactories);
                    if (ScoreState.numFactories < ScoreState.maxNumFactories && SaveAndLoad.isReadyPlay == true)
                    {
                        Debug.Log("arrived here 1 fac");
                        if (ScoreState.score >= ScoreState.factoryPrice)
                        {
                            Debug.Log("arrived here 2 fac");
                            if (SaveAndLoad.isReadyPlay == true)
                            {
                                Debug.Log("arrived here 3 fac");
                                Instantiate(panel.GetComponent<Building>().buildingKeeper.Factory, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                                plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Factory, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                                Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                                score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Factory.name);
                                Debug.Log(panel.GetComponent<Building>().buildingKeeper.Factory.name + " " + "built");
                            }
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }

                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
                case "wonder":
                    if (SaveAndLoad.isReadyPlay == false)
                    {
                        Instantiate(panel.GetComponent<Building>().buildingKeeper.Wonder, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                        plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Wonder, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                        Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                        score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Wonder.name);
                    }
                    if (ScoreState.numWonders < ScoreState.maxNumWonders && SaveAndLoad.isReadyPlay == true)
                    {
                        Debug.Log("arrived here 1 won");
                        if (ScoreState.score >= ScoreState.wonderPrice)
                        {
                            Debug.Log("arrived here 2 won");
                            if (SaveAndLoad.isReadyPlay == true)
                            {
                                Debug.Log("arrived here 3 won");
                                Instantiate(panel.GetComponent<Building>().buildingKeeper.Wonder, new Vector3(panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, buildingHeight, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z), Quaternion.identity);
                                plane.GetComponent<Map>().EditCellList(panel.GetComponent<Building>().buildingKeeper.Wonder, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.x, panel.GetComponent<Building>().buildingKeeper.SelectedCell.transform.position.z);
                                Destroy(panel.GetComponent<Building>().buildingKeeper.SelectedCell);
                                Debug.Log(panel.GetComponent<Building>().buildingKeeper.Wonder.name + " " + "built");
                                score.GetComponent<Score>().BuildScoreUpdate(panel.GetComponent<Building>().buildingKeeper.Wonder.name);
                            }
                        }
                        else
                        {
                            Debug.Log("you do not have enough score to pay for a this");
                        }

                    }
                    else
                    {
                        Debug.Log("you have reached the limit of this building");
                    }
                    break;
            }
        */
        }
    }


