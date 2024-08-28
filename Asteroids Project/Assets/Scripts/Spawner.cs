using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class Spawner : MonoBehaviour
{

    /*
     * This Script is in charge of spawning asteroids in the scene.
     * It uses empty gameObjects placed within the scene randomly chosen as spawn positions.
     */


    //components, prefabs and script references
    public Asteroid aPrefab;
    public GameObject warningPrefab;
    public UIScript sceneUI;
    public EnvrionmentController environmentScript;

    public GameObject healthPowerUpPrefab, ammoCapPrefab, ammoRechargePrefab;

    public PlayerMovement player;

    //a list of empty game objects used as spawn positions
    public GameObject[] spawnerLocations;

    //internal round count and round end 'check' variable
    private int roundCount = 1;
    private bool endOfRound = false;


    //internals Vars associated for spawning asteroids
    private int internalWaveCount = 2; 
    private int internalNumOfSpawn = 1;
    private float internalTimeGapMin = 10f;
    private float internalTimeGapMax = 16f;
    private int internalAddAmount = 1;


    //starts the initial wave on start
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        StartCoroutine(SpawnBasicAsteroid(internalWaveCount, internalNumOfSpawn, internalTimeGapMin, internalTimeGapMin));
    }



    //method to start the wave manually in case the scene is restarted.
    public void startTheGame() {
        StartCoroutine(SpawnBasicAsteroid(internalWaveCount, internalNumOfSpawn, internalTimeGapMin, internalTimeGapMin));
    }


    //checks for round end to trigger animation
    private void Update()
    {
        if (endOfRound) {//if end of round reached, initiate the next round
            endOfRound = false;
            StartCoroutine(increaseRound());
        }
    }


    //method called by animation triggers
    public void SpawnWave() {
        StartCoroutine(SpawnBasicAsteroid(internalWaveCount, internalNumOfSpawn, internalTimeGapMin, internalTimeGapMin));
    }


    //Coroutine in charge of spawning a wave.
    IEnumerator SpawnBasicAsteroid(int waveCount, int numOfSpawns, float timeGapMin, float timeGapMax) {

        int i = waveCount;

        while (i > 0) {
            yield return new WaitForSeconds(Random.Range(timeGapMin, timeGapMax));//semi-random wait time between spawns
            for (int k = 0; k < numOfSpawns; k++) {
                //picks a random spawn location, gets the position and starts the warning circle coroutine
                GameObject chosenSpwn = spawnerLocations[Random.Range(0, spawnerLocations.Length)];
                Vector3 pos = chosenSpwn.transform.position;
                StartCoroutine(SummonWarning(chosenSpwn,pos));
            }
            i -= 1;
        }
        endOfRound = true;
    }

    //spawns an animated warning where the asteroid will spawn
    IEnumerator SummonWarning(GameObject spawn, Vector3 pos) {

        //instantiates the warning, waits 5s and destroys it.
        GameObject wP = Instantiate(warningPrefab,pos,spawn.transform.rotation);
        yield return new WaitForSeconds(5f);
        Destroy(wP);

        //spawns a new asteroid on the same position, then initiates its movement func
        Asteroid ast = Instantiate(this.aPrefab, pos, spawn.transform.rotation);
        ast.asteroid();//references Asteroid script
    }


    private void spawnPowerUp() {
        GameObject spawnPos = spawnerLocations[Random.Range(0,spawnerLocations.Length)];

        int choice = Random.Range(0, 3);
        GameObject powerUp;
        switch (choice)
        {
            case 0:
                Debug.Log("Recharge Rate");
                powerUp = Instantiate(ammoRechargePrefab,spawnPos.transform.position,Quaternion.identity);
                break;
            case 1:
                Debug.Log("Capacity");
                powerUp = Instantiate(ammoCapPrefab, spawnPos.transform.position, Quaternion.identity);
                break;
            case 2:
                Debug.Log("Health");
                powerUp = Instantiate(healthPowerUpPrefab, spawnPos.transform.position, Quaternion.identity);
                break;
        }
    }


    //increments round
    IEnumerator increaseRound() {
        //spawn a random power up, recharge rate, capacity or health
        spawnPowerUp();

        yield return new WaitForSeconds(5f);
        roundCount++;//internal round counter
        if (roundCount > PlayerPrefs.GetFloat("BestScore", 0)) {
            PlayerPrefs.SetFloat("BestScore", roundCount);
        }
        player.setChargesForSeiz(roundCount);
        sceneUI.incrementRoundCounter(roundCount);//changes the UI, references UIScript
        if (roundCount == 2) {
            environmentScript.BeginEnvironmentAttacks();
        }

        //set difficulty increased, made at 3, 5, 7.
        if (roundCount == 3) {
            environmentScript.incrementAttackNumber(2);
            internalNumOfSpawn += 1;
            internalTimeGapMin -= 1;
        }

        if (roundCount == 5) {
            environmentScript.incrementAttackNumber(2);
            internalNumOfSpawn += 1;
            internalTimeGapMin -= 1;
            internalTimeGapMax -= 1;
        }
        

        if (roundCount == 7) {
            environmentScript.incrementAttackNumber(2);
            internalNumOfSpawn += 1;
            internalAddAmount = 2;
            internalTimeGapMin -= 2;
            internalTimeGapMax -= 2;
        }

        //adds the set number of waves to the next level
        internalWaveCount += internalAddAmount;

        //waits 5s for the user to 'clear' the scene a bit then starts the next wave.
        yield return new WaitForSeconds(7f);
        StartCoroutine(SpawnBasicAsteroid(internalWaveCount,internalNumOfSpawn,internalTimeGapMin,internalTimeGapMax));
    }
}
