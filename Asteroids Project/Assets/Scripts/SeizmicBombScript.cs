using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class SeizmicBombScript : MonoBehaviour
{
    

    //internal fuse count and internal spawning vector3
    public float fuse;
    private Vector3 targetArea;

    //variables for components and prefabs
    public ParticleSystem p;
    public GameObject bombPart;
    public GameObject ringPart;

    

    //uses two Coroutines to start the shooting process and destroy the prefab after a given time.
    public void shoot() {
        StartCoroutine(lerpOver());
        StartCoroutine(destroyObject());
    }

    //swaps which part of the bomb is visible
    private void explodeRing() {
        bombPart.gameObject.SetActive(false);
        ringPart.gameObject.SetActive(true);
    }



    //gets the position of the mouse and saves it globally.
    IEnumerator lerpOver()
    {
        targetArea = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//converts from screen to world point
        targetArea.z = 0;//resets z axis
        this.transform.position = targetArea;//places the prefab on this location
        StartCoroutine(playFuse(fuse));//starts the fuse routine

        yield return null;
    }


    //waits for a set time before continuing to the next step
    IEnumerator playFuse(float fuseTime) {
        yield return new WaitForSeconds(fuseTime);
        StartCoroutine(explode());
    }


    //plays the initial particle explosion, waits for it to finish then calls the next step.
    IEnumerator explode()
    {
        p.Play();//particle system gets played.
        yield return new WaitForSeconds(1.2f);
        explodeRing();
    }
    

    //waits a set time then destroys the game object.
    IEnumerator destroyObject() {
        yield return new WaitForSeconds(3.6f + fuse);
        Destroy(this.gameObject);
    }
}
