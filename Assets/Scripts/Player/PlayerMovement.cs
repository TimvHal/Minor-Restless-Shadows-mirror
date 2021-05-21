using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator _anim;
    public UserControls _userControls;
    public Vector3 positionSnapshot;
    public Rigidbody2D _rb;
    public Animator gunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _userControls = GameObject.Find("Main Camera").GetComponent<UserControls>();
        positionSnapshot = transform.position;
        SoundManager.PlaySound(SoundManager.Sound.OverWorldBase, GameAssets.i.MusicVolume, true);
    }

    // Update is called once per frame
    void Update()
    {
        Countdown();
        //Check whether the player is walking or not.
        bool isWalking = _userControls.movementStick.touchStart;

        //Update value for isWalking in the Animator.
        _anim.SetBool("isWalking", isWalking);
        
        //Get values X and Y depending on the joystick. (Aiming has a higher priority.)
        float inputX;
        float inputY;
        
        if (_userControls.aimingStick.touchStart)
        {
            inputX = _userControls.aimingStick.vector.x;
            inputY = _userControls.aimingStick.vector.y;
            UpdateSprite(inputX, inputY);
        }
        else if(_userControls.movementStick.touchStart)
        {
            inputX = _userControls.movementStick.vector.x;
            inputY = _userControls.movementStick.vector.y;
            UpdateSprite(inputX, inputY);
        }
        
        //Move Pete if he is walking.
        if (isWalking)
        {
            //Move character based on direction.
            MoveCharacter(_userControls.movementStick.vector);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    void MoveCharacter(Vector3 direction)
    {
        _rb.velocity = direction * BuffManager.GetBuff(BuffManager.BuffType.movement).currentValue;
    }

    void UpdateSprite(float inputX, float inputY)
    {
        _anim.SetFloat("x", inputX);
        _anim.SetFloat("y", inputY);
    }

    private float targetTime = 3f;
    private void Countdown()
    {
        targetTime -= Time.deltaTime;
 
        if (targetTime <= 0.0f)
        {
            targetTime = 3f;
            savePosition();
        }
    }
    void savePosition()
    {
        positionSnapshot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void RegisterEndAnimation()
    {
        gunAnimator.Play(0, -1, 0);
    }
}
