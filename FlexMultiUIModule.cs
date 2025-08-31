using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
///<summary>For UI Menu exclusive to each player, Gamepad only, use <see cref="FlexCursorInputModule"/> to add cursor support</summary>
[RequireComponent(typeof(MultiplayerEventSystem))]
public class FlexMultiUIModule : BaseInputModule
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
        if(_CurrentSelected is Slider slider)
            switch(slider.direction)
            {
                case Slider.Direction.LeftToRight:
                case Slider.Direction.RightToLeft:
                    //check if move is horizontal
                    if(Mathf.Abs(move.x) > Mathf.Abs(move.y))
                    {
                        slider.OnMove(new AxisEventData(eventSystem) { moveDir = move.x > 0 ? MoveDirection.Right : MoveDirection.Left });
                        return;
                    }
                    break;
                case Slider.Direction.BottomToTop:
                case Slider.Direction.TopToBottom:
                    //check if move is vertical
                    if(Mathf.Abs(move.y) > Mathf.Abs(move.x))
                    {
                        slider.OnMove(new AxisEventData(eventSystem) { moveDir = move.y > 0 ? MoveDirection.Up : MoveDirection.Down });
                        return;
                    }
                    break;
            }
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
        //TODO Debug.Log($"I wonder what {nameof(BaseInputModule)}.{nameof(BaseInputModule.Process)} is for");
    }
}