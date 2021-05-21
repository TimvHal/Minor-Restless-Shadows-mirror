using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagenumber : MonoBehaviour
{
    public SpriteRenderer _Renderer;

    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        _Renderer = GetComponent<SpriteRenderer>();
        enabled = false;
        StartCoroutine(ded());
    }

    // Update is called once per frame
    void Update()
    {
        if (damage > 0)
        {
            enabled = true;
            var col = _Renderer.color;
            col.a *= 0.9f;
            _Renderer.color = col;
            var pos = new Vector2(transform.position.x,transform.position.y + 0.001f);
            transform.position = pos;
        }
    }

    IEnumerator ded()
    {
        yield return new  WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
    
}
