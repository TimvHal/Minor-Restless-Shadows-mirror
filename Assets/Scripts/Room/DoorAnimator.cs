using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    private bool playerIsInside;
    public CheckTriggerGround ctg;
    public bool overide;
    public EnemySpawnController dc;
    public EnemySpawnController forward;
    private bool forwardDone = true;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.layer != 13) return;
        if (!forwardDone) return;
        if (ctg.playerIsInside && !overide)
        {
            anim.SetBool("Spawn", true);
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            anim.SetBool("Spawn", false);
        } 
    }

    private void Update()
    {
        if (ctg == null) return;
        if (ctg.playerIsInside && !overide)
        {
            anim.SetBool("Spawn", true);
            GetComponent<BoxCollider2D>().enabled = true;
            dc.enemiesSpawned = true;
            dc.SpawnEnemies();
        }
        else
        {
            anim.SetBool("Spawn", false);
        }

        if (dc.done && dc._currentEnemies == 0) overide = true;
        if (forward != null)
        {
            if (forward.done && forward._currentEnemies == 0)
            {
                forwardDone = true;
            }
            else
            {
                anim.SetBool("Spawn", true);
                GetComponent<BoxCollider2D>().enabled = true;
                forwardDone = false;
            }
        }
    }

    public void ForceDespawn()
    {
        anim.SetBool("Spawn", false);
    }

    public void DisableDoor()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
