using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
public class EventListener :EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate OnClick;
    public VoidDelegate OnSlideUp;
    static public EventListener GetListener(GameObject go) {
        EventListener listener= go.GetComponent<EventListener>();
        if (listener == null) listener = go.AddComponent<EventListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null) OnClick(gameObject);
    }
    public override void OnDrag(PointerEventData eventData)
    {
     //   Debug.Log(eventData.delta);
        if (eventData.delta.y > 0.5)
        {
            if (OnSlideUp != null) OnSlideUp(gameObject);
        }
    }
}
