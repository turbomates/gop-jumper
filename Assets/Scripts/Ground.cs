using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private Game game;
    private CameraFollow cameraFollow;

    private bool isLanded = false;
    private Vector2 previousPosition;

    private void Start() {
        game = GameObject.Find("Game").GetComponent<Game>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    public void Land() {
        if (!isLanded) {
            isLanded = true;
            cameraFollow.ChangeCameraLowestPositionY(transform.position.y);
            game.FinishLevel();
        }
    }
}
