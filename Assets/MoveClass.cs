using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class MoveClass : MonoBehaviour
{
	public float speed = 0.5f;
	public float goodDegs = 30.0f; // degrees per second
	public float badDegs = -30.0f; // degrees per second
    public float rotSpeed = 2.5f;
    public bool isDown = false;
    public float groundY = -1;
    public float velocity = 3;
    public void FixedUpdate()
	{

        if (Input.GetKey(KeyCode.D)) // Right
		{
            transform.Translate(new Vector3(speed, 0, 0));
        }
		if(Input.GetKey(KeyCode.A)) // Left
		{
            transform.Translate(new Vector3(-1 * speed, 0, 0));
        }

        if (Input.GetKey(KeyCode.LeftShift)) // Down
        {
            if (!isDown) {
                transform.Translate(new Vector3(0, -2, 0));
                speed /= 2;
                isDown = true;
            }  
        } else
        {
            if (isDown) {
                transform.Translate(new Vector3(0, 2, 0));
                speed *= 2;
            }

            isDown = false;
        }
        if (Input.GetKey(KeyCode.S)) // Back
        {
            transform.Translate(new Vector3(0, 0, -1 * speed));
        }
        if (Input.GetKey(KeyCode.W)) // Forward
        {
            transform.Translate(new Vector3(0, 0, speed));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - rotSpeed, transform.localRotation.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + rotSpeed, transform.localRotation.eulerAngles.z);
        }

    }
}