using System;
using System.Collections;
using System.Collections.Generic;
using Room;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Object = System.Object;

public class Flashlight : MonoBehaviour
{
    public UserControls _userControls;

    private static Light2D _flashlight;
    public Sprite[] TogglableSprites;
    private static bool _active;
    public bool checkActive = false;
    public Image spr;

    // Start is called before the first frame update
    void Start()
    {
        _userControls = GameObject.Find("Main Camera").GetComponent<UserControls>();
        _flashlight = GetComponent<Light2D>();
        _active = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkActive = _active;
        spr.sprite = TogglableSprites[_active ? 1 : 0];
        UpdateRotation();
    }

    void UpdateRotation()
    {
        float direction = _userControls.aimingStick.touchStart ? 
            _userControls.aimingStick.angle : 
                _userControls.movementStick.touchStart ? 
                    _userControls.movementStick.angle : 
                    _userControls.aimingStick.angle;
        transform.rotation = Quaternion.Euler(0, 0, direction - 90);
    }

    static void ToggleLight()
    {
        _active = !_active;
    }

    public static bool IsActive()
    {
        return _active;
    }

    public static void OnToggleFlashlight()
    {
        ToggleLight();
        if (IsActive())
        {
            _flashlight.intensity = 1;
            return;
        }
        _flashlight.intensity = 0;
    }

    public static void DisableFlashlight()
    {
        _flashlight.intensity = 0;
        _active = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && IsActive())
        {
            LightRegistration lr = GetEnemyControl(other.gameObject);
            lr.setLIght(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && IsActive())
        {
            LightRegistration lr = GetEnemyControl(other.gameObject);
            lr.setLIght(false);
        }
    }

    private LightRegistration GetEnemyControl(GameObject other)
    {
        LightRegistration lr = other.GetComponent<LightRegistration>();
        return lr;
    }
}
