using System;
using System.Collections;
using System.Collections.Generic;
using Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class SnowballStateMachine : MonoBehaviour, LightRegistration
{
    public enum State
    {
        Hiding,
        Wiggle
    }
    public State state = State.Hiding;
    public GameObject dummy;

    public Animator _animator;

    public Vector2[] SpawnPos;
    public Transform realSnowBall;
    private SnowballAttack attackScript;
    public DoorDeletion door;
    public Transform roomPosition;
    public int attackQuantity = 1;
    public List<GameObject> dummies = new List<GameObject>();
    private HealthController healthController;
    private bool InLight;

    private bool initiation;
    // Start is called before the first frame update
    void Start()
    {
        healthController = GetComponent<HealthController>();
        door = roomPosition.gameObject.GetComponent<DoorTile>().optionalDoor;
        foreach (var boxPosition in SpawnPos)
        {
            var clone = Instantiate(dummy);
            clone.transform.parent = transform.parent;
            clone.transform.localPosition = boxPosition;
            dummies.Add(clone);
        }
        attackScript = GetComponent<SnowballAttack>();
        SoundManager.StopSound(SoundManager.Sound.OverWorldBase);
        SoundManager.PlaySound(SoundManager.Sound.SnowballTheme, GameAssets.i.MusicVolume, true);
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1 / attackQuantity);
        if (initiation)
        {
            if (state == State.Hiding)
            {
                _animator.SetBool("Hiding", true);
                _animator.SetBool("Wiggle", false);
            }

            if (state == State.Wiggle)
            {
                _animator.SetBool("Wiggle", true);
                _animator.SetBool("Hiding", false);
            }
        }
        else
        {
            initiation = door.gameObject.GetComponent<DoorAnimator>().ctg.playerIsInside;
        }

        if (InLight && healthController.stage < 2)
        {
            _animator.speed = 0.25f;
        }
        else
        {
            _animator.speed = 0.5f;
        }

        StartCoroutine(Attack());
    }

    public void WiggleBehaviour()
    {
        if (healthController.died) return;
        GetComponent<SpriteRenderer>().color = Color.white;
        SoundManager.PlaySound(SoundManager.Sound.SnowballFireball, 0.8f);
        for (int i = 0; i < attackQuantity; i++)
        {
            var result = attackScript.GetAttack();
            result();
        }
        state = State.Hiding;
    }
    
    public void HidingBehaviour()
    {
        if (healthController.died || state == State.Wiggle) return;
        realSnowBall.localPosition = SpawnPos[new Random().Next(4)];
    }

    public void HidingReset()
    {
        if (initiation)
            state = State.Wiggle;
    }

    public void AttackCharge()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void setLIght(bool toggle)
    {
        InLight = toggle;
    }
}
