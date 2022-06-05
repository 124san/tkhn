using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventManager : MonoBehaviour
{
    public PlayerControl playerControl;

    void Start() {
        playerControl = this.transform.root.GetComponent<PlayerControl>();
    }

    void Respawn() {
        playerControl.Respawn();
    }

    void Attack()
    {
        playerControl.Attack();
    }
}
