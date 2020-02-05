using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = .3f;
    public float cameraBottomHeight = -2f;
    private Vector3 currentVelocity;

    void LateUpdate() {
        if (target.position.y - cameraBottomHeight > 0) {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y - cameraBottomHeight, transform.position.z);
            transform.position = newPos;
        }        
    }
    
}
