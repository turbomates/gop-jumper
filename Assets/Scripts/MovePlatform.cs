using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
  public float speed = 1f;
  public SpriteRenderer sr;

  private Vector3 direction = Vector3.left;

  private void Update() {
    float screenWidth = Camera.main.aspect * 2f * Camera.main.orthographicSize;
    
    if (transform.position.x < -screenWidth / 2 + sr.bounds.size.x / 2) {
      direction = Vector3.right;
    } else if (transform.position.x > screenWidth / 2 - sr.bounds.size.x / 2) {
      direction = Vector3.left;
    }

    transform.Translate(direction * Time.deltaTime * speed);
  }
}
