using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected AudioManager audioManager;

    // Start is called before the first frame update
    protected void Init()
    {
        audioManager = this.transform.Find("AudioManager").GetComponent<AudioManager>();
    }

    public abstract void Hurt();

    public abstract void Death();

    public void DestroySelf() 
    {
        Destroy(this.gameObject);
    }
}
