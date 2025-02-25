using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FlexProfileManager : MonoBehaviour
{
    public int PlayerCapacity = 2;
    public InputActionAsset InputAsset;
    public List<ExPlayerProfile> PlayerProfiles = new List<ExPlayerProfile>();
    public static FlexProfileManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(Instance.gameObject.name + nameof(FlexProfileManager)+ " Already exists");
            Destroy(this);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public void ClearProfiles()
    {
        PlayerProfiles.Clear();
    }
    public ExPlayerProfile AddProfile(Gamepad device, out int key)
    {
        ExPlayerProfile profile = null;
        foreach (var p in PlayerProfiles)
            if (p.GamepadDevice == device)
                profile = p;
        if (profile == null)
        {
            if (PlayerProfiles.Count >= PlayerCapacity)
            {
                key = -1;
                return null;
            }
            else
            {
                profile = new ExPlayerProfile(device, InputAsset, nameof(Gamepad));
                PlayerProfiles.Add(profile);
            }
        }
        key = PlayerProfiles.IndexOf(profile);
        return profile;
    }
}
public class ExPlayerProfile : fProfile//Project specific, add any variables you need
{
    public string PlayerName;
    public ExPlayerProfile(InputDevice device, InputActionAsset asset, string controlSchemeDevice) 
        : base(device, asset, controlSchemeDevice) { }
}