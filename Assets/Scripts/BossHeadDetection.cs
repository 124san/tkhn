using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadDetection : MonoBehaviour
{
    private Boss boss;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        boss = parent.GetComponent<Boss>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if(!boss.isDead)
        {
            if(other.gameObject.tag == "Player")
            {
                PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
                player.isPushed = true;
                Vector2 force = new Vector2(-0.5f, -1.0f);
                player.m_rigidbody.AddForce(force * 30, ForceMode2D.Impulse);
                boss.Hurt();
            }
        }
    }
}
