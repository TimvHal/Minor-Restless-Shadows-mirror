using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour,IDragHandler, IPointerUpHandler
{
    public string type;
    
    public Transform outercircle;
    public Stick stick;
    
    private void Awake()
    {
        stick = new Stick(
            type, 
            outercircle.position, 
            transform.position, 
            transform, 
            outercircle);;
    }

    private void FixedUpdate()
    {
        stick.pointA = stick.outerCircle.position;
        if (stick.touchStart)
        {
            stick.SaveDirectionInformation();
            stick.innerCircle.transform.position = new Vector2(stick.pointA.x + stick.vector.x,
                stick.pointA.y + stick.vector.y);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        stick.touchPos = eventData.position;
        stick.SaveDirectionInformation();
        stick.pointB = Camera.main.ScreenToWorldPoint(new Vector3(
            stick.touchPos.x, stick.touchPos.y,
            Camera.main.transform.position.z));
        stick.touchStart = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        stick.ResetSelf();
    }

}