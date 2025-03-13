using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
//only use control functions here, no additional support for pointers or anything else
[RequireComponent(typeof(MultiplayerEventSystem))]
public class FlexUIModule : BaseInputModule
{
    Transform _Root;
    public Transform RootTransform
    {
        get
        {
            if(_Root!=null)
                return _Root;
            else if (eventSystem is MultiplayerEventSystem m)
            {
                _Root = m.playerRoot.transform;
                return _Root;
            }
            else
                throw new System.Exception("Need " + nameof(MultiplayerEventSystem));
        }
    }
    Selectable _CurrentSelected;
    public void Navigate(Vector2 move)
    {
        Selectable s = _CurrentSelected.FindSelectable(move);
        if(s != null)
            Select(s);
    }
    public void Submit(InputAction.CallbackContext c)
    {
        if(c.performed)
            ExecuteEvents.Execute(_CurrentSelected.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
    }
    public void Select(Selectable s)
    {
        if (s.transform.IsChildOf(RootTransform))
        {
            eventSystem.SetSelectedGameObject(s.gameObject);
            _CurrentSelected = s;
        }
    }
    public void SelectFirst()
    {
        Selectable _SelectFirst = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();
        Select(_SelectFirst);
    }
    public override void Process()
    {
        //what does this even do?????

        //TODO this function is supposed to process their pointer stuff
    }
}