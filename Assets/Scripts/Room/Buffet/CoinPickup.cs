using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.layer == 13)
      {
         ScoreUI.instance.UpdateValue(1);
         Destroy(gameObject);
      }
   }
}
