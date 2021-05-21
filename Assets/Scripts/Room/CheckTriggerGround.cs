using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTriggerGround : MonoBehaviour
{
    public bool playerIsInside;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13) playerIsInside = true;
    }
}
