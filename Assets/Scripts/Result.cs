using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text text;

    private bool isActive = false;
    private int currentCoins = 0;
    private int picked;
    private int all;
    private float time = 0f;
    private float timeStep = 0f;
    private float deltaTime;

    private void Awake() {
        deltaTime = Time.deltaTime;
    }

    private string GetCurrentReward() {
        return currentCoins + "/" + all;
    }
 
    void Update() {
        if (isActive) {
            if (currentCoins >= picked) {
                isActive = false;
                time = 0f;
            } else {
                time += deltaTime;

                if (time >= timeStep) {
                    currentCoins += 1;
                    text.text = GetCurrentReward();
                    time = 0;
                }
            }
        }
    }

    public void SetCoins(int picked, int all) {
        this.picked = picked;
        this.all = all;
        currentCoins = 0;
        timeStep = 1.5f / picked;
        text.text = GetCurrentReward();
        isActive = true;
    }
}
