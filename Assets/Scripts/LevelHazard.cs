using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazard : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject player = other.gameObject;
        if (player.CompareTag("Player")) {
            PlayerControl playerControl = player.transform.GetComponent<PlayerControl>();
            playerControl.Death();
        }
    }
}
