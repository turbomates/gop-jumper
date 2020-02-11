using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = .3f;
    
    public float cameraBottomHeight = -2f;
    public float cameraLowestPositionY = 0;
    private Vector3 currentVelocity;
    private float deltaTime;

    private void Awake() {
        deltaTime = Time.deltaTime;
    }

    void LateUpdate() {
        if (cameraLowestPositionY > transform.position.y) {
            transform.Translate(Vector3.up * deltaTime);
        } else 
        if (target.position.y - cameraBottomHeight > cameraLowestPositionY) {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y - cameraBottomHeight, transform.position.z);
            transform.position = newPos;
        }    
    }

    public void ChangeCameraLowestPositionY(float positionY) {
        cameraLowestPositionY = positionY + Camera.main.orthographicSize;
    }
}
