using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetection : MonoBehaviour
{
    private Mob mobScript;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        mobScript = this.transform.parent.GetComponent<Mob>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if(!mobScript.isDead)
        {
            if(other.gameObject.tag == "Player")
            {
                this.transform.GetComponent<Collider2D>().enabled = false;
                parent.GetComponent<Collider2D>().enabled = false;
                other.gameObject.GetComponent<PlayerControl>().PerformJump();
                mobScript.Death();
            }
        }
    }
}
