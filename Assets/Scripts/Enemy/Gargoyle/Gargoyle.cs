using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Gargoyle : MonoBehaviour
{
    public Transform player;
    private Animator _anim;
    private float _speed;
    private bool _exposed;
    private bool _flashlight;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _flashlight = player.GetComponent<Light2D>();
        _speed = 3f;
        _exposed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Update exposed parameter.
        
        
        //Only move when not exposed.
        if (!_exposed)
        {
            Vector3 offset = player.transform.position - transform.position;
            Vector3 direction = Vector3.ClampMagnitude(offset, 1f);
            MoveCharacter(direction);

            float inputX = direction.normalized.x;
            UpdateSprite(inputX);

        }
    }

    void MoveCharacter(Vector3 direction)
    {
        transform.position += direction.normalized * (_speed * Time.deltaTime);
    }

    void UpdateSprite(float inputX)
    {
        _anim.SetFloat("x", inputX);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
    }
}
