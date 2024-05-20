using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using System;
public class FlexInputModule : BaseInputModule, fPlayerObject, NewControls.IMenuActions
{
    public override void Process()
    {
        //what does this even do?????
    }
    Selectable _SelectFirst;
    Transform _Root;
    Selectable _CurrentSelected;
    public InputDevice Device { get; set; }
    public fProfile Profile { get; set; }
    public Type InputInterface => typeof(NewControls.IMenuActions);
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Profile.CurrentActionMap.Disable();
    }
    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        Selectable s = _CurrentSelected.FindSelectable(move);
        //if(s != null)
            SelectNext(s);
    }
    public void SelectNext(Selectable s)
    {
        if (s != null && s.transform.IsChildOf(_Root))
        {
            eventSystem.SetSelectedGameObject(s.gameObject);
            _CurrentSelected = s;
        }
    }
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (_CurrentSelected != null)
        {
            ExecuteEvents.Execute(_CurrentSelected.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }
    }
    public void OnBind()
    {
        if (eventSystem is MultiplayerEventSystem m)
        {
            _Root = m.playerRoot.transform;
            _SelectFirst = m.firstSelectedGameObject.GetComponent<Selectable>();
        }
        _Root.gameObject.SetActive(true);
        SelectNext(_SelectFirst);
    }
}