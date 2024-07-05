using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
//using System;
public class FlexUIModule : BaseInputModule
{
    public override void Process()
    {
        //what does this even do?????

        //TODO this function is supposed to process their pointer stuff
    }
    Transform _Root;
    Selectable _CurrentSelected;
    public void Navigate(Vector2 move)
    {
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
    public void Submit()
    {
        //if (_CurrentSelected != null)
            ExecuteEvents.Execute(_CurrentSelected.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
    }
    public void OnBind()
    {
        Selectable _SelectFirst = null;
        if (eventSystem is MultiplayerEventSystem m)
        {
            _Root = m.playerRoot.transform;
            _SelectFirst = m.firstSelectedGameObject.GetComponent<Selectable>();
        }
        else
            throw new System.Exception("Need" + nameof(MultiplayerEventSystem));
        _Root.gameObject.SetActive(true);
        SelectNext(_SelectFirst);
    }
}