using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class joystick : MonoBehaviour
{
    public Transform gun;
    public Transform player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform circle;
    public Transform outerCircle;

    void Start()
    {
        pointA = outerCircle.position;
    }
    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0)){
            circle.transform.position = pointA;
            outerCircle.transform.position = pointA;
        }
        if(Input.GetMouseButton(0)){
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }else{
            touchStart = false;
            circle.transform.position = pointA;
        }
        
    }
    private void FixedUpdate(){
        if(touchStart){
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            moveCharacter(direction);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
             
            if (direction != Vector2.zero) 
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                gun.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

    }
    void moveCharacter(Vector2 direction){
        player.Translate(direction * speed * Time.deltaTime);
    }
}
