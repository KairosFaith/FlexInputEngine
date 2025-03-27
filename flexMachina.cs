using System;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
//Must name "Gamepad" as control scheme in the InputActionAsset
//System is designed for Gamepad only
/// <summary>
/// Each player object must implement this interface
/// </summary>
public interface fPlayerObject
{
    public Type InputInterface { get; }
    ExPlayerProfile Profile { get; set; }
    void OnBind();
    //void OnUnBind();
}
public abstract class fProfile
{
    public Gamepad GamepadDevice;
    public fPlayerObject BoundObject;
    InputUser _User;
    InputActionMap _CurrentActionMap;
    Action _ResetActions;
    InputActionAsset _Asset;
    public fProfile(Gamepad device, InputActionAsset asset)
    {
        _User = InputUser.PerformPairingWithDevice(device);
        GamepadDevice = device;
        _Asset = UnityEngine.Object.Instantiate(asset);
    }
    public void EnableInput(bool active)
    {
        if (active)
            _CurrentActionMap.Enable();
        else
            _CurrentActionMap.Disable();
    }
    public void UnBindObject()
    {
        EnableInput(false);
        _ResetActions?.Invoke();
        _ResetActions = null;
        BoundObject.Profile = null;
        //BoundObject.OnUnBind();
        BoundObject = null;
    }
    public void BindObject<T>(T playerObject) where T : fPlayerObject
    {
        if (BoundObject != null)
            UnBindObject();
        BoundObject = playerObject;
        BoundObject.Profile = (ExPlayerProfile)this;
        Type type = playerObject.InputInterface;
        string actionMapName = type.Name[1..^7];//$"I{actionMapName}Action"
        _CurrentActionMap = _Asset.FindActionMap(actionMapName);
        _User.AssociateActionsWithUser(_CurrentActionMap);//will internally replace existing
        _User.ActivateControlScheme(nameof(Gamepad));
        MethodInfo[] methods = type.GetMethods();
        foreach (MethodInfo mi in methods)
        {
            string actionName = mi.Name[2..];//$"On{actionName}"
            InputAction action = _CurrentActionMap.FindAction(actionName);
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
            };
        }
        EnableInput(true);
        BoundObject.OnBind();
    }
}