using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpking : MonoBehaviour {
    private Animator animator;

    private Vector2? lastRollNormal;
    private float rollStart;

    private Rigidbody2D body;

    private bool onFloor = false;

    [SerializeField]
    private float jumpPower = 1;

    [SerializeField, Range(1, 100)]
    private float rollTorque = 5;

    private Vector2 startPos;
    private int bonusScore;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        body = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start() {
        startPos = transform.position;
        bonusScore = 0;
    }

    private void OnMouseDown() {
        lastRollNormal = RollNormal();
        rollStart = Time.time;
    }

    private void OnMouseUp() {
        lastRollNormal = null;

        if (Time.time < rollStart + 0.25f) {
            if (onFloor) {
                animator.SetTrigger("Poke");

                body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    private void Update() {
        if (lastRollNormal.HasValue) {
            var pokeLine = RollNormal() * 5f;

            Debug.DrawLine(transform.position,
                transform.position + new Vector3(pokeLine.x, pokeLine.y, 0),
                Color.blue, Time.deltaTime);

            var newRollNormal = RollNormal();
            var rollAmount = Vector2.SignedAngle(lastRollNormal.Value, newRollNormal);

            body.AddTorque(Time.deltaTime * rollTorque * rollAmount);
        }

        if (Time.frameCount % 60 == 0) {
            var score = GetScore();
            if (score > GetHiScore()) {
                PlayerPrefs.SetInt("HiScore", score);
            }
        }
    }

    public int GetScore() {
        var travelled = ((Vector2)transform.position - startPos).x;
        if (travelled > float.Epsilon) {
            return Mathf.FloorToInt(travelled * 10) + bonusScore;
        } else {
            return bonusScore;
        }
    }

    public int GetHiScore() {
        return PlayerPrefs.GetInt("HiScore");
    }

    private Vector2 RollNormal() {
        var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return ((Vector2)(worldTouch - transform.position)).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor")) {
            onFloor = true;

            animator.SetTrigger("Poke");
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.gameObject.CompareTag("Pickup")) {
            var pickup = trigger.gameObject.GetComponent<BonusPickup>();
            if (pickup) {
                bonusScore += pickup.Value;
            }
            Destroy(pickup.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor")) {
            onFloor = false;
        }
    }
}
