using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovementTurnZones : MonoBehaviour
{
    // Turn the mob triggered horizontally 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Mob"))
        {
            Mob mob = other.gameObject.transform.GetComponent<Mob>();
            mob.Turn();
        }
    }
}
