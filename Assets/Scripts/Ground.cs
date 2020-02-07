using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private Game game;
    private CameraFollow cameraFollow;

    private bool isLanded = false;

    private void Start() {
        game = GameObject.Find("Game").GetComponent<Game>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!isLanded && collision.collider.GetComponent<Rigidbody2D>().velocity.y <= 0) {
            isLanded = true;

            cameraFollow.ChangeCameraLowestPositionY(transform.position.y);
            game.FinishLevel();
        }
    }
}
