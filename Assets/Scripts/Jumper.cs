using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public GameObject jumperLeft;
    public GameObject jumperRight;
    public SpriteRenderer sr;

    private List<Animator> animators = new List<Animator>();

    private void Start() {
        animators.Add(GetComponent<Animator>());
        animators.Add(jumperLeft.GetComponent<Animator>());
        animators.Add(jumperRight.GetComponent<Animator>());
    }

    void Update()
    {
        Camera camera = Camera.main;
        float screenWidth = camera.aspect * 2f * camera.orthographicSize;

        if (transform.position.x - sr.bounds.size.x / 2 > screenWidth / 2) {
            Vector3 newPosition = transform.position;
            newPosition.x -= screenWidth;
            transform.position = newPosition;
        } else if (transform.position.x + sr.bounds.size.x / 2 < -screenWidth / 2) {
            Vector3 newPosition = transform.position;
            newPosition.x += screenWidth;
            transform.position = newPosition;
        }

        Vector3 jumperLeftPosition = transform.position;
        jumperLeftPosition.x -= screenWidth;
        jumperLeft.transform.position = jumperLeftPosition;
        
        Vector3 jumperRightPosition = transform.position;
        jumperRightPosition.x += screenWidth;
        jumperRight.transform.position = jumperRightPosition;
    }

    public void AnimateFalling() {
        animators.ForEach(animator => {
            animator.SetBool("isFalling", true);
            animator.SetBool("isIdling", false);
            animator.SetBool("isSitting", false);
        });
    }

    public void AnimateIdle() {        
        animators.ForEach(animator => {
            animator.SetBool("isIdling", true);
            animator.SetBool("isFalling", false);
        });
    }

    public void AnimateSit() {
        animators.ForEach(animator => animator.SetBool("isIdling", false));
        animators.ForEach(animator => animator.SetBool("isSitting", true));
    }

    public void AnimateJump() {
        animators.ForEach(animator => animator.SetBool("isSitting", false));
    }

    public void AnimateReposition(float yVelocity) {
        animators.ForEach(animator => animator.SetFloat("yVelocity", yVelocity));
    }
}
