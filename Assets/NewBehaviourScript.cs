using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float rotSpeed = 2.5f;
    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x - rotSpeed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x + rotSpeed, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
    }
}
