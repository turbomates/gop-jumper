using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public SpriteRenderer sr;

    private Vector3 vertDirection = Vector3.down;
    private Vector3 horDirection = Vector3.left;

    void Update()
    {
        if (transform.localPosition.y > 0.2 || transform.localPosition.y < -0.2) {
            vertDirection = vertDirection == Vector3.down ? Vector3.up : Vector3.down;
        }

        if (transform.localPosition.x > -1.2 || transform.localPosition.x < -2) {
            horDirection = horDirection == Vector3.left ? Vector3.right : Vector3.left;
        } 

        transform.Translate(horDirection * speed * Time.deltaTime);
        transform.Translate(vertDirection * speed * Time.deltaTime);
    }

    public void ChangeColor(int level) {
        float gb = 1f - (level / 80f);
        sr.color = new Color(1f, gb, gb, 255);
    }
}
