using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>For using Mouse Clicks in conjunction with <see cref="FlexMultiUIModule"/></summary>
//Must Enable both Old and New Input System
//UI does not stay selected
//TODO Does NOT support dragging YET
public class FlexCursorInputModule : BaseInputModule
{
    public GraphicRaycaster Canvas;
    Vector2 _PrevCursorPosition;
    List<RaycastResult> _PrevRaycastResults = new List<RaycastResult>();
    PointerEventData _PointerEventData;
    GameObject _ClickedDownObject;
    //bool _MouseDown;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ClickDown();
        else if (Input.GetMouseButtonUp(0))
            ClickUp();
    }
    public override void Process()
    {
        _PointerEventData ??= new PointerEventData(eventSystem);
        Vector2 currentCursorPosition = Input.mousePosition;
        if(currentCursorPosition != _PrevCursorPosition)
            OnMouseMove(currentCursorPosition);
        _PrevCursorPosition = currentCursorPosition;
    }
    void OnMouseMove(Vector2 currentCursorPosition)
    {
        _PointerEventData.position = currentCursorPosition;
        List<RaycastResult> currentRaycastResults = new List<RaycastResult>();
        Canvas.Raycast(_PointerEventData, currentRaycastResults);
        foreach (RaycastResult r in currentRaycastResults)
            if (_PrevRaycastResults.Contains(r))
                _PrevRaycastResults.Remove(r);
            else
                ExecuteEvents.Execute(r.gameObject, _PointerEventData, ExecuteEvents.pointerEnterHandler);
        foreach (RaycastResult r in _PrevRaycastResults)
            ExecuteEvents.Execute(r.gameObject, _PointerEventData, ExecuteEvents.pointerExitHandler);
        _PrevRaycastResults = currentRaycastResults;
        //if(_MouseDown)
        //    ClickDrag();
    }
    void ClickDown()
    {
        if(_PrevRaycastResults.Count > 0)
        {
            _ClickedDownObject = _PrevRaycastResults[0].gameObject;
            ExecuteEvents.Execute(_ClickedDownObject, _PointerEventData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(_ClickedDownObject, _PointerEventData, ExecuteEvents.deselectHandler);
        }
    }
    void ClickUp()
    {
        if (_PrevRaycastResults.Count > 0)
        {
            GameObject clickedUpObject = _PrevRaycastResults[0].gameObject;
            if (_ClickedDownObject == clickedUpObject)
                ExecuteEvents.Execute(clickedUpObject, _PointerEventData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(clickedUpObject, _PointerEventData, ExecuteEvents.pointerUpHandler);
        }
    }
    //void ClickDrag()
    //{
    //    if (prevRaycastResults.Count > 0)
    //    {
    //        GameObject pointerMoveObject = prevRaycastResults[0].gameObject;
    //        if (_ClickedDownObject == pointerMoveObject)
    //            ExecuteEvents.Execute(pointerMoveObject, pointerEventData, ExecuteEvents.dragHandler);
    //    }
    //}
}
