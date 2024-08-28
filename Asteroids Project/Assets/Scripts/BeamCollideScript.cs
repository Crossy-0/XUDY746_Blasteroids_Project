using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class BeamCollideScript : MonoBehaviour
{
    private bool isHitting = false;
    [SerializeField]
    private float waitTime = 0.5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            if (!isHitting) {
                isHitting = true;
                PlayerMovement pScript = collision.gameObject.GetComponent<PlayerMovement>();
                pScript.reduceHealth(10);
                StartCoroutine(waitForHit());
            }
        }
    }


    IEnumerator waitForHit() {
        yield return new WaitForSeconds(waitTime);
        isHitting = false;
    }

}
