using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public GameObject scoreTextObject, plane;
    public float delayAmount;
    public double scoreDouble, scoreDoubleTemp1, scoreDoubleTemp2, scoreDecTemp3;
    public double scoreMultiplierInternal, scoreGainRateInternal, scoreInternal;
    public double scoreFloor;
    public string scoreRemainder1, scoreRemainder2, scoreRemainder3;
    protected float timer;
    public bool isLoadingBoolForMapGen;
    public bool testTrip;
    public bool CHECKMEBOOL;
    public double roundScoreTemp, processedScore;
    public double score, scoreMultiplier, scoreGainRate;
    public double maxNumHouses, maxNumLibraries, maxNumFactories, maxNumWonders;
    public double numHouses, numLibraries, numFactories, numCityCentres, numWonders, numBlockades, numRoads, numForests, numMTowers, numAATowers, numFarms;
    public double roadPrice, blockadePrice, towerMPrice, towerAAPrice, housePrice, libraryPrice, factoryPrice, wonderPrice;
    public double houseGainRate, libraryMultiplier, factoryGainRate, factoryMultiplier, wonderMultiplier;
    public bool isLoad;
    public void Update() //swapped out FixedUpdate
    {
        if (SaveAndLoad.IsReadyPlay)
        {
           // if (SaveAndLoad.IsInGame)
           // {
                if (testTrip != true)
                {
                    testTrip = true;
                    Debug.Log("ScoreState.Score == " + ScoreState.Score);
                    Debug.Log("ScoreObj.GetComponent<Score>().score == " + score);
                    Debug.Log("First score update");
                }
                timer += Time.deltaTime;
                if (timer >= delayAmount)
                {
                    timer = 0f;
                    roundScoreTemp = score += (scoreGainRate * scoreMultiplier);
                    processedScore = Math.Round(roundScoreTemp, 10);//ss
                    UpdateScore(processedScore);
                    ScoreState.Score = processedScore;
                    ScoreState.ScoreGainRate = scoreGainRate;
                    ScoreState.ScoreMultiplier = scoreMultiplier;
                }
            //}
        }
    }
    public void Start()
    {
        //initialise variables
        plane = GameObject.Find("Plane");
        timer = 0f;
        delayAmount = 2f;
        scoreRemainder1 = "";
        scoreRemainder2 = "";
        scoreRemainder3 = "";
        scoreDouble = 0;
        scoreDoubleTemp1 = 0;
        scoreDoubleTemp2 = 0;
        scoreDecTemp3 = 0;
        scoreTextObject = gameObject;
        Debug.Log(SaveAndLoad.IsLoad);
        Debug.Log(SaveAndLoad.SaveFileSelected);
       
        if (SaveAndLoad.IsLoad)
        {
            //SaveAndLoad.TestStringOutput(); - fixed, requires .Trim('\r', '\n') on end of all strings , possibly a static var issue, or a issue with streamreader/writer?
            SaveAndLoad.LoadState();
            ScoreOnLoad();
        }
        else if (SaveAndLoad.IsNew)
        {
            PlayerCharacterStaticClass.PlayerPP = 5;
            ScoreOnNewGame();
        }
      

        //prepare for drawing the city
        Debug.Log("Ready to generate map");//s
        plane.GetComponent<Map>().DrawBoard(); // MUST ADD THIS ELSEWHERE
        if (SaveAndLoad.IsInGame)
        {
            score += SaveAndLoad.UpdateScoreOnReturnToCityScene(ScoreState.Score);
            SaveAndLoad.IsInGame = false;
        }//ss
        UpdateScore(score);
    }
    /// <summary>
    /// run once on new game to initiate variables
    /// </summary>
    public void ScoreOnNewGame()
    {
        ScoreState.Score = 10000; // 10k score on start for dev testing - change this to 100 for production build
        ScoreState.ScoreGainRate = 10;
        ScoreState.ScoreMultiplier = 1;

        ScoreState.MaxNumFactories = 0;
        ScoreState.MaxNumHouses = 0;
        ScoreState.MaxNumLibraries = 0;
        ScoreState.MaxNumWonders = 0;
        ScoreState.NumAATowers = 0;
        ScoreState.NumMTowers = 0;
        ScoreState.NumHouses = 0;
        ScoreState.NumLibraries = 0;
        ScoreState.NumFactories = 0;
        ScoreState.NumCityCentres = 0;
        ScoreState.NumWonders = 0;
        ScoreState.NumRoads = 0;
        ScoreState.NumBlockades = 0;
        ScoreState.NumFarms = 0;
        ScoreState.NumForests = 0;

        ScoreState.RoadPrice = 150;
        ScoreState.BlockadePrice = 350;
        ScoreState.TowerMPrice = 850;
        ScoreState.TowerAAPrice = 450;
        ScoreState.HousePrice = 100;
        ScoreState.LibraryPrice = 750;
        ScoreState.FactoryPrice = 2750;
        ScoreState.WonderPrice = 40000;
        ScoreState.FactoryGainRate = 7.5;
        ScoreState.FactoryMultiplier = 0.08;
        ScoreState.WonderMultiplier = 0.8;
        ScoreState.LibraryMultiplier = 0.005;
        ScoreState.HouseGainRate = 5;

        score = ScoreState.Score;
        scoreGainRate = ScoreState.ScoreGainRate;
        scoreMultiplier = ScoreState.ScoreMultiplier;
        maxNumFactories = ScoreState.MaxNumFactories;
        maxNumHouses = ScoreState.MaxNumHouses;
        maxNumLibraries = ScoreState.MaxNumLibraries;
        maxNumWonders = ScoreState.MaxNumWonders;
        numAATowers = ScoreState.NumAATowers;
        numMTowers = ScoreState.NumMTowers;
        numHouses = ScoreState.NumHouses;
        numLibraries = ScoreState.NumLibraries;
        numFactories = ScoreState.NumFactories;
        numCityCentres = ScoreState.NumCityCentres;
        numWonders = ScoreState.NumWonders;
        numRoads = ScoreState.NumRoads;
        numBlockades = ScoreState.NumBlockades;
        numFarms = ScoreState.NumFarms;
        numForests = ScoreState.NumForests;

        roadPrice = ScoreState.RoadPrice;
        blockadePrice = ScoreState.BlockadePrice;
        towerMPrice = ScoreState.TowerMPrice;
        towerAAPrice = ScoreState.TowerAAPrice;
        housePrice = ScoreState.HousePrice;
        libraryPrice = ScoreState.LibraryPrice;
        factoryPrice = ScoreState.FactoryPrice;
        wonderPrice = ScoreState.WonderPrice;
        factoryGainRate = ScoreState.FactoryGainRate;
        factoryMultiplier = ScoreState.FactoryMultiplier;
        wonderMultiplier = ScoreState.WonderMultiplier;
        libraryMultiplier = ScoreState.LibraryMultiplier;
        houseGainRate = ScoreState.HouseGainRate;
    }
    /// <summary>
    /// run once on load game to initialise variables with saved variables (currently in text file for dev purposes
    /// </summary>
    public void ScoreOnLoad()
    {
        score = ScoreState.Score;
        scoreGainRate = ScoreState.ScoreGainRate;
        scoreMultiplier = ScoreState.ScoreMultiplier;
        maxNumFactories = ScoreState.MaxNumFactories;
        maxNumHouses = ScoreState.MaxNumHouses;
        maxNumLibraries = ScoreState.MaxNumLibraries;
        maxNumWonders = ScoreState.MaxNumWonders;
        numAATowers = ScoreState.NumAATowers;
        numMTowers = ScoreState.NumMTowers;
        numHouses = ScoreState.NumHouses;
        numLibraries = ScoreState.NumLibraries;
        numFactories = ScoreState.NumFactories;
        numCityCentres = ScoreState.NumCityCentres;
        numWonders = ScoreState.NumWonders;
        numRoads = ScoreState.NumRoads;
        numBlockades = ScoreState.NumBlockades;
        numFarms = ScoreState.NumFarms;
        numForests = ScoreState.NumForests;

        roadPrice = ScoreState.RoadPrice;
        blockadePrice = ScoreState.BlockadePrice;
        towerMPrice = ScoreState.TowerMPrice;
        towerAAPrice = ScoreState.TowerAAPrice;
        housePrice = ScoreState.HousePrice;
        libraryPrice = ScoreState.LibraryPrice;
        factoryPrice = ScoreState.FactoryPrice;
        wonderPrice = ScoreState.WonderPrice;
        factoryGainRate = ScoreState.FactoryGainRate;
        factoryMultiplier = ScoreState.FactoryMultiplier;
        wonderMultiplier = ScoreState.WonderMultiplier;
        libraryMultiplier = ScoreState.LibraryMultiplier;
        houseGainRate = ScoreState.HouseGainRate;
    }
    /// <summary>
    /// default score functionality for all scenes except city
    /// </summary>
    public void ScoreDefault()
    {

    }
    /// <summary>
    /// score functionality specifically for city scene
    /// </summary>
    public void ScoreCityScene()
    {
        score = ScoreState.Score;
        scoreGainRate = ScoreState.ScoreGainRate;
        scoreMultiplier = ScoreState.ScoreMultiplier;
        maxNumFactories = ScoreState.MaxNumFactories;
        maxNumHouses = ScoreState.MaxNumHouses;
        maxNumLibraries = ScoreState.MaxNumLibraries;
        maxNumWonders = ScoreState.MaxNumWonders;
        numAATowers = ScoreState.NumAATowers;
        numMTowers = ScoreState.NumMTowers;
        numHouses = ScoreState.NumHouses;
        numLibraries = ScoreState.NumLibraries;
        numFactories = ScoreState.NumFactories;
        numCityCentres = ScoreState.NumCityCentres;
        numWonders = ScoreState.NumWonders;
        numRoads = ScoreState.NumRoads;
        numBlockades = ScoreState.NumBlockades;
        numFarms = ScoreState.NumFarms;
        numForests = ScoreState.NumForests;

        roadPrice = ScoreState.RoadPrice;
        blockadePrice = ScoreState.BlockadePrice;
        towerMPrice = ScoreState.TowerMPrice;
        towerAAPrice = ScoreState.TowerAAPrice;
        housePrice = ScoreState.HousePrice;
        libraryPrice = ScoreState.LibraryPrice;
        factoryPrice = ScoreState.FactoryPrice;
        wonderPrice = ScoreState.WonderPrice;
        factoryGainRate = ScoreState.FactoryGainRate;
        factoryMultiplier = ScoreState.FactoryMultiplier;
        wonderMultiplier = ScoreState.WonderMultiplier;
        libraryMultiplier = ScoreState.LibraryMultiplier;
        houseGainRate = ScoreState.HouseGainRate;
    }
    public void UpdateScore(double score)
    {
        if (score < 1000)
        {
            scoreDouble = Math.Round(score, 0);

            scoreTextObject.GetComponent<TMP_Text>().text = "Score: " + scoreDouble.ToString();
        }
        else if (score > 999 && score < 1000000)
        {
            scoreDouble = score;
            scoreDoubleTemp1 = scoreDouble / 1000;
            scoreDoubleTemp2 = Math.Round(scoreDoubleTemp1, 2);

            scoreTextObject.GetComponent<TMP_Text>().text = "Score: " + scoreDoubleTemp2 + "K";
        }
        else if (score > 999999 && score < 1000000000)
        {
            scoreDouble = score;
            scoreDoubleTemp1 = scoreDouble / 1000000;
            scoreDoubleTemp2 = Math.Round(scoreDoubleTemp1, 2);

            scoreTextObject.GetComponent<TMP_Text>().text = "Score: " + scoreDoubleTemp2 + "M";
        }
        else if (score > 999999999 && score < 1000000000000)
        {
            scoreDouble = score;
            scoreDoubleTemp1 = scoreDouble / 1000000000;
            scoreDoubleTemp2 = Math.Round(scoreDoubleTemp1, 2);

            scoreTextObject.GetComponent<TMP_Text>().text = "Score: " + scoreDoubleTemp2 + "Bn";
        }
        else if (score > 999999999999 && score < 1000000000000000)
        {
            scoreDouble = score;
            scoreDoubleTemp1 = scoreDouble / 1000000000000;
            scoreDoubleTemp2 = Math.Round(scoreDoubleTemp1, 2);

            scoreTextObject.GetComponent<TMP_Text>().text = "Score: " + scoreDoubleTemp2 + "Tn";
        }
    }
    public void SetWonderNumLimit()//this needs to be reevaluated - does not calculate this correctly on remove and/or build
    {
        if (numHouses >= 0 && numHouses < 20) { maxNumWonders = 0; }
        else if (numHouses >= 20 && numHouses < 40) { maxNumWonders = 1; }
        else if (numHouses >= 40 && numHouses < 60) { maxNumWonders = 2; }
        else if (numHouses >= 60 && numHouses < 80) { maxNumWonders = 3; }
        else if (numHouses >= 80 && numHouses < 100) { maxNumWonders = 4; }
        else if (numHouses >= 100 && numHouses < 120) { maxNumWonders = 5; }
        else if (numHouses >= 120 && numHouses < 140) { maxNumWonders = 6; }
        else if (numHouses >= 140) { maxNumWonders = 6; }
    }
    public void SetLibraryNumLimit()
    {
        Debug.Log("Max num of librarys before added house " + maxNumLibraries);
        Debug.Log(numHouses);
        Debug.Log(numHouses / 9);
        Debug.Log(Math.Floor(numHouses / 9));//ss
        Debug.Log(Math.Floor((double)numHouses / 9));
        Debug.Log(Math.Floor((double)(numHouses / 9)));
        double tempDouble111 = Math.Floor((double)(numHouses / 9));
        maxNumLibraries = tempDouble111;
        Debug.Log("Max num of librarys after added house " + maxNumLibraries);
    }
    public void SetFactoryNumLimit()
    {
        Debug.Log("Max num of factorys before added house " + maxNumLibraries);
        Debug.Log(numHouses);
        Debug.Log(numHouses / 5);
        Debug.Log(Math.Floor(numHouses / 5));
        Debug.Log(Math.Floor((double)numHouses / 5));
        Debug.Log(Math.Floor((double)(numHouses / 5)));
        double tempDouble222 = Math.Floor((double)(numHouses / 5));
        maxNumFactories = tempDouble222;
        Debug.Log("Max num of factorys after added house " + maxNumLibraries);
    }
    public void RemoveUpdate(string buildingName)
    {
        switch (buildingName)
        {
            case "House(Clone)":
                numHouses -= 1;
                scoreGainRate -= houseGainRate;
                score += (housePrice / 2);
                SetLibraryNumLimit();
                SetFactoryNumLimit();
                SetWonderNumLimit();
                UpdateScore(score);
                break;
            case "Road(Clone)":
                numRoads -= 1;
                break;
            case "Forest(Clone)":
                numForests -= 1;
                break;
            case "Blockade(Clone)":
                numBlockades -= 1;
                break;
            case "Tower(Clone)":
                numMTowers -= 1;
                break;
            case "TowerAA(Clone)":
                numAATowers -= 1;
                break;
            case "Farm(Clone)":
                numFarms -= 1;
                score += housePrice;

                UpdateScore(score);
                break;
            case "City Centre(Clone)":
                numCityCentres -= 1;
                maxNumHouses -= 10;
                break;
            case "Library(Clone)":
                numLibraries -= 1;
                scoreMultiplier -= libraryMultiplier;
                score += (libraryPrice / 2);
                UpdateScore(score);
                /* double tempScoreDouble3 = score;
                 tempScoreDouble3 += (libraryPrice / 2);
                 UpdateScore(tempScoreDouble3);*/
                break;
            case "Factory(Clone)":
                numFactories -= 1;
                scoreMultiplier -= factoryMultiplier;
                scoreGainRate -= factoryGainRate;
                score += (factoryPrice / 2);
                UpdateScore(score);
                break;
            case "Wonder(Clone)":
                numWonders -= 1;
                scoreMultiplier -= wonderMultiplier;//s
                score += (factoryPrice / 2);
                UpdateScore(score);
                break;
        }
    }
    public void BuildScoreUpdate(string buildingName)
    {
        Debug.Log(buildingName);
        switch (buildingName)
        {
            case "Grassland":
                //do nothing
                break;
            case "House":

                if (SaveAndLoad.isReadyPlay)
                {
                    Debug.Log(score + " " + 1);
                    Debug.Log("Building House");
                    numHouses += 1;
                    scoreGainRate += houseGainRate;
                    score -= housePrice;
                    SetLibraryNumLimit();
                    SetFactoryNumLimit();
                    SetWonderNumLimit();
                    UpdateScore(score);
                }
                break;
            case "Road":
                if (SaveAndLoad.isReadyPlay)
                {
                    numRoads += 1;
                    score -= roadPrice;
                    UpdateScore(score);
                }
                break;
            case "Forest":
                if (SaveAndLoad.isReadyPlay)
                {
                    numForests += 1;
                }
                break;
            case "Blockade":
                if (SaveAndLoad.isReadyPlay)
                {
                    numBlockades += 1;
                    score -= blockadePrice;
                    UpdateScore(score);
                }
                break;
            case "Tower":
                if (SaveAndLoad.isReadyPlay)
                {
                    numMTowers += 1;
                    score -= towerMPrice;
                    UpdateScore(score);
                }
                break;
            case "TowerAA":
                if (SaveAndLoad.isReadyPlay)
                {
                    numAATowers += 1;
                    score -= towerAAPrice;
                    UpdateScore(score);
                }
                break;
            case "Farm":
                if (SaveAndLoad.isReadyPlay)
                {
                    numFarms += 1;
                }
                break;
            case "City Centre":
                Debug.Log("DEBUG222 - SaveAndLoad.isReadyPlay = " + SaveAndLoad.isReadyPlay);
                if (SaveAndLoad.isReadyPlay)
                {
                    Debug.Log("Building City Centre");
                    numCityCentres += 1;
                    maxNumHouses += 10;
                    Debug.Log(numCityCentres);
                }
                break;
            case "Library":
                if (SaveAndLoad.isReadyPlay)
                {
                    numLibraries += 1;
                    scoreMultiplier += libraryMultiplier;
                    score -= libraryPrice;
                    UpdateScore(score);
                }
                break;
            case "Factory":
                if (SaveAndLoad.isReadyPlay)
                {
                    numFactories += 1;
                    scoreGainRate += factoryGainRate;
                    scoreMultiplier += factoryMultiplier;//ss
                    score -= factoryPrice;
                    UpdateScore(score);
                }
                break;
            case "Wonder":
                if (SaveAndLoad.isReadyPlay)
                {
                    numWonders += 1;
                    scoreMultiplier += wonderMultiplier;
                    score -= wonderPrice;
                    UpdateScore(score);
                }
                break;
        }
    }
}