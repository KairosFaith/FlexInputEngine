using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
//only use control functions here, no additional support for pointers or anything else
[RequireComponent(typeof(MultiplayerEventSystem))]
public class FlexUIModule : BaseInputModule
{
    public Transform RootTransform
    {
        get
        {
            if (eventSystem is MultiplayerEventSystem m)
                return m.playerRoot.transform;
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
        Debug.Log($"I wonder what {nameof(BaseInputModule)}.{nameof(BaseInputModule.Process)} does");
    }
}