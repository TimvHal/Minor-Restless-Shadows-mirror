using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Calculations;

public class PlayerGun : MonoBehaviour
{
    public UserControls _userControls;

    public Vector3[] gunPositions;

    public int HoverAnimationIndex;
    private Animator anim;
    public Animator player;
    public GameObject bullet;

    float speed = 5f;

    private int oldIndex = 0;

    private bool walk;

    private bool bulletlock;

    private float currentMagazine;

    public TextMeshPro ammoDisplay;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ammoDisplay.sortingOrder = 10000;
        currentMagazine = BuffManager.GetBuff(BuffManager.BuffType.magazineSize).currentValue;
    }

    private void Update()
    {
        checkShoot();
        anim.SetBool("isWalking",player.GetBool("isWalking"));
        float angle = _userControls.aimingStick.angle;
        
        // 30 | 90 | 150 | 270 \\
        int index = 0;
        if (angle > 30 && angle <= 90)
        {
            index = 0;
        }if (angle > 90 && angle <= 150)
        {
            index = 1;
        }if (angle > 150 && angle <= 270)
        {
            index = 2;
        }if ((angle > 270 && angle <= 360) || (angle >= 0 && angle <= 30))
        {
            index = 3;
        }

        if (index != oldIndex) HoverAnimationIndex = 0;
        else HoverAnimationIndex += 1;
        if (walk && HoverAnimationIndex > 3) HoverAnimationIndex = 0;
        if (!walk && HoverAnimationIndex > 4) HoverAnimationIndex = 0;
        oldIndex = index;
        
        if (gunPositions[index].z < 0) angle = ReverseAngle(angle);
        transform.rotation = Quaternion.AngleAxis(TranslateTo180(angle), Vector3.forward);
        transform.localPosition = new Vector2(gunPositions[index].x,gunPositions[index].y);
        transform.localScale = new Vector3(gunPositions[index].z, transform.localScale.y, transform.localScale.z);
    }

    private void checkShoot()
    {
        if (!bulletlock && _userControls.aimingStick.touchStart) StartCoroutine(Shoot());
    }

    public void forceShoot()
    {
        if (!bulletlock) StartCoroutine(Shoot());
    }
    
    IEnumerator Shoot()
    {
        bulletlock = true;
        SoundManager.PlaySound(SoundManager.Sound.PlayerShoot);
        var multishot = BuffManager.GetBuff(BuffManager.BuffType.multishot).currentValue;
        var rounded = (int)Math.Ceiling( multishot/ 2);
        int ms = multishot % 2 == 0 ? 1 : 0;
        currentMagazine -= 1;
        if (multishot > 1) {
            for (int i = -rounded + 1; i < rounded + ms; i++)
            {
                var pos = transform.position;
                var angle = _userControls.aimingStick.angle;
                angle += i * 1;
                if (angle < 0) angle += 360;
                if (angle > 360) angle -= 360;
                ammoDisplay.text = "Ammo: " + currentMagazine;
                SetSpeed(Instantiate(bullet, GetCirlePosition(pos, 0.5f, angle), Quaternion.identity), _userControls.aimingStick.vector);
            }
        }
        else
        {
            var pos = transform.position;
            var angle = _userControls.aimingStick.angle;
            ammoDisplay.text = "Ammo: " + currentMagazine;
            SetSpeed(Instantiate(bullet, pos, transform.rotation), _userControls.aimingStick.vector);
        }
        
        var firerate = BuffManager.GetBuff(BuffManager.BuffType.firerate).currentValue;
        if (currentMagazine <= 0)
            SoundManager.PlaySound(SoundManager.Sound.PlayerReload);
        yield return new WaitForSeconds(currentMagazine > 0 ? 1/firerate: (1/firerate) * 5);
        if (currentMagazine <= 0)
        {
            currentMagazine = BuffManager.GetBuff(BuffManager.BuffType.magazineSize).currentValue;
            ammoDisplay.text = "Ammo: " + currentMagazine;
        }
        bulletlock = false;
    }

    private void SetSpeed(GameObject clone, Vector2 direction)
    {
        float frontalAngle = TranslateTo180(_userControls.aimingStick.angle);
        Physics2D.IgnoreCollision(clone.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
        clone.transform.rotation = Quaternion.AngleAxis(frontalAngle, Vector3.forward);
        clone.GetComponent<Rigidbody2D>().velocity = direction * (speed * BuffManager.GetBuff(BuffManager.BuffType.velocity).currentValue);
        var scale= BuffManager.GetBuff(BuffManager.BuffType.hitbox).currentValue;
        clone.transform.localScale = clone.transform.localScale * scale;
    }

}
