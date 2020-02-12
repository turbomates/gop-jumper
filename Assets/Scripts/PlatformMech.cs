﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMech : MonoBehaviour
{
    public GameObject platformLeft;
    public GameObject platformRight;
    public List<GameObject> bulbs;
    public EdgeCollider2D colider;

    private List<SpriteRenderer> bulbsSR = new List<SpriteRenderer>();
    private bool isCountdownActive = false;
    private float timeStep = 0.5f;
    private float waitingTime = 0f;
    private int progress = 0;

    private bool isTranslatingActive = false;
    private bool isTransltingReverseActive = false;

    private void Awake() {
        bulbs.ForEach(bulb => bulbsSR.Add(bulb.GetComponent<SpriteRenderer>()));
        SetBulbsColor(Color.red);
    }

    private void Update() {
        if (isTransltingReverseActive) {
            platformLeft.transform.Translate(Vector3.right * Time.deltaTime * 5f);
            platformRight.transform.Translate(Vector3.left * Time.deltaTime * 5f);

            if (platformLeft.transform.localPosition.x >= -0.65) {
                isTransltingReverseActive = false;
                colider.enabled = true;
                SetBulbsColor(Color.red);
            }
        }

        if (isTranslatingActive) {
            platformLeft.transform.Translate(Vector3.left * Time.deltaTime * 5f);
            platformRight.transform.Translate(Vector3.right * Time.deltaTime * 5f);

            if (platformLeft.transform.localPosition.x < -1.7) {
                isTranslatingActive = false;
                isTransltingReverseActive = true;
            }
        }

        if (isCountdownActive) {
            if (progress > bulbsSR.Count) {
                isCountdownActive = false;
                progress = 0;
                waitingTime = 0;
                isTranslatingActive = true;
                colider.enabled = false;
                SetBulbsColor(Color.green);
            } else {
                waitingTime += Time.deltaTime;

                if (waitingTime >= timeStep) {
                    if (progress < bulbsSR.Count) bulbsSR[progress].color = Color.yellow;
                    waitingTime = 0;
                    progress +=1;
                }
            }
        }
    }

    private void SetBulbsColor(Color color) {
        bulbsSR.ForEach(bulb => bulb.color = color);
    }

    public void StartCountdown() {
        isCountdownActive = true;
    }
}