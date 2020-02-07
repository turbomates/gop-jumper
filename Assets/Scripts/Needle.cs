using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    public Transform needleTransform;
    
    private float needleSpeed;
    private const float MAX_ANGLE  = 90;
    private const float MIN_ANGLE  = -90;
    
    private bool isStopped = false;
    private bool isGoingToPositive = false;
    private float currentAngle = MIN_ANGLE;

    public float getCurrentAngle() {
        return currentAngle;
    }

    private void Start() {
        needleTransform.eulerAngles = new Vector3(0, 0, currentAngle);
        int level = Prefs.GetLevel();
        needleSpeed = 100f + level / 2;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isStopped = true;
        }

        if (!isStopped) {
            if (currentAngle >= MAX_ANGLE) {
                isGoingToPositive = false;
            }

            if (currentAngle <= MIN_ANGLE) {
                isGoingToPositive = true;
            }

            if (isGoingToPositive) {
                currentAngle += needleSpeed * Time.deltaTime;
            } else {
                currentAngle -= needleSpeed * Time.deltaTime;
            }

            needleTransform.eulerAngles = new Vector3(0, 0, currentAngle);
        }
    }
}
