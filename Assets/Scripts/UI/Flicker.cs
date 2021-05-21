using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flicker : MonoBehaviour
{
    public Light2D flicker;

    private float increment = 0.05f;

    public GameObject bosses;

    private SpriteRenderer bosssprite;

    public Light2D text;
    // Start is called before the first frame update
    void Start()
    {
        flicker = GetComponent<Light2D>();
        bosssprite = bosses.GetComponent<SpriteRenderer>();
        StartCoroutine(flick());
    }

    IEnumerator flick()
    {
        yield return new WaitForSeconds(0.25f);
        if (flicker.pointLightInnerRadius >= 0.9 || flicker.pointLightInnerRadius <= 0.6) increment *= -1;
        flicker.pointLightInnerRadius += increment;
        var temp = bosssprite.color;
        text.intensity += text.intensity == 1 ? -increment : increment; 
        temp.a -= increment;
        bosssprite.color = temp;
        
        StartCoroutine(flick());
    }
}
