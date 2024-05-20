using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
public class fQuickPlay : MonoBehaviour//what PlayerInput was supposed to be
{
    public InputActionAsset ControlAsset;
    public string ActionMap;
    InputActionMap _Map;
    void Start()
    {
        _Map = ControlAsset.FindActionMap(ActionMap);
        ReadOnlyArray<InputAction> actions = _Map.actions;
        foreach (InputAction a in actions)
        {
            string functionName = "On" + a.name;
            void d(InputAction.CallbackContext ctx) => SendMessage(functionName, ctx, SendMessageOptions.RequireReceiver);
            a.started += d;
            a.performed += d;
            a.canceled += d;
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
}