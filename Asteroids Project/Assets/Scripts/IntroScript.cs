using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class IntroScript : MonoBehaviour
{
    //simply changes the scene from the intro scene to the menu
    //NOTE: This method is triggered by animation events of the scene

    public void changeScene() {
        SceneManager.LoadScene("Menu");
    }
}
