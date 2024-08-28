using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class OutOfBoundsScript : MonoBehaviour
{
    /*
     * NOTE:
     * This scripts entire purpose is to ensure that any object that leaves the play field is destroyed.
     * this was created with the intention to stop any rogue gameObjects from slowing the game down.
     */

    //if out of bounds, destroy the object.
    private void OnCollisionExit(Collision collision)
    {
        Destroy(collision.gameObject);
    }

}
