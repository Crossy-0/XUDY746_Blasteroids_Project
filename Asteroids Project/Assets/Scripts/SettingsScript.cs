using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class SettingsScript : MonoBehaviour
{
    
    //settings cascade object
    public GameInputSettings InputSettings;
    private InputAction move;
    private InputAction forward;
    private InputAction backward;
    private InputAction boost;
    private InputAction primFire;
    private InputAction secondFire;

    private SettingsDataHolder currentChangeBox = null;
    private int settingID= 0;


    private bool isListening = false;

    private void Awake()
    {
        InputSettings = new GameInputSettings();
    }

    

    private void OnEnable()
    {
        
        forward = InputSettings.Player.Forward;
        forward.Enable();
        backward = InputSettings.Player.Backwards;
        backward.Enable();
        boost = InputSettings.Player.Boost;
        boost.Enable();
        primFire = InputSettings.Player.PrimaryFire;
        primFire.Enable();
        secondFire = InputSettings.Player.SecondaryFire;
        secondFire.Enable();
    }

    private void OnDisable()
    {
        forward.Disable();
        backward.Disable();
        boost.Disable();
        primFire.Disable();
        secondFire.Disable();
    }

   


    private const string
        Select = "Select a keybind to change...",
        Change = "Press a key to change keybind!";

    
    [SerializeField]
    private Text tooltipText;


    private void Start()
    {
        tooltipText.text = Select;
    }


    private void OnGUI()
    {
        if (isListening) {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
                KeyCode keyCode = Event.current.keyCode;
                //checks on input
                isListening = false;
                tooltipText.text = Select;
                if (keyCode != KeyCode.Escape)
                {
                    SetNewKey(keyCode, settingID);
                }
                else {
                    Debug.Log("Escape Pressed... Cancelled");
                }
            }
        }
    }

    public void ChangeKeyBinding(GameObject guiButton, int sID) {
        tooltipText.text = Change;
        currentChangeBox = guiButton.GetComponent<SettingsDataHolder>();
        settingID = sID;
        isListening = true;
    }

    private void SetNewKey(KeyCode newKey, int id) {
        if (id != 0 && currentChangeBox != null) {
            Debug.Log("Set To: "+ newKey);
            //forwards
            if (id == 1)
            {
                move.ChangeBinding(0);
            }//backwards
            else if (id == 2) {

            }//boost
            else if (id == 3)
            {

            }//prim fire
            else if (id == 4)
            {

            }//second fire
            else if (id == 5)
            {

            }



        }
    }

    

    



    //check all inputs have a key attatched
    private void CheckControlsOnClose() {

    }



}
