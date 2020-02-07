using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public Rigidbody2D rb;
  public BoxCollider2D colider;

  public GameObject needlePrefab;
  public GameObject powerPrefab;

  public Jumper jumper;

  private GameObject needle;
  private GameObject power;

  private bool isOnPlatform = false;
  private float angle;
  private Vector2 previousPosition;

  private List<GameObject> shadowObjects = new List<GameObject>();

  private void Update() {
    Quaternion rotation = transform.rotation;
    rotation.z = rb.velocity.x * -0.01f;
    transform.rotation = rotation;

    // if (transform.position.y > 5) {
    //   Time.timeScale = 0f;
    // }

    if (isOnPlatform && Input.GetMouseButtonDown(0)) {
      if (power == null && needle != null) {
        angle = needle.GetComponent<Needle>().getCurrentAngle() + 90;
        
        jumper.AnimateSit();
        InstantiatePower();
      } else {
        jumper.AnimateJump();
        float force = power.GetComponent<Power>().getCurrentForce();
        Jump(force);
      }
    }

    jumper.AnimateReposition(rb.velocity.y);
  }

  private void Jump(float force) {
    Vector2 velocity = rb.velocity;
    velocity.x = force / 2 * Mathf.Cos((float) angle * Mathf.Deg2Rad);
    velocity.y = force;
    rb.velocity = velocity;

    isOnPlatform = false;
  }

  private void OnCollisionStay2D(Collision2D collision) {
    if (rb.velocity.y <= 0f && rb.position == previousPosition && !isOnPlatform) {
      if (collision.collider.GetComponent<MovePlatform>() != null) {
        transform.SetParent(collision.collider.transform);
      }

      InstantiateNeedle();
      isOnPlatform = true;
      jumper.AnimateIdle();
    }

    previousPosition = rb.position;
  }

  private void OnCollisionExit2D(Collision2D collision) {
    DestroyNeedleAndPower();

    if (collision.collider.GetComponent<MovePlatform>() != null) {
      transform.SetParent(null);
    }
  }

  private void DestroyNeedleAndPower() {
    if (!isOnPlatform) {
      if (needle != null) 
        Destroy(needle);
      if (power != null)
        Destroy(power);
      shadowObjects.ForEach(obj => Destroy(obj));
    }
  }

  private void InstantiateNeedle() {
    Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    needle = Instantiate(needlePrefab, position, Quaternion.identity);
    needle.transform.parent = transform;

    float width = Camera.main.aspect * 2f * Camera.main.orthographicSize;
    shadowObjects.Add(Instantiate(needlePrefab, new Vector3(position.x - width, position.y, position.z), Quaternion.identity));
    shadowObjects.Add(Instantiate(needlePrefab, new Vector3(position.x + width, position.y, position.z), Quaternion.identity));
  }

  private void InstantiatePower() {
    Vector3 position = transform.position;
    position.x = transform.position.x > 0 ? position.x - 0.5f : position.x + 0.5f;
    power = Instantiate(powerPrefab, position, Quaternion.identity);
    power.transform.parent = transform;

    float width = Camera.main.aspect * 2f * Camera.main.orthographicSize;
    shadowObjects.Add(Instantiate(powerPrefab, new Vector3(position.x - width, position.y, position.z), Quaternion.identity));
    shadowObjects.Add(Instantiate(powerPrefab, new Vector3(position.x + width, position.y, position.z), Quaternion.identity));
  }
}
