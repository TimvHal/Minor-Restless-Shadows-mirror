using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Calculations;
using static CanvasPositioningExtensions;

public class MovementRegister : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    public GameObject player;
    private UserControls userControls;

    private PlayerGun playerGun;
    // Start is called before the first frame update
    void Start()
    {
        userControls = player.GetComponentInChildren<UserControls>();
        playerGun = player.GetComponentInChildren<PlayerGun>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        userControls.movementStick.touchStart = true;
        userControls.movementStick.pointB = eventData.position;
        userControls.movementStick.SaveDirectionInformation();
        if (!userControls.aimingStick.touchStart && userControls.movementStick.touchStart)
            userControls.aimingStick.angle = userControls.movementStick.angle;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        userControls.movementStick.touchStart = true;
        userControls.movementStick.pointA = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        userControls.movementStick.touchStart = false;
    }
}
