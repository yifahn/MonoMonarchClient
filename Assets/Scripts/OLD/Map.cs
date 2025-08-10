using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//REVISED CODEBASE 18/04/22

public class Map : MonoBehaviour
{
    public static int offSet = 3;
    public bool houseSelected, librarySelected, factorySelected, wonderSelected, cityCentreSelected, roadSelected, blockadeSelected, farmSelected, grasslandSelected, forestSelected, towerAASelected, towerMSelected;
    public GameObject house, library, factory, citycentre, wonder, road, blockade, forest, grassland, towerAA, towerM, farm;

    //public GameObject g_Grassland;
    public GameObject panelBuild, scoreObj;
    public List<GameObject> mapListL1;
    public List<string> mapListL2;
    public GameObject gameKeeper;
    public GameObject playerSpawnCell, opponentSpawnCell; //declares
    public string debugString;

    //remove tracker variables
    public string selectedBuildingRemove;
    public float selectedBuildingRemoveX, selectedBuildingRemoveZ;

    public GameObject overwriteToggle;
    public string nodeToRemoveTemp;

    public bool checkShiftBuildPriceNumBuilding;//ss
    Vector3 position;


    public GameObject selectBoxBoundsText1, selectBoxBoundsText2;
    public bool boundsSelected1, boundsSelected2;
    public GameObject boundsObj1, boundsObj2;

    public GameObject selectedObj;

    public bool checkPriceNumBuilding;

    public void Start()
    {
        checkShiftBuildPriceNumBuilding = false;
        checkPriceNumBuilding = false;
        overwriteToggle = GameObject.Find("ToggleOverWrite");
        selectedObj = null;


        boundsSelected1 = false;
        boundsSelected2 = false;
        boundsObj1 = null;
        boundsObj2 = null;
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;

        Debug.Log("Initiating start variables for map.cs");
        selectBoxBoundsText1 = GameObject.Find("SelectedAltBuild1");
        selectBoxBoundsText2 = GameObject.Find("SelectedAltBuild2");
        selectBoxBoundsText1.SetActive(false);
        selectBoxBoundsText2.SetActive(false);

        gameKeeper = GameObject.Find("GameKeeper");//s
        scoreObj = GameObject.Find("Score");
        panelBuild = gameObject;

        house = (GameObject)Resources.Load(@"Buildings/House", typeof(GameObject));
        library = (GameObject)Resources.Load(@"Buildings/Library", typeof(GameObject));
        factory = (GameObject)Resources.Load(@"Buildings/Factory", typeof(GameObject));
        wonder = (GameObject)Resources.Load(@"Buildings/Wonder", typeof(GameObject));
        citycentre = (GameObject)Resources.Load(@"Buildings/City Centre", typeof(GameObject));
        farm = (GameObject)Resources.Load(@"Buildings/Farm", typeof(GameObject));
        grassland = (GameObject)Resources.Load(@"Buildings/Grassland", typeof(GameObject));
        towerAA = (GameObject)Resources.Load(@"Buildings/TowerAA", typeof(GameObject));
        towerM = (GameObject)Resources.Load(@"Buildings/Tower", typeof(GameObject));
        road = (GameObject)Resources.Load(@"Buildings/Road", typeof(GameObject));
        blockade = (GameObject)Resources.Load(@"Buildings/Blockade", typeof(GameObject));
        forest = (GameObject)Resources.Load(@"Buildings/Forest", typeof(GameObject));
    }
    public void Update()
    {
        if (Input.GetButtonUp("left alt"))//falsify and disable all items related to multi-build/multi-remove IF left alt release
        {
            Debug.Log("Released left alt, bounds variables set to false and null");
            if (boundsSelected1 && boundsSelected2)
            {
                float boundsObjX1 = boundsObj1.transform.position.x;
                float boundsObjY1 = boundsObj1.transform.position.z;
                float boundsObjX2 = boundsObj2.transform.position.x;
                float boundsObjY2 = boundsObj2.transform.position.z;
                int count = 0;
                if (boundsObjX1 < boundsObjX2)
                {
                    if (boundsObjY1 < boundsObjY2)
                    {
                        for (float y = boundsObjY1; y <= boundsObjY2; y += 3f)//bottom to top Obj1 to Obj2 Y
                        {
                            for (float x = boundsObjX1; x <= boundsObjX2; x += 3f)//left to right Obj1 to Obj2 X 
                            {

                                int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                                Component halo = mapListL1[i].GetComponent("Halo");
                                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                                count++;
                            }

                        }
                    }
                    else if (boundsObjY1 > boundsObjY2)
                    {
                        for (float y = boundsObjY1; y >= boundsObjY2; y -= 3f)//bottom to top Obj1 to Obj2 Y
                        {
                            for (float x = boundsObjX1; x <= boundsObjX2; x += 3f)//left to right Obj1 to Obj2 X
                            {
                                int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                                Component halo = mapListL1[i].GetComponent("Halo");
                                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                                count++;

                            }

                        }
                    }
                    else if (boundsObjY1 == boundsObjY2)
                    {
                        float y = boundsObjY1;
                        for (float x = boundsObjX1; x <= boundsObjX2; x += 3f)//left to right Obj1 to Obj2 X
                        {

                            int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[i].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                            count++;
                        }

                    }
                }
                else if (boundsObjX1 > boundsObjX2)
                {
                    if (boundsObjY1 < boundsObjY2)
                    {
                        for (float y = boundsObjY1; y <= boundsObjY2; y += 3f)//bottom to top Obj1 to Obj2 Y
                        {
                            for (float x = boundsObjX1; x >= boundsObjX2; x -= 3f)//left to right Obj1 to Obj2 X
                            {

                                int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                                Component halo = mapListL1[i].GetComponent("Halo");
                                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                                count++;
                            }

                        }
                    }
                    else if (boundsObjY1 > boundsObjY2)
                    {
                        for (float y = boundsObjY1; y >= boundsObjY2; y -= 3f)//bottom to top Obj1 to Obj2 Y
                        {
                            for (float x = boundsObjX1; x >= boundsObjX2; x -= 3f)//left to right Obj1 to Obj2 X
                            {

                                int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                                Component halo = mapListL1[i].GetComponent("Halo");
                                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                                count++;
                            }

                        }
                    }
                    else if (boundsObjY1 == boundsObjY2)
                    {
                        float y = boundsObjY1;
                        for (float x = boundsObjX1; x >= boundsObjX2; x -= 3f)//left to right Obj1 to Obj2 X
                        {

                            int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[i].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                            count++;
                        }
                    }
                }
                else if (boundsObjX1 == boundsObjX2)
                {
                    if (boundsObjY1 < boundsObjY2)
                    {
                        for (float y = boundsObjY1; y <= boundsObjY2; y += 3f)//bottom to top Obj1 to Obj2 Y
                        {

                            float x = boundsObjX1;
                            int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[i].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                            count++;
                        }
                    }
                    else if (boundsObjY1 > boundsObjY2)
                    {
                        float x = boundsObjX1;
                        for (float y = boundsObjY1; y >= boundsObjY2; y -= 3f)//bottom to top Obj1 to Obj2 Y
                        {

                            int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[i].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                            count++;
                        }
                    }
                    else if (boundsObjY1 == boundsObjY2)
                    {

                        float y = boundsObjY1;
                        float x = boundsObjX1;
                        int i = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        Component halo = mapListL1[i].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        count++;

                    }
                }
                Debug.Log("disabled " + count + " halos");
            }
            boundsObj1 = null;
            boundsObj2 = null;
            boundsSelected1 = false;
            boundsSelected2 = false;
            selectBoxBoundsText1.SetActive(false);
            selectBoxBoundsText2.SetActive(false);

        }
        if (Input.GetButtonUp("left shift"))
        {
            Debug.Log("left shift released");
            selectedObj = null;
        }
    }

    public void SingleBuildRemove(GameObject selectedNode) // NEW METHOD - passed testing - Noting that with 9 houses + 1 city centre, it is possible to overwrite city centre with a 10th house - passes numbuildinglimit check - this is acceptable
    {
        Debug.Log("SINGLE BUILD/REMOVE");
        SelectedBuildingRemoveTracker(selectedNode.name, selectedNode.transform.position.x, selectedNode.transform.position.z); // assists with tracking previously removed item, could also be used for undo function later
        if (!cityCentreSelected && !houseSelected && !librarySelected && !factorySelected && !roadSelected && !blockadeSelected && !wonderSelected && !towerAASelected && !towerMSelected && !grasslandSelected && !farmSelected && !forestSelected)
        {
            Debug.Log("Select a building or the remove function before selecting a node");
        }
        else
        {
            if (grasslandSelected && !selectedNode.name.Contains("Grassland")) //IF remove & IF selectedNode != Grassland THEN ignore overwriteToggle & proceed with removing gameobject + call Score.RemoveUpdate 
            {
                scoreObj.GetComponent<Score>().RemoveUpdate(selectedBuildingRemove);
                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                //NOTE to self for later if implementing UNDO - build a list which contains previously altered nodeArray, which type it is and their positions, limit list to ~50~ entrys??? - first in last out
            }
            else if (!grasslandSelected) //IF build
            {
                if (overwriteToggle.GetComponent<Toggle>().isOn) //if overwrite toggle == true THEN overwrite any building
                {
                    if (!selectedNode.name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                    {
                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                        {
                            if (selectedNode.name.Contains("Grassland")) // IF NOT overwriting THEN build
                            {
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Single Build on Grassland");
                            }
                            else // IF overwriting THEN RemoveUpdate & Build
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(selectedBuildingRemove);
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Single Build overwrite");
                            }
                        }
                    }
                }
                else //IF overwrite == false THEN only build on Grasslands
                {
                    if (!selectedNode.name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                    {
                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                        {
                            if (selectedNode.name.Contains("Grassland")) // IF NOT overwriting THEN build
                            {
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Single Build on Grassland");
                            }
                            else // IF overwriting THEN throw debug message
                            {
                                Debug.Log("Cannot overwrite building while OverWriteToggle is set to false");
                            }
                        }
                    }
                }
            }
        }
    }
    public void ShiftBuildRemove(GameObject selectedNode)// NEW METHOD - passed testing - Noting that with 9 houses + 1 city centre, it is possible to overwrite city centre with a 10th house - passes numbuildinglimit check - this is acceptable
    {
        SelectedBuildingRemoveTracker(selectedNode.name, selectedNode.transform.position.x, selectedNode.transform.position.z); // assists with tracking previously removed item, could also be used for undo function later

        if (!cityCentreSelected && !houseSelected && !librarySelected && !factorySelected && !roadSelected && !blockadeSelected && !wonderSelected && !towerAASelected && !towerMSelected && !grasslandSelected && !farmSelected && !forestSelected)
        {//check - do not execute if no build/remove is selected
            Debug.Log("Select a building or the remove function before selecting a node");
        }
        else
        {//if check success - proceed
            if (grasslandSelected && !selectedNode.name.Contains("Grassland")) //IF remove & IF selectedNode != Grassland THEN ignore overwriteToggle & proceed with removing gameobject + call Score.RemoveUpdate 
            {
                scoreObj.GetComponent<Score>().RemoveUpdate(selectedBuildingRemove);
                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                //NOTE to self for later if implementing UNDO - build a list which contains previously altered nodeArray, which type it is and their positions, limit list to ~50~ entrys??? - first in last out
            }
            else if (!grasslandSelected) //IF build
            {
                if (overwriteToggle.GetComponent<Toggle>().isOn) //if overwrite toggle == true THEN overwrite any building
                {
                    if (!selectedNode.name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                    {
                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                        {
                            if (selectedNode.name.Contains("Grassland")) // IF NOT overwriting THEN build
                            {
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Shift Build on Grassland");
                            }
                            else // IF overwriting THEN RemoveUpdate & Build
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(selectedBuildingRemove);
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Shift Build overwrite");
                            }
                        }
                    }
                }
                else
                {
                    if (!selectedNode.name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                    {
                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                        {
                            if (selectedNode.name.Contains("Grassland")) // IF NOT overwriting THEN build
                            {
                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                EditMap(NodeSelector(), selectedNode.transform.position.x, selectedNode.transform.position.z);
                                Debug.Log("Single Build on Grassland");
                            }
                            else // IF overwriting THEN throw debug message
                            {
                                Debug.Log("Cannot overwrite building while OverWriteToggle is set to false");
                            }
                        }
                    }
                }
            }
        }
    }
    public void MultiBuildRemove(GameObject selectedNode)
    {
        //yes to implement SelectedBuildingRemoveTracker(selectedNode.name, selectedNode.transform.position.x, selectedNode.transform.position.z); - not required right now, maybe later for undo
        if (!boundsSelected1 && !boundsSelected2)
        {
            boundsObj1 = selectedNode;
            boundsSelected1 = true;
            selectBoxBoundsText1.SetActive(true);
            string node1 = String.Format("x={0},y={1}", boundsObj1.transform.position.x / 3, -boundsObj1.transform.position.z / 3);
            selectBoxBoundsText1.GetComponent<Text>().text = node1;
            Debug.Log("first node selected");
            //add alt text to UIManager for boundsobj1.name
        }
        else if (boundsSelected1 && !boundsSelected2)
        {
            boundsObj2 = selectedNode;
            boundsSelected2 = true;
            selectBoxBoundsText2.SetActive(true);
            string node2 = String.Format("x={0},y={1}", boundsObj2.transform.position.x / 3, -boundsObj2.transform.position.z / 3);
            selectBoxBoundsText2.GetComponent<Text>().text = node2;
            Debug.Log("second node selected");

            float boundsObj1X = boundsObj1.GetComponent<Transform>().position.x;
            float boundsObj2X = boundsObj2.GetComponent<Transform>().position.x;
            float boundsObjY1 = boundsObj1.GetComponent<Transform>().position.z;
            float boundsObj2Y = boundsObj2.GetComponent<Transform>().position.z;
            Debug.Log(boundsObj1X + "," + boundsObjY1 + " Obj1 - " + boundsObj2X + "," + boundsObj2Y + " Obj2");
            if (boundsObj1X < boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                        {

                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                        }

                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                        {

                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                        }

                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                    {

                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                    }

                }
            }
            else if (boundsObj1X > boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                        {

                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                        }

                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                        {

                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                        }

                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                    {

                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                    }

                }
            }
            else if (boundsObj1X == boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    float x = boundsObj1X;
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {

                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    float x = boundsObj1X;
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {

                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    float x = boundsObj1X;

                    int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                    Component halo = mapListL1[index].GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                }
            }
        }
        else if (boundsSelected1 && boundsSelected2)
        {
            float boundsObj1X = boundsObj1.GetComponent<Transform>().position.x;
            float boundsObj2X = boundsObj2.GetComponent<Transform>().position.x;
            float boundsObjY1 = boundsObj1.GetComponent<Transform>().position.z;
            float boundsObj2Y = boundsObj2.GetComponent<Transform>().position.z;
            Debug.Log(boundsObj1X + "," + boundsObjY1 + " Obj1 - " + boundsObj2X + "," + boundsObj2Y + " Obj2");
            if (boundsObj1X < boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                        {
                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (grasslandSelected)//IF remove selected
                            {
                                if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                                {
                                    scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                    EditMap(NodeSelector(), x, y);

                                }
                            }
                            else if (!grasslandSelected) //IF build
                            {
                                if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }
                                else//if overwrite == false
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (mapListL1[index].name.Contains("Grassland"))
                                        {
                                            if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                            {
                                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                                EditMap(NodeSelector(), x, y);
                                            }
                                        }
                                    }

                                }
                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                        {
                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;//ss
                            if (grasslandSelected)//IF remove selected
                            {
                                if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                                {
                                    scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                    EditMap(NodeSelector(), x, y);
                                }
                            }
                            else if (!grasslandSelected) //IF build
                            {
                                if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }
                                else//if overwrite == false
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (mapListL1[index].name.Contains("Grassland"))
                                        {
                                            if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                            {
                                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                                EditMap(NodeSelector(), x, y);
                                            }
                                        }
                                    }

                                }
                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    for (float x = boundsObj1X; x <= boundsObj2X; x += 3f)//left to right Obj1 to Obj2 X
                    {
                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        if (grasslandSelected)//IF remove selected
                        {
                            if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                EditMap(NodeSelector(), x, y);
                            }
                        }
                        else if (!grasslandSelected) //IF build
                        {
                            if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                    {
                                        scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                        scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                        EditMap(NodeSelector(), x, y);
                                    }
                                }
                            }
                            else//if overwrite == false
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (mapListL1[index].name.Contains("Grassland"))
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }

                            }
                        }
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                    }

                }
            }
            else if (boundsObj1X > boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                        {
                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            if (grasslandSelected)//IF remove selected
                            {
                                if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                                {
                                    scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                    EditMap(NodeSelector(), x, y);
                                }
                            }
                            else if (!grasslandSelected) //IF build
                            {
                                if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }
                                else//if overwrite == false
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (mapListL1[index].name.Contains("Grassland"))
                                        {
                                            if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                            {
                                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                                EditMap(NodeSelector(), x, y);
                                            }
                                        }
                                    }

                                }
                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                        {
                            int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                            if (grasslandSelected)//IF remove selected
                            {
                                if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                                {
                                    scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                    EditMap(NodeSelector(), x, y);
                                }
                            }
                            else if (!grasslandSelected) //IF build
                            {
                                if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }
                                else//if overwrite == false
                                {
                                    if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                    {
                                        if (mapListL1[index].name.Contains("Grassland"))
                                        {
                                            if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                            {
                                                scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                                EditMap(NodeSelector(), x, y);
                                            }
                                        }
                                    }

                                }
                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    for (float x = boundsObj1X; x >= boundsObj2X; x -= 3f)//left to right Obj1 to Obj2 X
                    {
                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        if (grasslandSelected)//IF remove selected
                        {
                            if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                EditMap(NodeSelector(), x, y);
                            }
                        }
                        else if (!grasslandSelected) //IF build
                        {
                            if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                    {
                                        scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                        scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                        EditMap(NodeSelector(), x, y);
                                    }
                                }
                            }
                            else//if overwrite == false
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (mapListL1[index].name.Contains("Grassland"))
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }

                            }
                        }
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                    }
                }
            }
            else if (boundsObj1X == boundsObj2X)
            {
                if (boundsObjY1 < boundsObj2Y)
                {
                    for (float y = boundsObjY1; y <= boundsObj2Y; y += 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        float x = boundsObj1X;
                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        if (grasslandSelected)//IF remove selected
                        {
                            if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                EditMap(NodeSelector(), x, y);
                            }
                        }
                        else if (!grasslandSelected) //IF build
                        {
                            if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                    {
                                        scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                        scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                        EditMap(NodeSelector(), x, y);
                                    }
                                }
                            }
                            else//if overwrite == false
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (mapListL1[index].name.Contains("Grassland"))
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }

                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 > boundsObj2Y)
                {
                    float x = boundsObj1X;
                    for (float y = boundsObjY1; y >= boundsObj2Y; y -= 3f)//bottom to top Obj1 to Obj2 Y
                    {
                        int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                        if (grasslandSelected)//IF remove selected
                        {
                            if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                            {
                                scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                EditMap(NodeSelector(), x, y);
                            }
                        }
                        else if (!grasslandSelected) //IF build
                        {
                            if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                    {
                                        scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                        scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                        EditMap(NodeSelector(), x, y);
                                    }
                                }
                            }
                            else//if overwrite == false
                            {
                                if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                                {
                                    if (mapListL1[index].name.Contains("Grassland"))
                                    {
                                        if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                        {
                                            scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                            EditMap(NodeSelector(), x, y);
                                        }
                                    }
                                }

                            }
                            Component halo = mapListL1[index].GetComponent("Halo");
                            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                        }
                    }
                }
                else if (boundsObjY1 == boundsObj2Y)
                {
                    float y = boundsObjY1;
                    float x = boundsObj1X;
                    int index = 60 * ((int)-y / 3) + ((int)x / 3) - 61;
                    //
                    ////////////////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    if (grasslandSelected)//IF remove selected
                    {
                        if (!mapListL1[index].name.Contains("Grassland")) //IF Grassland is not the current selection
                        {
                            scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                            EditMap(NodeSelector(), x, y);
                        }
                    }
                    else if (!grasslandSelected) //IF build
                    {
                        if (overwriteToggle.GetComponent<Toggle>().isOn == true) //if overwrite toggle == true THEN overwrite any building
                        {
                            if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                            {
                                if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                {
                                    scoreObj.GetComponent<Score>().RemoveUpdate(mapListL1[index].name);
                                    scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                    EditMap(NodeSelector(), x, y);
                                }
                            }
                        }
                        else//if overwrite == false
                        {
                            if (!mapListL1[index].name.Contains(NodeSelector().name)) //DO NOT build IF building selected == node selected
                            {
                                if (mapListL1[index].name.Contains("Grassland"))
                                {
                                    if (PriceAndBuildLimitChecker(NodeSelector().name) == true) // DO NOT build if user does not succeed price/limit check
                                    {
                                        scoreObj.GetComponent<Score>().BuildScoreUpdate(NodeSelector().name);
                                        EditMap(NodeSelector(), x, y);
                                    }
                                }
                            }

                        }
                        Component halo = mapListL1[index].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                    }

                }
            }

            Debug.Log(boundsObj1X + "," + boundsObjY1 + " Obj1 - " + boundsObj2X + "," + boundsObj2Y + " Obj2");
            //uninitialise bounds variables
            Debug.Log("Finished Multi Build/Remove");
            boundsSelected1 = false;
            boundsSelected2 = false;
            boundsObj1 = null;
            boundsObj2 = null;
        }
    }
    public bool PriceAndBuildLimitChecker(string node)
    {
        bool returnBool = false;
        if (grasslandSelected == true)//if remove selected - pricecheck is not required as refund will occur
        {
            returnBool = true;
        }
        else //if build is selected - pricecheck required as score will be deducted
        {
            Debug.Log(scoreObj.GetComponent<Score>().score);
            switch (node)
            {
                case "City Centre":
                    returnBool = true;
                    break;
                case "House":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().housePrice && scoreObj.GetComponent<Score>().numHouses < scoreObj.GetComponent<Score>().maxNumHouses) { returnBool = true; }
                    break;
                case "Library":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().libraryPrice && scoreObj.GetComponent<Score>().numLibraries < scoreObj.GetComponent<Score>().maxNumLibraries) { returnBool = true; }
                    break;
                case "Factory":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().factoryPrice && scoreObj.GetComponent<Score>().numFactories < scoreObj.GetComponent<Score>().maxNumFactories) { returnBool = true; }
                    break;
                case "Wonder":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().wonderPrice && scoreObj.GetComponent<Score>().numWonders < scoreObj.GetComponent<Score>().maxNumWonders) { returnBool = true; }
                    break;
                case "Road":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().roadPrice) { returnBool = true; }
                    break;
                case "Blockade":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().blockadePrice) { returnBool = true; }
                    break;
                case "TowerAA":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().towerAAPrice) { returnBool = true; }
                    break;
                case "Tower":
                    if (scoreObj.GetComponent<Score>().score > scoreObj.GetComponent<Score>().towerMPrice) { returnBool = true; }
                    break;
                case "Forest":
                    returnBool = true;
                    break;
                case "Farm":
                    returnBool = true;
                    break;
            }
        }
        Debug.Log(returnBool);
        return returnBool;
    }
    public GameObject NodeSelector()
    {
        if (houseSelected) { return house; }
        else if (librarySelected) { return library; }
        else if (factorySelected) { return factory; }
        else if (cityCentreSelected) { return citycentre; }
        else if (wonderSelected) { return wonder; }
        else if (farmSelected) { return farm; }
        else if (grasslandSelected) { return grassland; }
        else if (forestSelected) { return forest; }
        else if (blockadeSelected) { return blockade; }
        else if (roadSelected) { return road; }
        else if (towerAASelected) { return towerAA; }
        else if (towerMSelected) { return towerM; }
        else { return null; }

    }



    /// <summary>
    /// edit map
    /// </summary>
    public void EditMap(GameObject replacement, float x, float y) //edited 01/08/22 due to refactoring all build/remove functions - single/shift/multi 
    {
        //declear and set variables for Instantiation
        int replacementX = (int)x / 3;
        int replacementY = (int)y / 3;
        float buildingHeight = 1.2f;
        replacement.transform.localScale.Set(2, 1, 2);//edit size of gameobject

        float indexPreProcessed = (60 * -replacementY - (60 - replacementX)) - 1; //get index from x & y pos of replacement GameObject
        int index = (int)indexPreProcessed; //cast as int

        int weight;
        switch (replacement.name)
        {
            case "Grassland":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, 0, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list
                weight = 2;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "City Centre":

                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "House":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                        //edit L2 list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Library":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                         //edit L2 list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Factory":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                     //edit L2 listss
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Wonder":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                  //edit L2 list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Road":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                        //edit L2 list
                weight = 1;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Blockade":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                           //edit L2 list
                weight = 20;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "TowerAA":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                //edit L2 list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Tower":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                    //edit L2 list
                weight = 500;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Forest":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list                                                                                                               //edit L2 list
                weight = 10;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
            case "Farm":
                Destroy(mapListL1[index]); //destroy gameobject at index of mapListL1
                mapListL1.RemoveAt(index); //remove gameobject from list
                mapListL1.Insert(index, Instantiate(replacement, position = new Vector3(x, buildingHeight, y), Quaternion.identity)); //instantiate obj and insert instantiated obj into list
                                                                                                                                      //edit L2 list
                weight = 3;
                if (SaveAndLoad.IsReadyPlay == false) { mapListL2.RemoveAt(index); mapListL2.Insert(index, CellState.MapListL2[index]); }
                else { mapListL2.RemoveAt(index); mapListL2.Insert(index, string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight)); }
                break;
        }
    }
    /// <summary>
    /// generate map
    /// </summary>
    /// 

    public void DrawBoardDefault()
    {
        mapListL1 = new List<GameObject>();
        mapListL2 = new List<string>();

        //NEW MAP GENERATIONss
        int weight = 2;
        for (int ii = 1; ii <= 33; ii++)
        {
            for (int j = 1; j <= 60; j++)
            {
                GameObject obj = grassland;
                obj.transform.localScale.Set(2, 1, 2);
                mapListL1.Add(Instantiate(obj, position = new Vector3(j * offSet, 0, -ii * offSet), Quaternion.identity));
                mapListL2.Add(string.Format("{0},1-0,2-0,3-0,4-0,5-0", weight));

            }
        }
    }
    public void DrawBoardLoad()
    {
        int index = 0;
        CellState.playerSpawn = ".";
        CellState.opponentSpawn = ".";
        for (int ii = 1; ii <= 33; ii++)
        {
            for (int j = 1; j <= 60; j++)
            {
                switch (CellState.MapListL1[index])
                {
                    case "Grassland(Clone)":
                        //do nothing
                        break;
                    case "City Centre(Clone)":
                        Debug.Log(-ii * offSet + " " + j * offSet + " " + offSet);
                        EditMap(citycentre, j * offSet, -ii * offSet);
                        break;
                    case "House(Clone)":
                        Debug.Log(CellState.MapListL2.Count());
                        Debug.Log(CellState.MapListL1.Count() + "COUNTERZ");
                        EditMap(house, j * offSet, -ii * offSet);
                        break;
                    case "Library(Clone)":
                        EditMap(library, j * offSet, -ii * offSet);
                        break;
                    case "Factory(Clone)":
                        EditMap(factory, j * offSet, -ii * offSet);
                        break;
                    case "Wonder(Clone)":
                        EditMap(wonder, j * offSet, -ii * offSet);
                        break;
                    case "Road(Clone)":
                        EditMap(road, j * offSet, -ii * offSet);
                        break;
                    case "Blockade(Clone)":
                        EditMap(blockade, j * offSet, -ii * offSet);
                        break;
                    case "TowerAA(Clone)":
                        EditMap(towerAA, j * offSet, -ii * offSet);
                        break;
                    case "Tower(Clone)":
                        EditMap(towerM, j * offSet, -ii * offSet);
                        break;
                    case "Forest(Clone)":
                        EditMap(forest, j * offSet, -ii * offSet);
                        break;
                    case "Farm(Clone)":
                        EditMap(farm, j * offSet, -ii * offSet);
                        break;
                }
                index++;
            }
        }

    }
    public void DrawBoardInGame()
    {
        int index = 0;
        for (int ii = 1; ii <= 33; ii++)
        {
            for (int j = 1; j <= 60; j++)
            {
                switch (CellState.MapListL1[index])
                {
                    case "Grassland(Clone)":
                        //do nothing
                        break;
                    case "City Centre(Clone)":
                        Debug.Log(-ii * offSet + " " + j * offSet + " " + offSet);
                        EditMap(citycentre, j * offSet, -ii * offSet);
                        break;
                    case "House(Clone)":
                        Debug.Log(CellState.MapListL2.Count());
                        Debug.Log(CellState.MapListL1.Count() + "COUNTERZ");
                        EditMap(house, j * offSet, -ii * offSet);
                        break;
                    case "Library(Clone)":
                        EditMap(library, j * offSet, -ii * offSet);
                        break;
                    case "Factory(Clone)":
                        EditMap(factory, j * offSet, -ii * offSet);
                        break;
                    case "Wonder(Clone)":
                        EditMap(wonder, j * offSet, -ii * offSet);
                        break;
                    case "Road(Clone)":
                        EditMap(road, j * offSet, -ii * offSet);
                        break;
                    case "Blockade(Clone)":
                        EditMap(blockade, j * offSet, -ii * offSet);
                        break;
                    case "TowerAA(Clone)":
                        EditMap(towerAA, j * offSet, -ii * offSet);
                        break;
                    case "Tower(Clone)":
                        EditMap(towerM, j * offSet, -ii * offSet);
                        break;
                    case "Forest(Clone)":
                        EditMap(forest, j * offSet, -ii * offSet);
                        break;
                    case "Farm(Clone)":
                        EditMap(farm, j * offSet, -ii * offSet);
                        break;
                }
                index++;
            }
        }
  
    }
    public void DrawBoard()
    {
        if (SaveAndLoad.IsLoad)
        {
            DrawBoardDefault();
            DrawBoardLoad();
        }
        else if (SaveAndLoad.IsNew)
        {
            DrawBoardDefault();
        }
         /*else if (SaveAndLoad.IsInGame)
         {
             DrawBoardDefault();
             DrawBoardInGame();
         }*/
        // SaveAndLoad.IsReadyPlay = true;
        // SaveAndLoad.IsNew = false;
        // SaveAndLoad.IsLoad = false;
        // SaveAndLoad.IsInGame = true; 
        SaveAndLoad.IsReadyPlay = true;
    }
    public void PublishCity()
    {
        //update all cellstate + scorestate static vars
        Debug.Log(CellState.MapListL1.Count() + " before clearing lists");
        Debug.Log(CellState.MapListL2.Count());
        CellState.MapListL1.Clear();
        CellState.MapListL2.Clear();
        Debug.Log(CellState.MapListL1.Count() + " after clearing lists");
        Debug.Log(CellState.MapListL2.Count());

        int index = 0;
        foreach (GameObject obj in mapListL1)
        {
            CellState.MapListL1.Add(obj.name); 
            CellState.MapListL2.Add(mapListL2[index]);
            index++;
        }
        AssignScoreStateValues();
        Debug.Log(CellState.MapListL1.Count() + " after redoing lists");
        Debug.Log(CellState.MapListL2.Count());
        //finished

        //save state to .txt file
        SaveAndLoad.SaveState();

    }

    public void AssignScoreStateValues()
    {
        ScoreState.MaxNumLibraries = scoreObj.GetComponent<Score>().maxNumLibraries;
        ScoreState.MaxNumFactories = scoreObj.GetComponent<Score>().maxNumFactories;
        ScoreState.MaxNumHouses = scoreObj.GetComponent<Score>().maxNumHouses;
        ScoreState.MaxNumWonders = scoreObj.GetComponent<Score>().maxNumWonders;
        ScoreState.NumAATowers = scoreObj.GetComponent<Score>().numAATowers;
        ScoreState.NumMTowers = scoreObj.GetComponent<Score>().numMTowers;
        ScoreState.NumHouses = scoreObj.GetComponent<Score>().numHouses;
        ScoreState.NumLibraries = scoreObj.GetComponent<Score>().numLibraries;
        ScoreState.NumFactories = scoreObj.GetComponent<Score>().numFactories;
        ScoreState.NumCityCentres = scoreObj.GetComponent<Score>().numCityCentres;
        ScoreState.NumWonders = scoreObj.GetComponent<Score>().numWonders;
        ScoreState.NumRoads = scoreObj.GetComponent<Score>().numRoads;
        ScoreState.NumBlockades = scoreObj.GetComponent<Score>().numBlockades;
        ScoreState.NumFarms = scoreObj.GetComponent<Score>().numFarms;
        ScoreState.NumForests = scoreObj.GetComponent<Score>().numForests;

        ScoreState.TowerAAPrice = scoreObj.GetComponent<Score>().towerAAPrice;
        ScoreState.TowerMPrice = scoreObj.GetComponent<Score>().towerMPrice;
        ScoreState.HousePrice = scoreObj.GetComponent<Score>().housePrice;
        ScoreState.LibraryPrice = scoreObj.GetComponent<Score>().libraryPrice;
        ScoreState.FactoryPrice = scoreObj.GetComponent<Score>().factoryPrice;
        ScoreState.WonderPrice = scoreObj.GetComponent<Score>().wonderPrice;
        ScoreState.RoadPrice = scoreObj.GetComponent<Score>().roadPrice;
        ScoreState.BlockadePrice = scoreObj.GetComponent<Score>().blockadePrice;

        ScoreState.HouseGainRate = scoreObj.GetComponent<Score>().houseGainRate;
        ScoreState.FactoryGainRate = scoreObj.GetComponent<Score>().factoryGainRate;
        ScoreState.FactoryMultiplier = scoreObj.GetComponent<Score>().factoryMultiplier;
        ScoreState.LibraryMultiplier = scoreObj.GetComponent<Score>().libraryMultiplier;
        ScoreState.WonderMultiplier = scoreObj.GetComponent<Score>().wonderMultiplier;
    }

    public void SelectNone()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectHouse()
    {
        houseSelected = true; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectBlockade()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = true; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectRoad()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = true; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectWonder()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = true; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectLibrary()
    {
        houseSelected = false; librarySelected = true; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectFactory()
    {
        houseSelected = false; librarySelected = false; factorySelected = true; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectCityCentre()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = true; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectTowerM()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = true;
    }
    public void SelectTowerAA()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = false; towerAASelected = true; towerMSelected = false;
    }
    public void SelectForest()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = false; forestSelected = true; towerAASelected = false; towerMSelected = false;
    }
    public void SelectFarm()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = true; grasslandSelected = false; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectGrassland()
    {
        houseSelected = false; librarySelected = false; factorySelected = false; wonderSelected = false; cityCentreSelected = false; roadSelected = false; blockadeSelected = false; farmSelected = false; grasslandSelected = true; forestSelected = false; towerAASelected = false; towerMSelected = false;
    }
    public void SelectedBuildingRemoveTracker(string buildingName, float x, float z)
    {
        selectedBuildingRemove = buildingName;
        selectedBuildingRemoveX = x;
        selectedBuildingRemoveZ = z;

    }

}
