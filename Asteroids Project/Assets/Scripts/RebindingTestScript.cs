using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class RebindingTestScript: MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private Text keybindSelectNotification;
    [SerializeField] private InputActionReference inputActionRef;
    [SerializeField] private TMP_Text buttonText;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private string select = "Select a keybind to change...";
    private string change = "Press a key to change keybind!";
    

    private void Start()
    {
        keybindSelectNotification.text = select;
        int bindingIndex = inputActionRef.action.GetBindingIndexForControl(inputActionRef.action.controls[0]);
        buttonText.text = InputControlPath.ToHumanReadableString(inputActionRef.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
    public void StartRebinding()
    {
        keybindSelectNotification.text = change;
        rebindingOperation = inputActionRef.action.PerformInteractiveRebinding()
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnMatchWaitForAnother(0.1f)
                    .OnCancel(operation => Cancelled())
                    .OnComplete(operation => RebindComplete())
                    .Start();
    }

    private void Cancelled() {
        keybindSelectNotification.text = select;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
    }


    private void RebindComplete()
    {
        keybindSelectNotification.text = select;
        int bindingIndex = inputActionRef.action.GetBindingIndexForControl(inputActionRef.action.controls[0]);
        buttonText.text = InputControlPath.ToHumanReadableString(inputActionRef.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperation.Dispose();
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        
    }

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds)) {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds",rebinds);
    }


}
