using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float clampAngle = 80.0f;
    public int speedSlowDown = 5;
    public float speed = 5.0f;
    public float scrollSpeed = 5.0f;

    private float xSpeed = 0.0f;
    private float ySpeed = 0.0f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            transform.eulerAngles = new Vector3(transform.rotation.y, transform.rotation.x, transform.rotation.z);
            xSpeed += speedH * Input.GetAxis("Mouse X");
            ySpeed -= speedV * Input.GetAxis("Mouse Y");

            ySpeed = Mathf.Clamp(ySpeed, -clampAngle, clampAngle); //Stops Camera flipping

            transform.eulerAngles = new Vector3(ySpeed, xSpeed, 0.0f);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (transform.position.y < 3)
        {
            GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
        }

        Vector3 dir = transform.forward;
        Vector3 dirRight = transform.right;
        dir.y = 0;
        dir = dir.normalized;
        dirRight.y = 0;
        dirRight = dirRight.normalized;
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody>().velocity += speed * dir;
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody>().velocity += -speed * dir;
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().velocity += -speed * dirRight;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().velocity += speed * dirRight;
        }
        var scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 dirScroll = transform.forward;
        if (scroll > 0f)
        {
            if (transform.position.y > 3)
                GetComponent<Rigidbody>().velocity += scrollSpeed * dirScroll; //Scroll in
        }
        else if (scroll < 0f)
        {
            GetComponent<Rigidbody>().velocity += -scrollSpeed * dirScroll; //Scroll out
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed /= speedSlowDown;
            scrollSpeed /= speedSlowDown;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed *= speedSlowDown;
            scrollSpeed *= speedSlowDown;
        }
    }
}
