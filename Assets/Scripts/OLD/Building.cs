/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIManager;

public class Building : MonoBehaviour
{
    public GameObject house, library, factory, citycentre, wonder, road, blockade, forest, grassland, towerAA, towerM, farm;
    public bool isBuildPlacementAllowed, buildingSelected, spawnPlayerSelected, spawnOpponentSelected, removeSelected;
    //public bool createHouseBool, createLibraryBool, createFactoryBool, createCityCentreBool, createWonderBool;
    public GameObject cell;//SelectedCell,
    public GameObject gameKeeper, plane, panelBuild, selectedNode, score;
    public GameObject selectBoxBounds1, selectBoxBounds2, selectBoxBoundsText1, selectBoxBoundsText2;
    public bool hasSelectedBoxBounds1, hasSelectedBoxBounds2;
    public float boxBoundsX1, boxBoundsX2, boxBoundsY1, boxBoundsY2;
    public BuildingKeeper buildingKeeper;
    public GameObject objectToDelete;

    //***********************************************************
    public GameObject TempObj;
    //***********************************************************

    //should look at swapping all the code out from selector bools and change to enums
    private void Update()
    {

        if (buildingSelected && !spawnPlayerSelected && !spawnOpponentSelected && !removeSelected) //spawn building
        {
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            if (Input.GetMouseButton(0) && Input.GetButton("left shift"))//multiple BUILD function
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 500.0f))
                {
                    if (hit.collider.name.Contains("Grassland"))//ss
                    {
                        if (CellState.playerSpawnBool == false && CellState.opponentSpawnBool == false)
                        {
                            TempObj = hit.collider.gameObject; //6
                            selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
                           // TempObj.GetComponent<Map>().EditMap();
                            //BuildingKeeper.SelectedCell.gameObject.GetInstanceID();
                        }
                        else if (CellState.playerSpawnBool == true || CellState.opponentSpawnBool == true)
                        {
                            Debug.Log("Remove player and/or opponent spawn/s in order to utilise the multiple build function");
                        }
                    }
                }
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            else if (Input.GetButton("left alt"))//box build function
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 500.0f))
                    {
                        Debug.Log(hasSelectedBoxBounds1);
                        Debug.Log(hasSelectedBoxBounds2);
                        if ((hasSelectedBoxBounds1 && hasSelectedBoxBounds2) || hit.collider.gameObject.name.Contains("City Centre") || hit.collider.gameObject.name.Contains("Road") || hit.collider.gameObject.name.Contains("Blockade") || hit.collider.gameObject.name.Contains("Grassland") || hit.collider.gameObject.name.Contains("Forest") || hit.collider.gameObject.name.Contains("Wonder") || hit.collider.gameObject.name.Contains("Tower") || hit.collider.gameObject.name.Contains("TowerAA") || hit.collider.gameObject.name.Contains("Library") || hit.collider.gameObject.name.Contains("Factory") || hit.collider.gameObject.name.Contains("House") || hit.collider.gameObject.name.Contains("Farm"))
                        {//if a raycast hits a building... NOT any other object THEN...




                            if (hasSelectedBoxBounds1 == false && hasSelectedBoxBounds2 == false) // if bounds are not set
                            {

                                selectBoxBounds1 = hit.collider.gameObject;
                                hasSelectedBoxBounds1 = true;//ss
                                selectBoxBoundsText1.SetActive(true);

                                boxBoundsX1 = hit.collider.gameObject.GetComponent<Transform>().position.x; //check for the x,y pos to find the exact node selected 
                                boxBoundsY1 = hit.collider.gameObject.GetComponent<Transform>().position.z;

                                boxBoundsX1 /= 3;
                                boxBoundsY1 /= 3;

                                string BoxBounds1Temp = boxBoundsY1.ToString();//convert to string to substring the negative off the beginning of the z position/y variable
                                string selectBoxBounds1Temp = String.Format("x={0},y={1}", boxBoundsX1, BoxBounds1Temp.Substring(1));//continue^
                                Debug.Log(selectBoxBounds1Temp);
                                selectBoxBoundsText1.GetComponent<Text>().text = selectBoxBounds1Temp;//ss
                            }
                            else if (hasSelectedBoxBounds1 == true && hasSelectedBoxBounds2 == false) // if the first bounds is set
                            {
                                //if (Physics.Raycast(ray, out hit, 500.0f))
                                //{

                                selectBoxBounds2 = hit.collider.gameObject;
                                hasSelectedBoxBounds2 = true;
                                selectBoxBoundsText2.SetActive(true);

                                boxBoundsX2 = hit.collider.gameObject.GetComponent<Transform>().position.x; //check for the x,y pos to find the exact node selected 
                                boxBoundsY2 = hit.collider.gameObject.GetComponent<Transform>().position.z;

                                boxBoundsX2 /= 3;
                                boxBoundsY2 /= 3;

                                string BoxBounds2Temp = boxBoundsY2.ToString();//convert to string to substring the negative off the beginning of the z position/y variable
                                string selectBoxBounds2Temp = String.Format("x={0},y={1}", boxBoundsX2, BoxBounds2Temp.Substring(1));//continue^
                                Debug.Log(selectBoxBounds2Temp);
                                selectBoxBoundsText2.GetComponent<Text>().text = selectBoxBounds2Temp;//ss
                                                                                                      //SUCCESSFULLY CONVERTS BOXBOUNDS2 POS TO TEXT FIELD ON BOTTOM UIManager PANEL!
                                                                                                      // }
                            }
                            else if (hasSelectedBoxBounds1 == true && hasSelectedBoxBounds2 == true) // if both bounds are set
                            {
                                float tempX = selectBoxBounds1.transform.position.x;//get x & z position of BoxBounds1
                                float tempZ = selectBoxBounds1.transform.position.z;

                                float tempX2 = selectBoxBounds2.transform.position.x;//get x & z position of BoxBounds2
                                float tempZ2 = selectBoxBounds2.transform.position.z;

                                List<string> nodeSelectionList = new List<string>();


                                //all use cases for the orientation of the first and second node selection [ expand to see more ]
                                if (tempX > tempX2)//box2x is to the left of box1x
                                {
                                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                    if (tempZ < tempZ2)//top left
                                    {
                                        Debug.Log("box2 is above and to the left of box1");


                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x >= tempX2; x -= 3f)
                                        {
                                            for (float y = tempZ; y <= tempZ2; y += 3f)
                                            {



                                                //process position variables
                                                float tempSelectedX = x / 3;
                                                float tempSelectedY = y / 3;
                                                string selectedX = tempSelectedX.ToString();
                                                string selectedY = tempSelectedY.ToString().Substring(1);
                                                Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                                string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                                //store processed position variables
                                                nodeSelectionList.Add(processedNodePos);
                                            }
                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j >= countX2; j--)//column
                                        {
                                            Debug.Log("reached here also");
                                            for (int i = listY[iterator]; i >= countY2; i--)//row 
                                            {
                                                //count = (60 * countY2 - (60 - countX2))
                                                count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
                                                count -= 1;
                                                if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                                {

                                                    /*BuildingKeeper.SelectedCell = plane.GetComponent<Map>().mapListL1[count];
                                                     BuildingKeeper.SelectedCell.GetComponent<Replace>().callRemove();
                                                     Destroy(BuildingKeeper.SelectedCell);*/
/*
                                                    buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                    buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                    Debug.Log("complete success");
                                                }
                                                else
                                                {
                                                    string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                    Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                                }
                                                iterator++;
                                            }
                                        }
                                    }
                                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                    else if (tempZ == tempZ2)//middle left
                                    {
                                        float y = 0f;
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x >= tempX2; x -= 3f)
                                        {
                                            //process position variables
                                            y = tempZ;
                                            float tempSelectedX = x / 3;
                                            float tempSelectedY = y / 3;
                                            string selectedX = tempSelectedX.ToString();
                                            string selectedY = tempSelectedY.ToString().Substring(1);
                                            Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                            string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                            //store processed position variables
                                            nodeSelectionList.Add(processedNodePos);

                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j >= countX2; j--)//column
                                        {
                                            Debug.Log("reached here also");
                                            //for (int i = listY[iterator]; i >= countY2; i--)//row ss
                                            // {
                                            // string processed1Y1 = y.ToString();
                                            // int processedY1Int = processed1Y1.IndexOf('.');
                                            // string processed2Y1 = processed1Y1.Substring();
                                            count = (60 * countY2 - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane objectss
                                            count -= 1;
                                            if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                            {
                                                buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                Debug.Log("complete success");
                                            }
                                            else
                                            {
                                                string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                            }
                                            iterator++;
                                            // }
                                        }

                                    }
                                    else if (tempZ > tempZ2)//bottom left
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x >= tempX2; x -= 3f)
                                        {
                                            for (float y = tempZ; y >= tempZ2; y -= 3f)
                                            {
                                                //process position variables
                                                float tempSelectedX = x / 3;
                                                float tempSelectedY = y / 3;
                                                string selectedX = tempSelectedX.ToString();
                                                string selectedY = tempSelectedY.ToString().Substring(1);
                                                Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                                string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                                //store processed position variables
                                                nodeSelectionList.Add(processedNodePos);
                                            }
                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j >= countX2; j--)//column
                                        {
                                            Debug.Log("reached here also");
                                            for (int i = listY[iterator]; i <= countY2; i++)//row 
                                            {
                                                count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
                                                count -= 1;
                                                if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                                {
                                                    buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                    buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                    Debug.Log("complete success");
                                                }
                                                else
                                                {
                                                    string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                    Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                                }
                                                iterator++;
                                            }
                                        }
                                    }
                                }
                                else if (tempX < tempX2)//box2x is to the right of box1x
                                {
                                    if (tempZ < tempZ2)//top right
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x <= tempX2; x += 3f)
                                        {
                                            for (float y = tempZ; y <= tempZ2; y += 3f)
                                            {
                                                //process position variables
                                                float tempSelectedX = x / 3;
                                                float tempSelectedY = y / 3;
                                                string selectedX = tempSelectedX.ToString();
                                                string selectedY = tempSelectedY.ToString().Substring(1);
                                                Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                                string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                                //store processed position variables
                                                nodeSelectionList.Add(processedNodePos);
                                            }
                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j <= countX2; j++)//column
                                        {
                                            Debug.Log("reached here also");
                                            for (int i = listY[iterator]; i >= countY2; i--)//row 
                                            {
                                                count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
                                                count -= 1;
                                                if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                                {
                                                    buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                    buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                    Debug.Log("complete success");
                                                }
                                                else
                                                {
                                                    string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                    Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                                }
                                                iterator++;
                                            }
                                        }
                                    }
                                    else if (tempZ > tempZ2)//bottom right
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x <= tempX2; x += 3f)
                                        {
                                            for (float y = tempZ; y >= tempZ2; y -= 3f)
                                            {
                                                //process position variables
                                                float tempSelectedX = x / 3;
                                                float tempSelectedY = y / 3;
                                                string selectedX = tempSelectedX.ToString();
                                                string selectedY = tempSelectedY.ToString().Substring(1);
                                                Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                                string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                                //store processed position variables
                                                nodeSelectionList.Add(processedNodePos);
                                            }
                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j <= countX2; j++)//column
                                        {
                                            Debug.Log("reached here also");
                                            for (int i = listY[iterator]; i <= countY2; i++)//row 
                                            {
                                                count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
                                                count -= 1;
                                                if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                                {
                                                    buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                    buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                    Debug.Log("complete success");
                                                }
                                                else
                                                {
                                                    string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                    Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                                }
                                                iterator++;
                                            }
                                        }
                                    }
                                    else if (tempZ == tempZ2)//middle right
                                    {
                                        float y = 0f;
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float x = tempX; x <= tempX2; x += 3f)
                                        {
                                            //process position variables
                                            y = tempZ;
                                            float tempSelectedX = x / 3;
                                            float tempSelectedY = y / 3;
                                            string selectedX = tempSelectedX.ToString();
                                            string selectedY = tempSelectedY.ToString().Substring(1);
                                            Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                            string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                            //store processed position variables
                                            nodeSelectionList.Add(processedNodePos);

                                        }
                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        for (int j = listX[iterator]; j <= countX2; j++)//column
                                        {
                                            Debug.Log("reached here also");
                                            //for (int i = listY[iterator]; i >= countY2; i--)//row ss
                                            // {
                                            // string processed1Y1 = y.ToString();
                                            // int processedY1Int = processed1Y1.IndexOf('.');
                                            // string processed2Y1 = processed1Y1.Substring();
                                            count = (60 * countY2 - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane objectss
                                            count -= 1;
                                            if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                            {
                                                buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                Debug.Log("complete success");
                                            }
                                            else
                                            {
                                                string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                            }
                                            iterator++;
                                            // }
                                        }
                                    }
                                }
                                else if (tempX == tempX2)//box2x is on the same row as box1x
                                {
                                    if (tempZ < tempZ2)//top middle
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed
                                        float x = 0f;
                                        for (float y = tempZ; y <= tempZ2; y += 3f)
                                        {
                                            //process position variables
                                            x = tempX;
                                            float tempSelectedX = x / 3;
                                            float tempSelectedY = y / 3;
                                            string selectedX = tempSelectedX.ToString();
                                            string selectedY = tempSelectedY.ToString().Substring(1);
                                            Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                            string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                            //store processed position variables
                                            nodeSelectionList.Add(processedNodePos);
                                        }

                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//

                                        Debug.Log("reached here also");
                                        for (int i = listY[iterator]; i >= countY2; i--)//row 
                                        {
                                            count = (60 * i - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
                                            count -= 1;
                                            if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                            {
                                                buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                Debug.Log("complete success");
                                            }
                                            else
                                            {
                                                string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                            }
                                            iterator++;
                                        }

                                    }
                                    else if (tempZ > tempZ2)//bottom middle
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed
                                        for (float y = tempZ; y >= tempZ2; y -= 3f)
                                        {
                                            float x = tempX;
                                            //process position variables
                                            float tempSelectedX = x / 3;
                                            float tempSelectedY = y / 3;
                                            string selectedX = tempSelectedX.ToString();
                                            string selectedY = tempSelectedY.ToString().Substring(1);
                                            Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                            string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                            //store processed position variables
                                            nodeSelectionList.Add(processedNodePos);
                                        }

                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//
                                        Debug.Log("reached here also");
                                        for (int i = listY[iterator]; i <= countY2; i++)//row 
                                        {
                                            count = (60 * i - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
                                            count -= 1;
                                            if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                            {
                                                buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


                                                buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
                                                Debug.Log("complete success");
                                            }
                                            else
                                            {
                                                string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
                                                Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
                                            }
                                            iterator++;
                                        }

                                    }
                                    else if (tempZ == tempZ2)//middle middle
                                    {
                                        //iterate through all selected nodeArray until all node positions are processed

                                        //process position variables
                                        float x = tempX;
                                        float y = tempZ;
                                        float tempSelectedX = x / 3;
                                        float tempSelectedY = y / 3;
                                        string selectedX = tempSelectedX.ToString();
                                        string selectedY = tempSelectedY.ToString().Substring(1);
                                        Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
                                        string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
                                        //store processed position variables
                                        nodeSelectionList.Add(processedNodePos);


                                        Debug.Log(nodeSelectionList.Count);

                                        List<int> listX = new List<int>();
                                        List<int> listY = new List<int>();
                                        foreach (string nodePosition in nodeSelectionList)
                                        {
                                            //get individual x and y from nodePosition var - current node
                                            string[] splitInput = nodePosition.Split(',');
                                            string stringCountX = splitInput[0];
                                            string stringCountY = splitInput[1];
                                            int countX = int.Parse(stringCountX);
                                            int countY = int.Parse(stringCountY);
                                            listX.Add(countX);
                                            listY.Add(countY);
                                        }


                                        //get individual x and y from nodePosition var - last node
                                        string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
                                        string[] splitInput2 = nodePosition2.Split(',');
                                        string stringCountX2 = splitInput2[0];
                                        string stringCountY2 = splitInput2[1];
                                        int countX2 = int.Parse(stringCountX2);
                                        int countY2 = int.Parse(stringCountY2);


                                        //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
                                        Debug.Log("reached here");

                                        Debug.Log(listX[0]);
                                        Debug.Log(listY[0]);
                                        Debug.Log(listX.Count);
                                        Debug.Log(listY.Count);

                                        int count = 0;
                                        int iterator = 0;//ss
                                                         // count = (60 * countY2 - (60 - countX2))
                                        count = (60 * countY2 - (60 - countX2)) - 1;//using x,y position calculate the position of the node within mapListL1 in plane object
                                       
                                        Debug.Log(count);
                                        if (plane.GetComponent<Map>().mapListL1[count].name.Contains("Grassland")) //if node is Grassland THEN
                                        {
                                           
                                            Debug.Log(plane.GetComponent<Map>().mapListL1[count]);
                                             buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);
                                            Debug.Log(plane.GetComponent<Map>().mapListL1[count].transform.position.x);
                                            Debug.Log(plane.GetComponent<Map>().mapListL1[count].transform.position.z);
                                            Debug.Log(buildingKeeper.SelectedCell.transform.position.x);
                                            Debug.Log(buildingKeeper.SelectedCell.transform.position.z);

                                            buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);
                                            buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();


                                            Debug.Log("complete success");

                                            /* plane.GetComponent<Map>().mapListL1[index].GetComponent<Replace>().callReplace();ss
                                            objectToDelete = plane.GetComponent<Map>().mapListL1[index];
                                            buildingKeeper.UpdateSelectedCell(objectToDelete);
                                            Destroy(buildingKeeper.SelectedCell);//InvalidOperationException: Destroying the root game GameObject of a prefab is not permitted.*/
/* }
 else
 {
     string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
     Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
 }
 //iterator++;ss


}
}
}
}

else//if raycast hits any other object other than a node THEN
{
Debug.Log("not a valid object to add to the box build set");
}
}
}
}
else if (Input.GetButtonUp("left alt"))//on left alt release - release all variables and controls related to multi build
{
hasSelectedBoxBounds1 = false;
hasSelectedBoxBounds2 = false;
selectBoxBounds1 = null;
selectBoxBounds2 = null;
boxBoundsX1 = 0f;
boxBoundsX2 = 0f;
boxBoundsY1 = 0f;
boxBoundsY2 = 0f;
selectBoxBoundsText1.SetActive(false);
selectBoxBoundsText2.SetActive(false);
Debug.Log("released left alt - clearing selection for multi build");
}
else if (Input.GetMouseButtonDown(0) && !Input.GetButton("left shift") && !Input.GetButton("left alt"))//single build function
{

RaycastHit hit;
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 500.0f))
{
string tempString = hit.collider.name;
Debug.Log(tempString);
if (hit.collider.name.Contains("Grassland"))
{
if (CellState.playerSpawnBool == true && CellState.opponentSpawnBool == false)
{
int tempInt1 = int.Parse(CellState.opponentSpawn);
Debug.Log(tempInt1);
buildingKeeper.UpdateSelectedCell(hit.collider.gameObject); //3
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
Debug.Log(buildingKeeper.SelectedCell);
int ii = 0;
foreach (GameObject cell in plane.GetComponent<Map>().mapListL1)
{
if (plane.GetComponent<Map>().mapListL1[ii].GetInstanceID() == buildingKeeper.SelectedCell.GetInstanceID())
{
if (ii == tempInt1)
{
 Debug.Log("Cannot build on top of a spawn");
}
else
{
 Debug.Log("arrived here");
 buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
 break;
}
if (ii == tempInt1)
{
 Debug.Log("breaking");
 break;
}
}
ii++;
}
ii = 0;
}
else if (CellState.playerSpawnBool == false && CellState.opponentSpawnBool == true)
{
int tempInt2 = int.Parse(CellState.opponentSpawn);
Debug.Log(tempInt2);
buildingKeeper.UpdateSelectedCell(hit.collider.gameObject); //4
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
Debug.Log(buildingKeeper.SelectedCell);
int iii = 0;
foreach (GameObject cell in plane.GetComponent<Map>().mapListL1)
{
if (plane.GetComponent<Map>().mapListL1[iii].GetInstanceID() == buildingKeeper.SelectedCell.GetInstanceID())
{
if (iii == tempInt2)
{
 Debug.Log("Cannot build on top of a spawn");
}
else
{
 buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
 Debug.Log("arrived here");
 break;
}
if (iii == tempInt2)
{
 Debug.Log("breaking");
 break;
}
}
iii++;
}
iii = 0;
}
else if (CellState.playerSpawnBool == true && CellState.opponentSpawnBool == true)
{

int tempInt3 = int.Parse(CellState.playerSpawn);
Debug.Log(tempInt3);
int tempInt4 = int.Parse(CellState.opponentSpawn);
Debug.Log(tempInt4);
buildingKeeper.SelectedCell = hit.collider.gameObject; //5
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
Debug.Log(buildingKeeper.SelectedCell.GetInstanceID());
// Debug.Log(plane.GetComponent<Map>().mapListL1[3].GetInstanceID());
int iiii = 0;
foreach (GameObject cell in plane.GetComponent<Map>().mapListL1)
{
//Debug.Log("reached this point");
if (plane.GetComponent<Map>().mapListL1[iiii].GetInstanceID() == buildingKeeper.SelectedCell.GetInstanceID())
{
Debug.Log("reached this point"); // i dont think code ever ends up executing... because of removeselected area underneath, this condition is never true?
if (iiii == tempInt3 || iiii == tempInt4)
{
 Debug.Log("Cannot build on top of a spawn");
 iiii++;
}
else
{
 buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
 Debug.Log("arrived here");

 break;
}
if (iiii == tempInt3 || iiii == tempInt4)
{
 Debug.Log("breaking");
 break;
}
}
iiii++;
}
iiii = 0;
}
else if (CellState.playerSpawnBool == false && CellState.opponentSpawnBool == false)
{
Debug.Log("Will not check if attempting to build on spawn positions as there are NO spawns currently");
buildingKeeper.UpdateSelectedCell(hit.collider.gameObject); //6
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
Debug.Log(buildingKeeper.SelectedCell);
buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();
// BuildingKeeper.SelectedCell.gameObject.GetInstanceID();
}
}
}
}
}    
else if (!spawnPlayerSelected && !spawnOpponentSelected && removeSelected) //delete building or spawn point and gain a refund of half of the building cost
{
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
if (Input.GetMouseButton(0) && Input.GetButton("left shift"))//multiple REMOVE function
{
RaycastHit hit;
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 500.0f))
{
if (hit.collider.name.Contains("Road") || hit.collider.name.Contains("Blockade") || hit.collider.name.Contains("City Centre") || hit.collider.name.Contains("House") || hit.collider.name.Contains("Library") || hit.collider.name.Contains("Factory") || hit.collider.name.Contains("Wonder") || hit.collider.name.Contains("Forest") || hit.collider.name.Contains("TowerAA") || hit.collider.name.Contains("Tower") || hit.collider.name.Contains("Farm"))//ss
{
if (CellState.playerSpawnBool == false && CellState.opponentSpawnBool == false)
{
buildingKeeper.UpdateSelectedCell(hit.collider.gameObject); //6
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
Debug.Log(buildingKeeper.SelectedCell);
buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
Destroy(buildingKeeper.SelectedCell);
// BuildingKeeper.SelectedCell.gameObject.GetInstanceID();
}
else if (CellState.playerSpawnBool == true || CellState.opponentSpawnBool == true)
{
Debug.Log("Remove player and/or opponent spawn/s in order to utilise the multiple build function");
}
}
}
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
else if (Input.GetButton("left alt"))//box REMOVE function
{
if (Input.GetMouseButtonDown(0))
{
Debug.Log(plane.GetComponent<Map>().mapListL1[1].transform.position.x);
Debug.Log(plane.GetComponent<Map>().mapListL1[1].transform.position.z);
RaycastHit hit;
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 500.0f))
{
Debug.Log(hasSelectedBoxBounds1);
Debug.Log(hasSelectedBoxBounds2);
if ((hasSelectedBoxBounds1 && hasSelectedBoxBounds2) || hit.collider.gameObject.name.Contains("Grassland") || hit.collider.gameObject.name.Contains("City Centre") || hit.collider.gameObject.name.Contains("Road") || hit.collider.gameObject.name.Contains("Blockade") || hit.collider.gameObject.name.Contains("Forest") || hit.collider.gameObject.name.Contains("Wonder") || hit.collider.gameObject.name.Contains("Tower") || hit.collider.gameObject.name.Contains("TowerAA") || hit.collider.gameObject.name.Contains("Library") || hit.collider.gameObject.name.Contains("Factory") || hit.collider.gameObject.name.Contains("House") || hit.collider.gameObject.name.Contains("Farm"))
{//if a raycast hits a building... NOT any other object THEN...
if (hasSelectedBoxBounds1 == false && hasSelectedBoxBounds2 == false) // if bounds are not set
{
Debug.Log("BORAT IS NICE");
selectBoxBounds1 = hit.collider.gameObject;
hasSelectedBoxBounds1 = true;
selectBoxBoundsText1.SetActive(true);

boxBoundsX1 = hit.collider.gameObject.GetComponent<Transform>().position.x; //check for the x,y pos to find the exact node selected 
boxBoundsY1 = hit.collider.gameObject.GetComponent<Transform>().position.z;

boxBoundsX1 /= 3;
boxBoundsY1 /= 3;

string BoxBounds1Temp = boxBoundsY1.ToString();//convert to string to substring the negative off the beginning of the z position/y variable
string selectBoxBounds1Temp = String.Format("x={0},y={1}", boxBoundsX1, BoxBounds1Temp.Substring(1));//continue^
Debug.Log(selectBoxBounds1Temp);
selectBoxBoundsText1.GetComponent<Text>().text = selectBoxBounds1Temp;//ss
}
else if (hasSelectedBoxBounds1 == true && hasSelectedBoxBounds2 == false) // if the first bounds is set
{
//if (Physics.Raycast(ray, out hit, 500.0f))
//{

selectBoxBounds2 = hit.collider.gameObject;
hasSelectedBoxBounds2 = true;
selectBoxBoundsText2.SetActive(true);

boxBoundsX2 = hit.collider.gameObject.GetComponent<Transform>().position.x; //check for the x,y pos to find the exact node selected 
boxBoundsY2 = hit.collider.gameObject.GetComponent<Transform>().position.z;

boxBoundsX2 /= 3;
boxBoundsY2 /= 3;

string BoxBounds2Temp = boxBoundsY2.ToString();//convert to string to substring the negative off the beginning of the z position/y variable
string selectBoxBounds2Temp = String.Format("x={0},y={1}", boxBoundsX2, BoxBounds2Temp.Substring(1));//continue^
Debug.Log(selectBoxBounds2Temp);
selectBoxBoundsText2.GetComponent<Text>().text = selectBoxBounds2Temp;//ss
                                                               //SUCCESSFULLY CONVERTS BOXBOUNDS2 POS TO TEXT FIELD ON BOTTOM UIManager PANEL!
                                                               // }
}
else if (hasSelectedBoxBounds1 == true && hasSelectedBoxBounds2 == true) // if both bounds are set
{
float tempX = selectBoxBounds1.transform.position.x;//get x & z position of BoxBounds1
float tempZ = selectBoxBounds1.transform.position.z;

float tempX2 = selectBoxBounds2.transform.position.x;//get x & z position of BoxBounds2
float tempZ2 = selectBoxBounds2.transform.position.z;

List<string> nodeSelectionList = new List<string>();


//all use cases for the orientation of the first and second node selection [ expand to see more ]
if (tempX > tempX2)//box2x is to the left of box1x
{
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
if (tempZ < tempZ2)//top left
{
 Debug.Log("box2 is above and to the left of box1");


 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x >= tempX2; x -= 3f)
 {
     for (float y = tempZ; y <= tempZ2; y += 3f)
     {
         //process position variables
         float tempSelectedX = x / 3;
         float tempSelectedY = y / 3;
         string selectedX = tempSelectedX.ToString();
         string selectedY = tempSelectedY.ToString().Substring(1);
         Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
         string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
         //store processed position variables
         nodeSelectionList.Add(processedNodePos);
     }
 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j >= countX2; j--)//column
 {
     Debug.Log("reached here also");
     for (int i = listY[iterator]; i >= countY2; i--)//row 
     {
         count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
         count -= 1;
         Debug.Log(count);
         //hit.collider.gameObject.name.Contains("City Centre") || hit.collider.gameObject.name.Contains("Road") || hit.collider.gameObject.name.Contains("Blockade") || hit.collider.gameObject.name.Contains("Forest") || hit.collider.gameObject.name.Contains("Wonder") || hit.collider.gameObject.name.Contains("Tower") || hit.collider.gameObject.name.Contains("TowerAA") || hit.collider.gameObject.name.Contains("Library") || hit.collider.gameObject.name.Contains("Factory") || hit.collider.gameObject.name.Contains("House") || hit.collider.gameObject.name.Contains("Farm")
         if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
         {
             Debug.Log("box2 is TOP LEFT of box1");
             Debug.Log(count);
             buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);
             Debug.Log(plane.GetComponent<Map>().mapListL1[count].name);
             Debug.Log(buildingKeeper.SelectedCell.name);
             string tempDebugString = string.Format("x={0} | y={1}", buildingKeeper.SelectedCell.transform.position.x, buildingKeeper.SelectedCell.transform.position.z);
             Debug.Log(tempDebugString);

             //buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);
            // buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();


             buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
             // Destroy(BuildingKeeper.SelectedCell);
             Debug.Log("complete success");
         }
         else
         {
             string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
             Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
         }
         iterator++;
     }
 }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
else if (tempZ == tempZ2)//middle left
{
 float y = 0f;
 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x >= tempX2; x -= 3f)
 {
     //process position variables
     y = tempZ;
     float tempSelectedX = x / 3;
     float tempSelectedY = y / 3;
     string selectedX = tempSelectedX.ToString();
     string selectedY = tempSelectedY.ToString().Substring(1);
     Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
     string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
     //store processed position variables
     nodeSelectionList.Add(processedNodePos);

 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j >= countX2; j--)//column
 {
     Debug.Log("reached here also");
     //for (int i = listY[iterator]; i >= countY2; i--)//row ss
     // {
     // string processed1Y1 = y.ToString();
     // int processedY1Int = processed1Y1.IndexOf('.');
     // string processed2Y1 = processed1Y1.Substring();
     count = (60 * countY2 - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane objectss
     count -= 1;
     if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
     {
         buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


         buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
         // Destroy(BuildingKeeper.SelectedCell);
         Debug.Log("complete success");
     }
     else
     {
         string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
         Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
     }
     iterator++;
     // }
 }

}
else if (tempZ > tempZ2)//bottom left
{
 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x >= tempX2; x -= 3f)
 {
     for (float y = tempZ; y >= tempZ2; y -= 3f)
     {
         //process position variables
         float tempSelectedX = x / 3;
         float tempSelectedY = y / 3;
         string selectedX = tempSelectedX.ToString();
         string selectedY = tempSelectedY.ToString().Substring(1);
         Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
         string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
         //store processed position variables
         nodeSelectionList.Add(processedNodePos);
     }
 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j >= countX2; j--)//column
 {
     Debug.Log("reached here also");
     for (int i = listY[iterator]; i <= countY2; i++)//row 
     {
         count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
         count -= 1;
         if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
         {
             buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


             buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
             //   Destroy(BuildingKeeper.SelectedCell);
             Debug.Log("complete success");
         }
         else
         {
             string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
             Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
         }
         iterator++;
     }
 }
}
}
else if (tempX < tempX2)//box2x is to the right of box1x
{
if (tempZ < tempZ2)//top right
{
 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x <= tempX2; x += 3f)
 {
     for (float y = tempZ; y <= tempZ2; y += 3f)
     {
         //process position variables
         float tempSelectedX = x / 3;
         float tempSelectedY = y / 3;
         string selectedX = tempSelectedX.ToString();
         string selectedY = tempSelectedY.ToString().Substring(1);
         Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
         string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
         //store processed position variables
         nodeSelectionList.Add(processedNodePos);
     }
 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j <= countX2; j++)//column
 {
     Debug.Log("reached here also");
     for (int i = listY[iterator]; i >= countY2; i--)//row 
     {
         count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
         count -= 1;
         if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
         {
             buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


             buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
             //  Destroy(BuildingKeeper.SelectedCell);
             Debug.Log("complete success");
         }
         else
         {
             string tempDebugString = String.Format("Node is not Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
             Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
         }
         iterator++;
     }
 }
}
else if (tempZ > tempZ2)//bottom right
{
 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x <= tempX2; x += 3f)
 {
     for (float y = tempZ; y >= tempZ2; y -= 3f)
     {
         //process position variables
         float tempSelectedX = x / 3;
         float tempSelectedY = y / 3;
         string selectedX = tempSelectedX.ToString();
         string selectedY = tempSelectedY.ToString().Substring(1);
         Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
         string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
         //store processed position variables
         nodeSelectionList.Add(processedNodePos);
     }
 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j <= countX2; j++)//column
 {
     Debug.Log("reached here also");
     for (int i = listY[iterator]; i <= countY2; i++)//row 
     {
         count = (60 * i - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane object
         count -= 1;
         if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
         {
             buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


             buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
             //  Destroy(BuildingKeeper.SelectedCell);
             Debug.Log("complete success");
         }
         else
         {
             string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
             Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
         }
         iterator++;
     }
 }
}
else if (tempZ == tempZ2)//middle right
{
 float y = 0f;
 //iterate through all selected nodeArray until all node positions are processed
 for (float x = tempX; x <= tempX2; x += 3f)
 {
     //process position variables
     y = tempZ;
     float tempSelectedX = x / 3;
     float tempSelectedY = y / 3;
     string selectedX = tempSelectedX.ToString();
     string selectedY = tempSelectedY.ToString().Substring(1);
     Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
     string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
     //store processed position variables
     nodeSelectionList.Add(processedNodePos);

 }
 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 for (int j = listX[iterator]; j <= countX2; j++)//column
 {
     Debug.Log("reached here also");
     //for (int i = listY[iterator]; i >= countY2; i--)//row ss
     // {
     // string processed1Y1 = y.ToString();
     // int processedY1Int = processed1Y1.IndexOf('.');
     // string processed2Y1 = processed1Y1.Substring();
     count = (60 * countY2 - (60 - j));//using x,y position calculate the position of the node within mapListL1 in plane objectss
     count -= 1;
     if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
     {
         buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


         buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
         //  Destroy(BuildingKeeper.SelectedCell);
         Debug.Log("complete success");
     }
     else
     {
         string tempDebugString = String.Format("Node is not Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
         Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
     }
     iterator++;
     // }
 }
}
}
else if (tempX == tempX2)//box2x is on the same row as box1x
{
if (tempZ < tempZ2)//top middle
{
 //iterate through all selected nodeArray until all node positions are processed
 float x = 0f;
 for (float y = tempZ; y <= tempZ2; y += 3f)
 {
     //process position variables
     x = tempX;
     float tempSelectedX = x / 3;
     float tempSelectedY = y / 3;
     string selectedX = tempSelectedX.ToString();
     string selectedY = tempSelectedY.ToString().Substring(1);
     Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
     string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
     //store processed position variables
     nodeSelectionList.Add(processedNodePos);
 }

 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//

 Debug.Log("reached here also");
 for (int i = listY[iterator]; i >= countY2; i--)//row 
 {
     count = (60 * i - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
     count -= 1;
     if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
     {
         buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);


         buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
         // Destroy(BuildingKeeper.SelectedCell);
         Debug.Log("complete success");
     }
     else
     {
         string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
         Debug.Log(tempDebugString); //if node is Grassland, do NOT remove
     }
     iterator++;
 }

}
else if (tempZ > tempZ2)//bottom middle
{
 //iterate through all selected nodeArray until all node positions are processed
 for (float y = tempZ; y >= tempZ2; y -= 3f)
 {
     float x = tempX;
     //process position variables
     float tempSelectedX = x / 3;
     float tempSelectedY = y / 3;
     string selectedX = tempSelectedX.ToString();
     string selectedY = tempSelectedY.ToString().Substring(1);
     Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
     string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
     //store processed position variables
     nodeSelectionList.Add(processedNodePos);
 }

 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int count = 0;
 int iterator = 0;//
 Debug.Log("reached here also");
 for (int i = listY[iterator]; i <= countY2; i++)//row 
 {
     count = (60 * i - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
     count -= 1;
     if (plane.GetComponent<Map>().mapListL1[count].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[count].name.Contains("House") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[count].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[count].name.Contains("Blockade")) //if node is Grassland THEN
     {
         buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[count]);//ss


         buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
         //  Destroy(BuildingKeeper.SelectedCell);
         Debug.Log("complete success");
     }
     else
     {
         string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
         Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
     }
     iterator++;
 }

}
else if (tempZ == tempZ2)//middle middle
{

 //iterate through all selected nodeArray until all node positions are processed

 //process position variables
 float x = tempX;
 float y = tempZ;
 float tempSelectedX = x / 3;
 float tempSelectedY = y / 3;
 string selectedX = tempSelectedX.ToString();
 string selectedY = tempSelectedY.ToString().Substring(1);
 Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
 string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
 //store processed position variables
 nodeSelectionList.Add(processedNodePos);


 Debug.Log(nodeSelectionList.Count);

 List<int> listX = new List<int>();
 List<int> listY = new List<int>();
 foreach (string nodePosition in nodeSelectionList)
 {
     //get individual x and y from nodePosition var - current node
     string[] splitInput = nodePosition.Split(',');
     string stringCountX = splitInput[0];
     string stringCountY = splitInput[1];
     int countX = int.Parse(stringCountX);
     int countY = int.Parse(stringCountY);
     listX.Add(countX);
     listY.Add(countY);
 }


 //get individual x and y from nodePosition var - last node
 string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
 string[] splitInput2 = nodePosition2.Split(',');
 string stringCountX2 = splitInput2[0];
 string stringCountY2 = splitInput2[1];
 int countX2 = int.Parse(stringCountX2);
 int countY2 = int.Parse(stringCountY2);


 //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
 Debug.Log("reached here");

 Debug.Log(listX[0]);
 Debug.Log(listY[0]);
 Debug.Log(listX.Count);
 Debug.Log(listY.Count);

 int index = 0;
 int iterator = 0;//ss
                  // count = (60 * countY2 - (60 - countX2))
 index = (60 * countY2 - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
 index -= 1;
 Debug.Log(index);
 if (plane.GetComponent<Map>().mapListL1[index].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[index].name.Contains("House") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[index].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Blockade")) //if node is Grassland THEN
 {

     Debug.Log(plane.GetComponent<Map>().mapListL1[index]);
     buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[index]);
     Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.x);
     Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.z);
     Debug.Log(buildingKeeper.SelectedCell.transform.position.x);
     Debug.Log(buildingKeeper.SelectedCell.transform.position.z);

     buildingKeeper.UpdateSelectedCell(plane.GetComponent<Map>().mapListL1[index]);
     buildingKeeper.SelectedCell.GetComponent<Replace>().callReplace();


     Debug.Log("complete success");

     /* plane.GetComponent<Map>().mapListL1[index].GetComponent<Replace>().callReplace();ss
     objectToDelete = plane.GetComponent<Map>().mapListL1[index];
     buildingKeeper.UpdateSelectedCell(objectToDelete);
     Destroy(buildingKeeper.SelectedCell);//InvalidOperationException: Destroying the root game GameObject of a prefab is not permitted.*/
/*    }
     else
     {
         string tempDebugString = String.Format("Node is not Grassland, will not replace - {0},{1}", listX[iterator], listY[iterator]);
         Debug.Log(tempDebugString); //if node is NOT Grassland, do NOT replace
     }





     /* //iterate through all selected nodeArray until all node positions are processed

      //process position variables

      float tempSelectedX = tempX / 3;
      float tempSelectedY = tempZ / 3;
      string selectedX = tempSelectedX.ToString();
      string selectedY = tempSelectedY.ToString().Substring(1);
      Debug.Log(selectedX + "," + selectedY);//SUCCESS - INCLUDES ALL POSITIONS OF ALL APPROPRIATE NODES - output == 1,2 for x = 1 & y = 2 instead of output == 3,-6
      string processedNodePos = String.Format("{0},{1}", selectedX, selectedY);
      //store processed position variables
      nodeSelectionList.Add(processedNodePos);


      Debug.Log(nodeSelectionList.Count);

      List<int> listX = new List<int>();
      List<int> listY = new List<int>();
      foreach (string nodePosition in nodeSelectionList)
      {
          //get individual x and y from nodePosition var - current node
          string[] splitInput = nodePosition.Split(',');
          string stringCountX = splitInput[0];
          string stringCountY = splitInput[1];
          int countX = int.Parse(stringCountX);
          int countY = int.Parse(stringCountY);
          listX.Add(countX);
          listY.Add(countY);
      }


      //get individual x and y from nodePosition var - last node
      string nodePosition2 = nodeSelectionList[nodeSelectionList.Count - 1];
      string[] splitInput2 = nodePosition2.Split(',');
      string stringCountX2 = splitInput2[0];
      string stringCountY2 = splitInput2[1];
      int countX2 = int.Parse(stringCountX2);
      int countY2 = int.Parse(stringCountY2);


      //using these fetched variables, iterate through each node within selection and build on appropriate nodeArray
      Debug.Log("reached here");

      Debug.Log(listX[0]);
      Debug.Log(listY[0]);
      Debug.Log(listX.Count);
      Debug.Log(listY.Count);

      int index = 0;
      int iterator = 0;//ss

      // index = countX2 + 60 * countY2; //FOUND HERE https://www.codegrepper.com/code-examples/javascript/how+to+convert+2d+coords+to+1d+array+coords
      //y = index / width; - index to 2d coord
      //x = index % width;
      //index = x + width * y; - 2d coord to index of list
      //this doesn't work - if first (pos 0) was selected, x == 1 & y == 1 then 1 + 60 == 60 * 1 == 60. position 60 is not the required answer 
      index = (60 * countY2 - (60 - countX2)) - 1; // -1 at the end so it gets actual pos in list, as list index starts from 0 not 1
      Debug.Log("DEBUG1 - " + index);
      // count = (60 * countY2 - (60 - countX2));//using x,y position calculate the position of the node within mapListL1 in plane object
      // count -= 1;
      Debug.Log(index);
      int xParse = listX[0] * 3;
      int zParse = -listY[0] * 3;
      Debug.Log(xParse);
      Debug.Log(zParse);


      if (plane.GetComponent<Map>().mapListL1[index].name.Contains("City Centre") || plane.GetComponent<Map>().mapListL1[index].name.Contains("House") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Library") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Factory") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Wonder") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Tower") || plane.GetComponent<Map>().mapListL1[index].name.Contains("TowerAA") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Farm") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Forest") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Road") || plane.GetComponent<Map>().mapListL1[index].name.Contains("Blockade")) //if node is Grassland THEN
      {
          Debug.Log("DEBUG1");
          /*
int iiiii = 0;
buildingKeeper.UpdateSelectedCell(hit.collider.gameObject);
Debug.Log(buildingKeeper.SelectedCell.transform.position.x);
Debug.Log(buildingKeeper.SelectedCell.transform.position.z);
selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
string debugString = String.Format("removing {0}", hit.collider.name);
Debug.Log(debugString);
buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
Destroy(buildingKeeper.SelectedCell);


          //Destroy(plane.GetComponent<Map>().mapListL1[count]);

          Debug.Log(plane.GetComponent<Map>().mapListL1[index]);
          Debug.Log(plane.GetComponent<Map>().mapListL1[index].name);
          Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.x);
          Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.z);

          /* 
            Debug.Log(buildingKeeper.SelectedCell);
            Debug.Log(buildingKeeper.SelectedCell.transform.position.x);//ss
            Debug.Log(buildingKeeper.GetSelectedCell().transform.position.z);//ss
            Debug.Log(plane.GetComponent<Map>().mapListL1[1].name);//ssss
            Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.x);//ss
            Debug.Log(plane.GetComponent<Map>().mapListL1[index].transform.position.z);/*
            // Debug.Log(plane.GetComponent<Map>().mapListL1[index - 1].transform.position.x);//ss
            // Debug.Log(plane.GetComponent<Map>().mapListL1[index - 1].transform.position.z);


            objectToDelete = plane.GetComponent<Map>().mapListL1[index].gameObject;
          Debug.Log("DEBUG1");
            buildingKeeper.UpdateSelectedCell(objectToDelete);
          Debug.Log("DEBUG1");
          plane.GetComponent<Map>().mapListL1[index].GetComponent<Replace>().callReplace();
          Debug.Log("DEBUG1");
          Destroy(objectToDelete);//InvalidOperationException: Destroying the root game GameObject of a prefab is not permitted.

             Debug.Log("complete success");


        }
        else
        {
            string tempDebugString = String.Format("Node is Grassland, will not remove - {0},{1}", listX[iterator], listY[iterator]);
            Debug.Log(tempDebugString); //if node is Grassland, do not remove
        }
  */
//iterator++;ss

/* 
                                    }
                                  }
                              }
                          }

                          else//if raycast hits any other object other than a node THEN
                          {
                              Debug.Log("not a valid object to add to the box build set");
                          }
                      }
                  }
              }
              else if (Input.GetButtonUp("left alt"))//on left alt release - release all variables and controls related to multi REMOVE
              {
                  hasSelectedBoxBounds1 = false;
                  hasSelectedBoxBounds2 = false;
                  selectBoxBounds1 = null;
                  selectBoxBounds2 = null;
                  boxBoundsX1 = 0f;
                  boxBoundsX2 = 0f;
                  boxBoundsY1 = 0f;
                  boxBoundsY2 = 0f;
                  selectBoxBoundsText1.SetActive(false);
                  selectBoxBoundsText2.SetActive(false);
                  Debug.Log("released left alt - clearing selection for multi build");
              }
              else if (Input.GetMouseButtonDown(0) && !Input.GetButton("left shift") && !Input.GetButton("left alt"))//single remove function
              {

                  RaycastHit hit;
                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out hit, 500.0f))
                  {
                      string tempString = hit.collider.name;
                      Debug.Log(tempString);
                      if (hit.collider.name.Contains("Road") || hit.collider.name.Contains("Blockade") || hit.collider.name.Contains("City Centre") || hit.collider.name.Contains("House") || hit.collider.name.Contains("Library") || hit.collider.name.Contains("Factory") || hit.collider.name.Contains("Wonder") || hit.collider.name.Contains("Forest") || hit.collider.name.Contains("TowerAA") || hit.collider.name.Contains("Tower") || hit.collider.name.Contains("Farm"))
                      {
                          int iiiii = 0;
                          buildingKeeper.UpdateSelectedCell(hit.collider.gameObject);
                          Debug.Log(buildingKeeper.SelectedCell.transform.position.x);
                          Debug.Log(buildingKeeper.SelectedCell.transform.position.z);
                          selectedNode.GetComponent<SelectedNode>().UpdateSelectedNode();
                          string debugString = String.Format("removing {0}", hit.collider.name);
                          Debug.Log(debugString);
                          buildingKeeper.SelectedCell.GetComponent<Replace>().callRemove(buildingKeeper.SelectedCell.name);
                          Destroy(buildingKeeper.SelectedCell);
                      }
                  }



              }/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                        }
                                    }


    public void SelectSpawnPlayer()
    {
        spawnOpponentSelected = false;
        spawnPlayerSelected = true; //TRUE
        removeSelected = false;
        buildingSelected = false;
    }
    public void SelectSpawnOpponent()
    {
        spawnPlayerSelected = false;
        buildingSelected = false;
        removeSelected = false;
        spawnOpponentSelected = true; //TRUE
    }
    private void Start()
    {
        selectedNode = GameObject.Find("SelectedNode");
        plane = GameObject.Find("Plane");
        gameKeeper = GameObject.Find("GameKeeper");
        panelBuild = GameObject.Find("LeftPanel");
        score = GameObject.Find("Score");
        buildingSelected = false;
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        removeSelected = false;

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

        buildingKeeper = new BuildingKeeper(house, library, factory, citycentre, wonder, road, blockade, forest, grassland, towerAA, towerM, farm);

        hasSelectedBoxBounds1 = false;
        hasSelectedBoxBounds2 = false;
        selectBoxBoundsText1 = GameObject.Find("SelectedAltBuild1");//ss
        selectBoxBoundsText2 = GameObject.Find("SelectedAltBuild2");
        selectBoxBoundsText1.SetActive(false);
        selectBoxBoundsText2.SetActive(false);
        boxBoundsX1 = 0f;
        boxBoundsY1 = 0f;
        boxBoundsX2 = 0f;
        boxBoundsY2 = 0f;
    }
    public void CreateMTower()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = true;//TRUE
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(towerM.name + " " + "Selected");
    }
    public void CreateAATower()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = true; //TRUE
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(towerAA.name + " " + "Selected");
    }
    public void CreateBlockade()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = true; //TRUE
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(blockade.name + " " + "Selected");
    }
    public void CreateForest()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = true;//TRUE
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        CellState.createFarmBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(forest.name + " " + "Selected");
    }
    public void CreateGrassland()
    {
        removeSelected = true;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false; 
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = true; //TRUE
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(grassland.name + " " + "Selected");
    }
    public void CreateFarm()
    {
        removeSelected = false;
        CellState.createFarmBool = true; //TRUE
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(farm.name + " " + "Selected");
    }
    public void CreateHouse()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = true; //TRUE
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(house.name + " " + "Selected");
    }
    public void CreateLibrary()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = true; //TRUE
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(library.name + " " + "Selected");
    }
    public void CreateCityCentre()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = true; //TRUE
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(citycentre.name + " " + "Selected");
    }
    public void CreateFactory()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = true; //TRUE
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(factory.name + " " + "Selected");
    }
    public void CreateWonder()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = true; //TRUE
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = false;
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(wonder.name + " " + "Selected");
    }
    public void CreateRoad()
    {
        removeSelected = false;
        CellState.createFarmBool = false;
        CellState.createCityCentreBool = false;
        CellState.createFactoryBool = false;
        CellState.createWonderBool = false;
        CellState.createLibraryBool = false;
        CellState.createHouseBool = false;
        CellState.createRoadBool = true; ;//TRUE
        CellState.createGrasslandBool = false;
        CellState.createBlockadeBool = false;
        CellState.createFarmBool = false;
        CellState.createForestBool = false;
        CellState.createMTowerBool = false;
        CellState.createAATowerBool = false;
        buildingSelected = true; //TRUE
        spawnPlayerSelected = false;
        spawnOpponentSelected = false;
        Debug.Log(road.name + " " + "Selected");
    }
   
}
*/