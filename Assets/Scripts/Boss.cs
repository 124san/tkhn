using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] float baseAttackInterval = 3f;
    
    [SerializeField] float speedUpRate = 2f;
    [SerializeField] Vector3 spellOffset = new Vector3(0,0,0);
    [SerializeField] GameObject spellPrefab;
    public int prefireNum = 2;
    public bool isDead = false;
    public int health = 2;
    public bool invinsible = false;
    public int rateApplyTime = 0;
    private float timer;
    private Animator animator;
    void Start()
    {
        base.Init();
        timer = baseAttackInterval;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            timer = baseAttackInterval * Mathf.Pow((1f/speedUpRate), rateApplyTime);
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            animator.SetTrigger("Cast");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Only deletect player collision when not ded
        if(!isDead && other.gameObject.tag == "Player")
        {
            float distance = Vector2.Distance(gameObject.transform.position, other.gameObject.transform.position);
            Debug.Log(distance);
            PlayerControl playerControl = other.gameObject.transform.GetComponent<PlayerControl>();
            playerControl.Death();
        }
    }

    public override void Death(){
        // TODO
        audioManager.Play("Death");
        animator.SetTrigger("Death");
        this.gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public override void Hurt() {
        health -= 1;
        rateApplyTime += 1;
        if (health <= 0) Death();
        else {
            animator.SetTrigger("Hurt");
            audioManager.Play("Hurt");
        }
    }

    public void Cast() {
        Debug.Log("bam");
        audioManager.Play("Cast");
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Instantiate(spellPrefab, playerPosition+spellOffset, Quaternion.identity);
    }
    
}
