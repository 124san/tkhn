using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontdestory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    private static dontdestory instance = null;
    public static dontdestory Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}