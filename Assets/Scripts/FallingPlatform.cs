using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float fallingDelay = 0.5f;
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private bool activated = false;
    private float timer;
    private float destroyTimer = 3f;
    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody2D>();
        timer = fallingDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                this.transform.GetComponent<Collider2D>().enabled = false;
                this.transform.parent.GetComponent<Collider2D>().enabled = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                rb.gravityScale = 2;
                destroyTimer -= Time.deltaTime;
                if (destroyTimer <= 0f) {
                    GameObject.Destroy(rb.gameObject);
                }
                MobMovementTurnZones turnZone = this.transform.parent.GetComponentInChildren<MobMovementTurnZones>();
                if (turnZone) {
                    GameObject.Destroy(turnZone.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        GameObject player = other.gameObject;
        if (player.CompareTag("Player")) {
            // To check if player is standing on the platform
            PlayerControl pc = player.GetComponent<PlayerControl>();
            if (pc.isGrounded)
                activated = true;
        }
    }
}
