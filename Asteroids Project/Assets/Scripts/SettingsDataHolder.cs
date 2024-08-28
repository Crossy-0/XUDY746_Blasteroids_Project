using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class SettingsDataHolder : MonoBehaviour
{
    //held on each control object

    //UI CONTROLLER
    [SerializeField]
    private int settingID;
    private SettingsScript sScript;

    private void Awake()
    {
        sScript = FindObjectOfType<SettingsScript>();//getting the settings script on each component
    }

    public void ActiveKeyBind()
    {
        sScript.ChangeKeyBinding(this.gameObject, settingID);
    }

}
