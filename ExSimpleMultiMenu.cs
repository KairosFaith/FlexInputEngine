using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ExSimpleMultiMenu : MonoBehaviour
{
    public bool AllowNewPlayers;
    public List<FlexInputModule> UIModules;
    void Start()
    {
        //clear player profiles
        FlexProfileManager.Instance.ClearProfiles();
        //create player profiles based on available gamepads
        var gamepads = Gamepad.all;
        foreach (Gamepad pad in gamepads)
            AddProfile(pad);
        InputSystem.onDeviceChange += onDeviceChange;
    }
    void AddProfile(Gamepad pad)
    {
        //register player profiles with PlayerProfileManager
        fProfile profile = FlexProfileManager.Instance.AddProfile(pad, out int key);
        //activate menu for each player profile
        FlexInputModule playerObject = UIModules[key];
        profile.BindObject(playerObject);
    }
    //allow new players to join
    void onDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    AddProfile(gamepad);
                    break;
            }
        }
    }
}