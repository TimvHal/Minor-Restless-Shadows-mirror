using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheatToggle : MonoBehaviour, IPointerClickHandler
{
    public Image checkbox;
    public Sprite[] boxes;
    public int index = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.buttonPress);
        CheatManager.ToggleCheat(CheatManager.Cheat.God);
        checkbox.sprite = boxes[index];
        index = index == 0 ? 1 : 0;
    }
}
