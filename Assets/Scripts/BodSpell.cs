using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodSpell : MonoBehaviour
{
    public Transform attackPoint;
    
    [SerializeField] Vector2 attackSize = new Vector2(1, 1);
    private int preFireNum;
    private int preFireCounter = 0;

    private AudioManager audioManager;
    private Animator m_Anim;

    void Start()
    {
        audioManager = this.transform.Find("AudioManager").GetComponent<AudioManager>();
        m_Anim = this.transform.GetComponent<Animator>();
        preFireNum = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().prefireNum;
    }

    public void PreFire() {
        preFireCounter += 1;
        if(preFireCounter >= preFireNum) {
            preFireCounter = 0;
            m_Anim.SetTrigger("Cast");
        }
    } 

    public void SpellHit() {
        Debug.Log(audioManager);
        audioManager.Play("Strike");
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0);
        foreach(Collider2D player in hitPlayers) {
            if (player.CompareTag("Player")) {
                PlayerControl playerControl = player.gameObject.transform.GetComponent<PlayerControl>();
                playerControl.Death();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

    public void DestroySelf() 
    {
        Destroy(this.gameObject);
    }
}
