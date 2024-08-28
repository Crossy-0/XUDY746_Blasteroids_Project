using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class uiColliderScripts : MonoBehaviour
{
    /*
     * NOTE:
     * This script is in charge of communicating with the UIScript to let it know which section of UI 
     * to hide based on which collider the player enters/exits
     */

    
    //public bool used to communicate with UIScript
    public bool playerIn = false;

    //Script reference
    public UIScript uiScript;



    
    private void Start()
    {
        //automatically grabbing the reference
        uiScript = FindObjectOfType<UIScript>();
    }


    //checks if a player enters. if so change the internal bool value to true.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") {
            playerIn = true;
        }
        //references UIScript to check both scripts to see which one changed.
        uiScript.checkUIColliders();
    }



    //checks if a player exits. if so change the internal bool value to false.
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "Player") {
            playerIn = false;
        }
        //calls the UIScript to check the colliders again
        uiScript.checkUIColliders();
    }
}
