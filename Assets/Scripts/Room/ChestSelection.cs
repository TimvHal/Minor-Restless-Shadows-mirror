using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestSelection : MonoBehaviour
{
    public UnityEvent _unityEvent = new UnityEvent();
    public GameObject[] chests;

    private void Start()
    {
        _unityEvent.AddListener(listener);
    }

    public void listener()
    {
        foreach (var GameObjects in chests)
        {
            if (!GameObjects.GetComponent<BuffBox>().inactive)
            {
                Destroy(GameObjects);
            }
        }
    }
}
