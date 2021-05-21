using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDeletion : MonoBehaviour
{
    public int _enemyMax = 0;
    public bool final;

    public int _currentEnemy = 0;

    public bool deleted = false;
    
    private CountDown _deathTimer = new CountDown(1f);

    public DoorTile.Side direction;
    public Sprite spriteVert;
    public Sprite spriteHor;

    // Start is called before the first frame update
    void Start()
    {
        var anim = GetComponent<Animator>();
        if (direction == DoorTile.Side.east || direction == DoorTile.Side.west) anim.SetBool("Side", true);
        else
        {
            anim.SetBool("Front", true);
                var collider = GetComponent<BoxCollider2D>();
           collider.size = new Vector2(collider.size.x * 2, collider.size.y);
        }
        var spritemanager =
            GetComponent<SpriteRenderer>();
        switch (direction)
        {
            case DoorTile.Side.north:
                spritemanager.sprite = spriteVert;
                break;
            case DoorTile.Side.east :
                spritemanager.sprite = spriteHor;
                break;
            case DoorTile.Side.west:
                spritemanager.sprite = spriteHor;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                break;
            case DoorTile.Side.south:
                spritemanager.sprite = spriteVert;
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (_currentEnemy == 0 && other.gameObject.layer == 13) 
        {
            deleted = true;
        }
    }

}
