using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    private LevelLoader loader;
    
    void Start()
    {
        loader = this.transform.parent.GetComponent<LevelLoader>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            loader.LoadNextLevel();
        }
    }
}
