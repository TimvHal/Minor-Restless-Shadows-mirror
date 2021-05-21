using System;
using System.Collections;
using System.Collections.Generic;
using Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{

    public float _health;

    private bool damageTaken = false;

    public GameObject collider;
    
    public Text HealthRepresentation;
    public GameObject HealthIconRepresentaiton;

    private DamageRegistration _registration;

    private bool Godmode;
    // Start is called before the first frame update
    void Start()
    {
        Godmode = CheatManager.GetCheat(CheatManager.Cheat.God).active;
        _registration = collider.GetComponent<DamageRegistration>();
        _health = BuffManager.GetBuff(BuffManager.BuffType.health).currentValue;
    }
    
    private void Update()
    {
        if (_health <= 0)
        {
            GameState.Instance.SetEnd(true);
            SceneManager.LoadScene("Game Over");
        }
        if (_registration.colliding) Collide();
        collider.transform.position = transform.position;
        HealthIconRepresentaiton.GetComponent<Healthrepresentation>().DetermineHealth(_health);
    }

    private void Collide()
    {
        if (!damageTaken && !Godmode)
        {
            _health -= 1;
            ScoreUI.instance.UpdateValue(-1);
            damageTaken = true;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(resetDamageIndication());
        }
    }

    IEnumerator resetDamageIndication()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        damageTaken = false;
    }
}