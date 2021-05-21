using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaga : MonoBehaviour
{

    public DoorDeletion _bossDoor;
    public GameObject _plaga;

    // Update is called once per frame
    void Update()
    {
        if (_bossDoor == null) return;
        if (_bossDoor.deleted)
        {
            StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(3f);
        _plaga.SetActive(true);
    }
}
