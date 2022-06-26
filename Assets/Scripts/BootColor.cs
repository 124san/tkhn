using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootColor : MonoBehaviour
{
    private Renderer rend;
    [SerializeField] Color colorToTurnTo = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = colorToTurnTo;
    }

    public void changeColor(Color c)
    {
        rend = GetComponent<Renderer>();
        print(rend);
        print(c);
        rend.material.color = c;
    }
}
