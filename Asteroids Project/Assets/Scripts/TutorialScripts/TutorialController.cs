using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class TutorialController : MonoBehaviour
{
    public void Continue() {
        PlayerPrefs.SetInt("Played", 1);
        SceneManager.LoadScene(2);//Loads the Game Scene
    }
}
