using System;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
public abstract class fProfile
{
    public InputDevice GamepadDevice;
    public InputUser User;
    public InputActionMap CurrentActionMap;
    Action _ResetActions;
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
    public void EnableInput(bool active)
    {
        if (active)
            CurrentActionMap.Enable();
        else
            CurrentActionMap.Disable();
    }
    public void UnBindObject()
    {
        EnableInput(false);
        _ResetActions?.Invoke();
        _ResetActions = null;
        BoundObject.Profile = null;
        BoundObject = null;
    }
    public void BindObject<T>(T playerObject) where T : fPlayerObject
    {
        if (BoundObject != null)
            UnBindObject();
        BoundObject = playerObject;
        BoundObject.Profile = (ExPlayerProfile)this;
        Type type = playerObject.InputInterface;
        string actionMapName = type.Name[1..^7];//$"I{actionMapName}Action";
        CurrentActionMap = _Asset.FindActionMap(actionMapName);
        User.AssociateActionsWithUser(CurrentActionMap);//will internally replace existing
        User.ActivateControlScheme(_ControlSchemeDevice);
        MethodInfo[] methods = type.GetMethods();
        foreach (MethodInfo mi in methods)
        {
            string actionName = mi.Name[2..];//remove the "On"
            InputAction action = CurrentActionMap.FindAction(actionName);
            Action<InputAction.CallbackContext> d = 
                (Action<InputAction.CallbackContext>)mi.CreateDelegate(typeof(Action<InputAction.CallbackContext>), playerObject);
            action.started += d;
            action.performed += d;
            action.canceled += d;
            _ResetActions += () =>
            {
                action.started -= d;
                action.performed -= d;
                action.canceled -= d;
                //playerObject.OnUnBind();
            };
        }
        EnableInput(true);
        BoundObject.OnBind();
    }
}
public interface fPlayerObject
{
    public Type InputInterface { get; }
    ExPlayerProfile Profile { get; set; }
    void OnBind();
    //void OnUnBind();
}