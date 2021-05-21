using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimatedDeath : MonoBehaviour
{
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
