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
public class UIScript : MonoBehaviour
{
    //internal score counter
    private int currentScore = 0;

    //public gameObjects and scripts in the scene
    public GameObject pauseUI;
    public GameObject areYouSureUI;
    public PlayerMovement player;
    public uiColliderScripts u1;
    public uiColliderScripts u2;

    //all text components of the UI
    public Text score;
    public Text objective;
    public Text roundCount;
    public Text health;
    public Text gameOver;
    public Text bulletCount;
    public Image seizmicBullet1, seizmicBullet2, seizmicBullet3;

    public GameObject ContinueBtn, SettingsBtn, ExitBtn, SettingsContent,AudioContents;

    /*
     * public method to end the game
     * NOTE:
     * stopping the game using timeScale does not stop the update function and is not a good solution going forward.
     * it only works in this case because there isnt anything to do afterwards
     */
    public void endGame() {
        StartCoroutine(EndGame());
    }


    //update UI for health
    public void updateHealth(int healthValue) {
        health.text = "Health: "+healthValue+"%";
    }

    //update UI for ammo
    public void UpdateAmmoCounter(int ammo, int maxAmmo) {
        bulletCount.text = "Ammo: " +ammo + "/"+maxAmmo;
    }

    public void UpdateSeizmicAmmo(int ammo) {
        switch (ammo) {
            case 0:
                seizmicBullet1.gameObject.SetActive(false);
                seizmicBullet2.gameObject.SetActive(false);
                seizmicBullet3.gameObject.SetActive(false);
                break;
            case 1:
                seizmicBullet1.gameObject.SetActive(true);
                seizmicBullet2.gameObject.SetActive(false);
                seizmicBullet3.gameObject.SetActive(false);
                break;
            case 2:
                seizmicBullet1.gameObject.SetActive(true);
                seizmicBullet2.gameObject.SetActive(true);
                seizmicBullet3.gameObject.SetActive(false);
                break;
            case 3:
                seizmicBullet1.gameObject.SetActive(true);
                seizmicBullet2.gameObject.SetActive(true);
                seizmicBullet3.gameObject.SetActive(true);
                break;
        }
    }

    //update round UI
    public void incrementRoundCounter(int newRound) {
        roundCount.text = "Level: " +newRound;
    }

    //Pauses the game and activated the pause menu
    public void pauseGame() {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    //resumes the game, and removes the pause menu
    public void resumeGame() {
        pauseUI.SetActive(false);
        areYouSureUI.SetActive(false);
        player.paused = false;
        Time.timeScale = 1;
    }


    //activates 'are you sure' UI for pause menu
    public void areYouSureOn() {
        areYouSureUI.SetActive(true);
    }

    //deactivates 'are you sure' UI for pause menu
    public void areYouSureOff()
    {
        areYouSureUI.SetActive(false);
    }


    //exits the 'game' scene 
    public void exitToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);  //this feature isn't working yet due to the automated fading of screens so for now it just quits the game
        //Application.Quit();
    }


    //putting settings menu on
    public void OpenSettings()
    {
        //remove the buttons and add the settings UI
        ContinueBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        ExitBtn.SetActive(false);

        SettingsContent.SetActive(true);
        AudioContents.SetActive(false);

    }
     
    //set all UI back to normal
    public void ResetMenu() {
        ContinueBtn.SetActive(true);
        SettingsBtn.SetActive(true);
        ExitBtn.SetActive(true);

        SettingsContent.SetActive(false);
        AudioContents.SetActive(false);
    }

    public void OpenAudioSettings() {
        AudioContents.SetActive(true);
    }

    //Checks if the player enters the top or bottom quadrant of the scene, and disables the relevent UI to ensure visibility.
    public void checkUIColliders() {
        if (u1.playerIn)//the top collider
        {
            //hides the UI
            score.gameObject.SetActive(false);
            objective.gameObject.SetActive(false);
            roundCount.gameObject.SetActive(false);
        }
        else {
            //reveals the UI
            score.gameObject.SetActive(true);
            objective.gameObject.SetActive(true);
            roundCount.gameObject.SetActive(true);
        }

        if (u2.playerIn)//the bottom collider
        {
            //hides the UI
            health.gameObject.SetActive(false);
        }
        else {
            //reveals the UI
            health.gameObject.SetActive(true);
        }
    }

    //adds score to the UI using the integer passed in
    public void AddScore(int s) {
        string scoreGet = score.text;//gets the UI text

        //attempts to handle parsing the existing score UI, adding the score and updating UI
        try
        {
            string[] str = scoreGet.Split(':');
            currentScore = int.Parse(str[1].Trim());
            currentScore += s;
            score.text = ("Score: " + currentScore);
        }
        catch {
            Debug.Log("error!! not parsed");
            currentScore = 0;
            score.text = ("Score: " + currentScore);
        }
    }


    IEnumerator EndGame() {
        gameOver.gameObject.SetActive(true);
        player.paused = true;//references the player movement script to prevent movement
        yield return new WaitForSeconds(4);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
