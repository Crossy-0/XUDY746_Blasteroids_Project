using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class EnvrionmentController : MonoBehaviour
{
    //pellet prefabs
    [SerializeField]
    private GameObject pushPelletPrefab;
    [SerializeField]
    private GameObject pullPelletPrefab;

    //spawns for pellets, and time management for it
    [SerializeField]
    private GameObject[] spawns;
    [SerializeField]
    [Range(0f, 10f)]
    private float timeGapBetweenStart, timeGapBetweenEnd, roundBeginWaitStart, roundBeginWaitEnd;


    [SerializeField]
    private Transform minBeamPos, maxBeamPos;
    [SerializeField]
    private GameObject beamPrefab;


    public int astLimit = 3;

    //private variable that controls when to spawn environmental affects
    private bool startEvents = false;
    private int attacks = 2;
    private bool roundEnd = false;
    private bool midTrigger = false;

    //sfx references
    public AudioSource beamSFXSource;
    public AudioClip beamSFX;

    
    


    public void Update()
    {
        if (startEvents) {
            if (!midTrigger) {
                midTrigger = true;
                StartCoroutine(SpawnPellet());
            }
                
        }
    }


    public void SetRoundEnd() {
        roundEnd = true;
    }

    public void SetRoundStart() {
        roundEnd = false;
    }

    public void BeginEnvironmentAttacks() {
        startEvents = true;
    }

    public void incrementAttackNumber(int n) {
        attacks += n;
    }


    void CheckFunction() {
        int max = 0;
        GameObject bestSpawn = null;
        //this is the finite state checker
        for (int i = 0; i < spawns.Length; i++) {
            GameObject g = spawns[i];
            AstCount aC = g.GetComponent<AstCount>();
            if (aC.count > max) {
                max = aC.count;
                bestSpawn = spawns[i];
            }
        }

        //if no asteroids
        if (bestSpawn == null)
        {
            float distanceBetween = 0;
            float pos1 = 0;
            float pos2 = 0;
            //1.1 being the distance between 2 beams. beam is 0.6 in width
            while (distanceBetween < 1.1) {
                pos1 = Random.Range(minBeamPos.transform.position.y, maxBeamPos.transform.position.y);
                pos2 = Random.Range(minBeamPos.transform.position.y, maxBeamPos.transform.position.y);
                distanceBetween = pos1 - pos2;
                distanceBetween = Mathf.Abs(distanceBetween);
            }

            Vector3 v1 = new Vector3(0,pos1,0);
            Vector3 v2 = new Vector3(0, pos2, 0);
            Instantiate(beamPrefab,v1,Quaternion.identity);
            Instantiate(beamPrefab, v2, Quaternion.identity);
            PlayBeamSFX();


        }
        else if (max < astLimit)
        {
            float pos = Random.Range(minBeamPos.transform.position.y, maxBeamPos.transform.position.y);
            Vector3 v1 = new Vector3(0, pos, 0);
            Instantiate(beamPrefab, v1, Quaternion.identity);
            PlayBeamSFX();
            Instantiate(pullPelletPrefab, bestSpawn.transform.position, Quaternion.identity);
        }
        else {
            float pos = Random.Range(minBeamPos.transform.position.y, maxBeamPos.transform.position.y);
            Vector3 v1 = new Vector3(0, pos, 0);
            Instantiate(beamPrefab, v1, Quaternion.identity);
            PlayBeamSFX();
            Instantiate(pushPelletPrefab, bestSpawn.transform.position, Quaternion.identity);
        }




    }

    public void PlayBeamSFX() {
        beamSFXSource.PlayOneShot(beamSFX);
    }

    IEnumerator SpawnPellet() {
        int attacksRemaining = attacks;
        yield return new WaitForSeconds(Random.Range(roundBeginWaitStart,roundBeginWaitEnd));
        while (!roundEnd && attacksRemaining > 0) {
            //call check function to spawn using FSM. if more than certain amount do the pellet, else do the beam. also if pellet check which one has most things in. if extra time make it so if the player consistantly does it faster than it can use attacks, spawn multiple at once.
            CheckFunction();
            yield return new WaitForSeconds(Random.Range(timeGapBetweenStart,timeGapBetweenEnd));
            attacksRemaining -= 1;
        }
        midTrigger = false;
    }



}
