using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class InputGetter : MonoBehaviour
{
    //gets the player prefs input rebinds from persistent storage 

    [SerializeField]private InputActionAsset actions;


    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public InputActionAsset GetInputSettings() {

        return actions;
    }


}
