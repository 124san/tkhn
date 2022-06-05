using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour {

    public PlayerControl m_root;

    // Use this for initialization
    void Start()
    {
        m_root = this.transform.root.GetComponent<PlayerControl>();
       
    }

 

    ContactPoint2D[] contacts = new ContactPoint2D[1];

    
    void OnTriggerEnter2D(Collider2D other)
    {
        GroundCheck(other);
    }
    private void OnTriggerStay2D(Collider2D other) {
        GroundCheck(other);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Block")) 
            m_root.isGrounded = false;
     
    }

    void GroundCheck(Collider2D other) {
        if (other.CompareTag("Ground") || other.CompareTag("Block")) {
            if (other.CompareTag("Ground"))
            {
                m_root.isDownJumpGroundCheck = true;

            }
            else
            {
                m_root.isDownJumpGroundCheck = false;
            }

            if (m_root.m_rigidbody.velocity.y <= 0.05)
            {
                m_root.isJumping = false;
                m_root.isGrounded = true;
                m_root.isPushed = false;
                m_root.currentJumpCount = 0;
                m_root.dashCount = 0;
            }
        } 
    }

}
