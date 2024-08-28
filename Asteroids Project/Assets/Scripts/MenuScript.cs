using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class MenuScript : MonoBehaviour
{
    //Boolean check for first time play
    private bool firstPlay = true;

    //gets the fading black screen image
    public Image fader;

    //components for exiting game
    public Button yes;
    public Button no;
    public Text youSure;

    //basic menu components 
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject helpButton;
    [SerializeField] private GameObject exitButton;

    //settings menu components
    [SerializeField] private GameObject settings_BackButton;
    [SerializeField] private GameObject settings_AudioOptions;
    [SerializeField] private GameObject settings_ControlsOptions;

    //settings menu cascading objects
    [SerializeField] private GameObject settings_AudioCascadePanel;
    [SerializeField] private GameObject settings_ControlsCascadePanel;


    //high score UI
    [SerializeField] private TMP_Text highScoreText;


    private void Start()
    {
        highScoreText.text = "Best Score: " + PlayerPrefs.GetFloat("BestScore", 0);

        int playedInt = PlayerPrefs.GetInt("Played", 0);//0 -> False, 1 -> True
        if (playedInt != 0) { firstPlay = false; }
    }

    private void OnEnable()
    {
        Animator faderAnim = fader.GetComponent<Animator>();
        faderAnim.Play("FadeOutMenu");
    }

   





    //method to display 'are you sure' UI when 'exit' clicked
    public void areYouSureActive()
    {
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        youSure.gameObject.SetActive(true);
    }

    //deactivates the 'are you sure' UI components
    public void areYouSureDeactive()
    {
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        youSure.gameObject.SetActive(false);
    }

    //quits the application
    public void exitGame()
    {
        Application.Quit();
    }


    //deactivates the fading screen (NOTE: triggered by animation events)
    public void disabledFader() {
        fader.gameObject.SetActive(false);
    }

    //loads the game scene
    public void PlayGame() {
        if (firstPlay)
        {
            SceneManager.LoadScene(3);
        }
        else {
            SceneManager.LoadScene("PlayScene");
        }
    }

    //disable the four main menu components and activate the 3 side parts
    public void OpenSettings() {
        //hide menu options
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        helpButton.SetActive(false);
        exitButton.SetActive(false);
        //hide high score
        highScoreText.gameObject.SetActive(false);

        //display settings options
        settings_BackButton.SetActive(true);
        settings_AudioOptions.SetActive(true);
        settings_ControlsOptions.SetActive(true);

        //hide the cascading options menu's
        HideSettingsOptionsCascades();

    }

    //save options here----------------------------------
    public void CloseSettings() {
        //hide the cascading options menu's
        HideSettingsOptionsCascades();

        //hide settings options
        settings_BackButton.SetActive(false);
        settings_AudioOptions.SetActive(false);
        settings_ControlsOptions.SetActive(false);

        //show menu options
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        //helpButton.SetActive(true);
        exitButton.SetActive(true);
        //un-hide high score
        highScoreText.gameObject.SetActive(true);
    }

    //hides the pop out windows for each settings option so they dont overlap on load
    void HideSettingsOptionsCascades() {
        settings_AudioCascadePanel.SetActive(false);
        settings_ControlsCascadePanel.SetActive(false);
    }


    //settings cascade selection
    public void DisplayAudioSettings() {
        HideSettingsOptionsCascades();
        settings_AudioCascadePanel.SetActive(true);
    }

    public void DisplayControlsSettings()
    {
        HideSettingsOptionsCascades();
        settings_ControlsCascadePanel.SetActive(true);
    }

}
