using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//Must name "Gamepad" as control scheme in the InputActionAsset
//System is designed for Gamepad only
public class FlexProfileManager : MonoBehaviour
{
    public int PlayerCapacity = 2;
    public InputActionAsset InputAsset;
    public List<ExPlayerProfile> PlayerProfiles = new List<ExPlayerProfile>();
    public static FlexProfileManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public void ClearAllBindings()
    {
        foreach (var p in PlayerProfiles)
            p.UnBindObject();
    }
    public ExPlayerProfile AddProfile(Gamepad device, out int key)
    {
        ExPlayerProfile profile = null;
        foreach (var p in PlayerProfiles)
            if (p.GamepadDevice == device)
                profile = p;
        if (profile == null)
        {
            if (PlayerProfiles.Count < PlayerCapacity)
            {
                profile = new ExPlayerProfile(device, InputAsset);
                PlayerProfiles.Add(profile);
            }
            else
            {
                key = -1;
                return null;
            }
        }
        key = PlayerProfiles.IndexOf(profile);
        return profile;
    }
}
public class ExPlayerProfile : fProfile
{
    public string PlayerName;
    //Add any additional player specific data here
    public ExPlayerProfile(Gamepad device, InputActionAsset asset) : base(device, asset) { }
}