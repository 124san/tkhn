using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Enemy
{
    private Animator m_Anim;
    private Rigidbody2D m_rigidbody;

    // Movement related
    [SerializeField] bool enableMovement = true;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] bool movingRight = true;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        m_Anim = this.transform.GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        // Enemy facing
        if(!movingRight){
            Flip(false);
        } else {
            Flip(true);
        }

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if(enableMovement)
        {
            float speed = movingRight ? moveSpeed : -moveSpeed;
            m_rigidbody.velocity = new Vector2(speed, 0.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Only deletect player collision when not ded
        if(!isDead && other.gameObject.tag == "Player")
        {
            float distance = Vector2.Distance(gameObject.transform.position, other.gameObject.transform.position);
            PlayerControl playerControl = other.gameObject.transform.GetComponent<PlayerControl>();
            playerControl.Death();
        }
    }

    public void Turn()
    {
        movingRight = !movingRight;
    }

    private void Flip(bool faceLeft) {
        float x = Mathf.Abs(transform.localScale.x);
        float y = Mathf.Abs(transform.localScale.y);
        float z = Mathf.Abs(transform.localScale.z);
        transform.localScale = new Vector3(faceLeft ? x : -x, y, z);
    }

    // Start death animation
    public override void Death() 
    {
        if(!isDead) 
        {
            // Mob falls down on death
            m_rigidbody.velocity = new Vector2(0.0f, -0.2f);
            this.transform.GetComponent<Collider2D>().enabled = false;
            HeadDetection head = this.gameObject.GetComponentInChildren<HeadDetection>();
            if (head) {
                head.GetComponent<Collider2D>().enabled = false;
            }
            audioManager.Play("Death");
            enableMovement = false;
            isDead = true;
            m_Anim.SetTrigger("Death");
        }
    }

    public override void Hurt()
    {
        Death();
    }

    // Destroy the object when death animation finish
    
}
