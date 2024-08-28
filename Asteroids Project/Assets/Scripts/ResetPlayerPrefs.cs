using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class ResetPlayerPrefs : MonoBehaviour
{
    [SerializeField] private Button editorButton;

    //only allows the button to reset playerpref data from the editor
    private void Awake()
    {
        if (Application.isEditor)
        {
            editorButton.gameObject.SetActive(true);
        }
        else {
            editorButton.gameObject.SetActive(false);
        }
    }

    public void RemoveData() {
        PlayerPrefs.DeleteAll();
    }
}
