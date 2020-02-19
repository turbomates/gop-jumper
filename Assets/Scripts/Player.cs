﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
  public Rigidbody2D rb;
  public BoxCollider2D colider;

  public GameObject needlePrefab;
  public GameObject powerPrefab;

  public Jumper jumper;

  private GameObject needleGameObject;
  private GameObject power;
  private Game game;

  private bool isOnPlatform = false;
  private float angle;
  private Vector2 previousPosition;

  private List<GameObject> needles = new List<GameObject>();
  private List<GameObject> powers = new List<GameObject>();

  private void Awake() {
    game = GameObject.Find("Game").GetComponent<Game>();
  }

  private void Update() {
    game.UpdatePlayerProgress(transform.position.y);

    Quaternion rotation = transform.rotation;
    rotation.z = rb.velocity.x * -0.01f;
    transform.rotation = rotation;

    if (isOnPlatform) {
      if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && power == null && needleGameObject != null) {
        StopNeedle();
        jumper.AnimateSit();
        InstantiatePower();
      } else if (Input.GetMouseButtonUp(0) && power != null) {
        jumper.AnimateJump();
        float force = power.GetComponent<Power>().GetCurrentForce();
        Jump(force);
      }
    }

    jumper.AnimateReposition(rb.velocity.y);
  }

  private void StopNeedle() {
    Needle needle = needleGameObject.GetComponent<Needle>();
    needle.Stop();
    needles.ForEach(obj => obj.GetComponent<Needle>().Stop());
    angle = needle.GetCurrentAngle() + 90;
  }

  private void Jump(float force) {
    Vector2 velocity = rb.velocity;
    velocity.x = force / 3 * Mathf.Cos((float) angle * Mathf.Deg2Rad);
    float xForceFaultCoeficient = angle > 90f ? angle - 90f : 90f - angle;
    velocity.y = force - xForceFaultCoeficient / 30f;
    rb.velocity = velocity;
  }

  private void OnCollisionStay2D(Collision2D collision) {
    if (rb.velocity.y <= 0f && rb.position == previousPosition && !isOnPlatform) {
      if (collision.collider.GetComponent<MovePlatform>() != null) {
        transform.SetParent(collision.collider.transform);
      } else {
        PlatformMech platformMech = collision.collider.GetComponent<PlatformMech>();
        if (platformMech != null) {
          platformMech.StartCountdown();
        } else {
          Ground ground = collision.collider.GetComponent<Ground>();
          if (ground != null) {
            ground.Land();
          }
        }
      } 

      InstantiateNeedle();
      isOnPlatform = true;
      jumper.AnimateIdle();
    }

    previousPosition = rb.position;
  }

  private void OnCollisionExit2D(Collision2D collision) {
    isOnPlatform = false;
    DestroyNeedleAndPower();

    if (collision.collider.GetComponent<MovePlatform>() != null) {
      transform.SetParent(null);
    }
  }

  public void DestroyNeedleAndPower() {
    if (!isOnPlatform) {
      if (needleGameObject != null) 
        Destroy(needleGameObject);
      if (power != null)
        Destroy(power);
      needles.ForEach(obj => Destroy(obj));
      powers.ForEach(obj => Destroy(obj));
      needles.Clear();
      powers.Clear();
    }
  }

  private void InstantiateNeedle() {
    Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    needleGameObject = Instantiate(needlePrefab, position, Quaternion.identity);
    needleGameObject.transform.parent = transform;

    float width = Camera.main.aspect * 2f * Camera.main.orthographicSize;
    needles.Add(Instantiate(needlePrefab, new Vector3(position.x - width, position.y, position.z), Quaternion.identity));
    needles.Add(Instantiate(needlePrefab, new Vector3(position.x + width, position.y, position.z), Quaternion.identity));
  }

  private void InstantiatePower() {
    Vector3 position = transform.position;
    position.x = transform.position.x > 0 ? position.x - 0.5f : position.x + 0.5f;
    power = Instantiate(powerPrefab, position, Quaternion.identity);
    power.transform.parent = transform;

    float width = Camera.main.aspect * 2f * Camera.main.orthographicSize;
    powers.Add(Instantiate(powerPrefab, new Vector3(position.x - width, position.y, position.z), Quaternion.identity));
    powers.Add(Instantiate(powerPrefab, new Vector3(position.x + width, position.y, position.z), Quaternion.identity));
  }
}
