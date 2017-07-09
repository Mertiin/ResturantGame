using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MovementSpeed = 0.1f;
    public float ScrollSpeed = 5f;
    public Camera Camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Camera.transform.position += new Vector3(0, MovementSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Camera.transform.position += new Vector3(0, -MovementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.transform.position += new Vector3(-MovementSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Camera.transform.position += new Vector3(MovementSpeed, 0);
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
		var newSize = Camera.orthographicSize - scroll * ScrollSpeed;
		if(newSize > 0 && newSize < 20){
			Camera.orthographicSize = newSize;
		}
    }
}
