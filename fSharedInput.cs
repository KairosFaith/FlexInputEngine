using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>For single player game or shared player object, supports all <see cref="InputDevice"/> bindings in <see cref="InputActionAsset"/></summary>
public class fSharedInput : MonoBehaviour
{
    public InputActionAsset ControlAsset;
    public string ActionMap;
    InputActionMap _Map;
    Action _ResetActions;
    void Start()
    {
        _Map = ControlAsset.FindActionMap(ActionMap);
        foreach (InputAction a in _Map.actions)
        {
            string functionName = "On" + a.name;
            void d(InputAction.CallbackContext ctx) => SendMessage(functionName, ctx, SendMessageOptions.RequireReceiver);
            a.started += d;
            a.performed += d;
            a.canceled += d;
            _ResetActions += () =>
            {
                a.started -= d;
                a.performed -= d;
                a.canceled -= d;
            };
        }
        EnableInput(true);
    }
    public void EnableInput(bool active)
    {
        if (active)
            _Map.Enable();
        else
            _Map.Disable();
    }
    private void OnDestroy()
    {
        ClearOut();
    }
    public void ClearOut()
    {
        _ResetActions?.Invoke();
        _ResetActions = null;
    }
}