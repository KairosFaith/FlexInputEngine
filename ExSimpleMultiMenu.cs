using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ExSimpleMultiMenu : MonoBehaviour
{
    public bool AllowNewPlayers;
    public List<fPlayerObject> PlayerObjects;
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
        if (profile == null)
            return;
        fPlayerObject playerObject = PlayerObjects[key];
        profile.BindObject(playerObject);
    }
    //allow new players to join
    void onDeviceChange(InputDevice device, InputDeviceChange change)
    {
            switch (change)
            {
                case InputDeviceChange.Added:
                if (device is Gamepad gamepad)
                    AddProfile(gamepad);
                break;
            }
    }
}