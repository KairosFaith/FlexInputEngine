using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
public class fProfile
{
    public InputDevice GamepadDevice;
    public InputUser User;
    public InputActionMap CurrentActionMap;
    InputActionAsset _Asset;
    public fPlayerObject BoundObject;
    string _ControlSchemeDevice;
    public fProfile(InputDevice device, InputActionAsset asset, string controlSchemeDevice)
    {
        User = InputUser.PerformPairingWithDevice(device);
        GamepadDevice = device;
        _Asset = UnityEngine.Object.Instantiate(asset);
        _ControlSchemeDevice = controlSchemeDevice;
    }
    public void BindObject<T>(T playerObject) where T : fPlayerObject
    {
        if(BoundObject!=null)
        {
            BoundObject.Profile = null;
            BoundObject = null;
        }
        BoundObject = playerObject;
        BoundObject.Profile = this;
        CurrentActionMap = _Asset.FindActionMap(playerObject.ActionMapName);
        User.AssociateActionsWithUser(CurrentActionMap);//will internally replace existing
        User.ActivateControlScheme(_ControlSchemeDevice);
        Type type = playerObject.InputInterface;
        foreach (var mi in type.GetMethods())
        {
            string actionName = mi.Name.Substring(2);//remove the "On"
            InputAction action = CurrentActionMap.FindAction(actionName);
            if (action != null)
            {
                Action<InputAction.CallbackContext> d = (Action<InputAction.CallbackContext>)mi.CreateDelegate(typeof(Action<InputAction.CallbackContext>), playerObject);
                action.Reset();//clear the preexisting bindings
                action.started += d;
                action.performed += d;
                action.canceled += d;
            }
        }
        CurrentActionMap.Enable();
        BoundObject.OnBind();
    }
}
public interface fPlayerObject
{
    public string ActionMapName { get; }
    public Type InputInterface { get; }
    InputDevice Device { get; set; }
    fProfile Profile { get; set; }
    void OnBind();
}