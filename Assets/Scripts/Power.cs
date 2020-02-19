using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public GameObject power;

    private float powerSpeed;
    
    private const float MAX_HEIGHT = 1f;
    private const float MIN_HEIGHT = 0f;

    private float maxForce;
    private const float MAX_FORCE = 11f;
    private const float MIN_FORCE = 3f;

    private bool isGoingToPositive = false;
    private bool isStopped = false;
    private float currentHeight = MIN_HEIGHT;

    public void SetMaxForce(double powerup) {
        maxForce = MAX_FORCE + (float) powerup / 2;
    }

    public float GetCurrentForce() {
        return maxForce * currentHeight + MIN_FORCE;
    }

    private void Start() {
        SetMaxForce(Prefs.GetPowerup());
        powerSpeed = 2f + Prefs.GetLevel() / 100;
    }

    void Update()
    {
        float step = powerSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) {
            isStopped = true;
        }

        if (!isStopped) {
            if (currentHeight >= MAX_HEIGHT) {
                isGoingToPositive = false;
            }

            if (currentHeight <= MIN_HEIGHT) {
                isGoingToPositive = true;
            }

            if (isGoingToPositive) {
                currentHeight += step;
            } else {
                currentHeight -= step;
            }

            float YPosition = isGoingToPositive ? power.transform.position.y + step / 2 : power.transform.position.y - step / 2;
            power.transform.localScale = new Vector3(power.transform.localScale.x, currentHeight);
            power.transform.position = new Vector3(power.transform.position.x, YPosition);
        }
    }
}
