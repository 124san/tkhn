using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    public Rigidbody2D m_rigidbody;
    protected BoxCollider2D m_BoxCollider;
    protected Animator m_Anim;

    public float m_timeSinceAttack = 0.0f;

    private Vector2 newVelocity;

    public bool isDownJumpGroundCheck = true; // True if cannot perform down jump

    [Header("[Setting]")]

    // Equipment
    [SerializeField] bool armor = true;
    [SerializeField] bool boots = true;
    [SerializeField] bool sword = true;
    
    // Movement related
    [Header("[Movement]")]
    [SerializeField] float acceleration = 9f;
    [SerializeField] float deceleration = 6f;
    [SerializeField] float turnRate = 1f;
    [SerializeField] float maxSpeed = 3.0f;
    public float currSpeed = 0.0f;
    private bool faceLeft = true;

    // Jump related
    [Header("[Jump]")]
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] int maxJump = 2;
    public int currentJumpCount = 0;

    // Condition related
    [Header("[Condition]")]
    public bool isGrounded = false;
    public bool isSit = false;
    public bool isDying = false;
    public bool isAttacking = false;
    public bool isJumping = false;
    public bool isRunning = false;
    public bool isDashing = false;
    public bool isPushed = false;

    // Dash related
    [Header("[Dash]")]
    [SerializeField] bool doubleTapDashEnable = true;
    [SerializeField] bool buttonDashEnable = true;
    [SerializeField] float doubleTapInterval = 0.5f; // 0.5 second before double tap detection reset
    private float doubleTapTimer = 0.0f; 
    
    [SerializeField] float dashSpeed = 10.0f;
    [SerializeField] int maxDash = 1;
    [SerializeField] float dashDuration = 0.5f;
    private float dashTimer = 0.0f;
    public int dashCount = 0;
    public int dashDirection = 0; // dash Direction, 1 means right, -1 means left, 0 is neutral

    // Attack related
    [Header("[Attack]")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Sound related;
    [Header("[Sound]")]
    private AudioManager audioManager;

    // Death & Respawn related;
    [Header("[Death]")]
    public bool isDead = false; 

    void Start() {
        m_BoxCollider  = this.transform.GetComponent<BoxCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();

        audioManager = this.transform.Find("AudioManager").GetComponent<AudioManager>();

        // Face right on default
        faceLeft = false;
        Flip(false);

        // Change character appearence based on equipment
        // Helmet
        GameObject.Find("Hat-Helmet").gameObject.SetActive(armor);
        // Feet
        if(!boots) {
            GameObject.Find("Leg").GetComponent<BootColor>().changeColor(Color.white);
            GameObject.Find("Leg (1)").GetComponent<BootColor>().changeColor(Color.white);
        }
        // Sword
        GameObject.Find("Weapon").gameObject.SetActive(sword);
    }

    void Update() {
        if(!isDead) {
            // Get all input variable
            // p represent hold, c represent click
            bool h_left = Input.GetButton("Left");
            bool h_right = Input.GetButton("Right");
            bool h_up = Input.GetButton("Up");
            bool h_down = Input.GetButton("Down");

            bool c_attack = Input.GetButtonDown("Attack");
            bool c_jump = Input.GetButtonDown("Jump");
            bool c_dashLeft = Input.GetButtonDown("Left");
            bool c_dashRight = Input.GetButtonDown("Right");
            bool c_dashButton = Input.GetButtonDown("Dash");

            bool isStopping = h_down || !(h_left ^ h_right);
            
            // Horizontal Movement
            // Acceleration system
            // if(!isDashing) {
            //     if  (isStopping) {
            //         if(currSpeed > 0) {
            //             currSpeed = Mathf.Max(0, currSpeed - (deceleration * Time.deltaTime));
            //         } else if(currSpeed < 0) {
            //             currSpeed = Mathf.Min(0, currSpeed + (deceleration * Time.deltaTime));
            //         }
                    
            //     } else {
            //         // Move left
            //         if(h_left) {
            //             // If currently moving to the right
            //             if (currSpeed > 0) {
            //                 currSpeed = Mathf.Max(-maxSpeed, currSpeed - turnRate*(acceleration * Time.deltaTime));
            //             }
            //             else currSpeed = Mathf.Max(-maxSpeed, currSpeed - (acceleration * Time.deltaTime));
            //             faceLeft = true;
            //             Flip(true);
            //         }

            //         // Move right
            //         if(h_right){
            //             if (currSpeed < 0) {
            //                 currSpeed = Mathf.Min(maxSpeed, currSpeed + turnRate*(acceleration * Time.deltaTime));
            //             }
            //             else currSpeed = Mathf.Min(maxSpeed, currSpeed + (acceleration * Time.deltaTime));
            //             faceLeft = false;
            //             Flip(false);
            //         }
            //     }
            // }

            // Basic System (No Acceleration/Deceleration)
            if(!isDashing) {
                // Move left
                if(h_left) {
                    currSpeed = -maxSpeed;
                    faceLeft = true;
                    Flip(true);
                } else if(h_right){ // Move right
                    currSpeed = maxSpeed;
                    faceLeft = false;
                    Flip(false);
                } else {
                    currSpeed = 0;
                }
            }


            // Jump related
            if(c_jump && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){ // Cannot jump/drop while attacking
                if(h_down && !isDownJumpGroundCheck){
                    // Jump down from platform
                    DownJump();
                } else if(!h_down) {
                    // Jump up
                    if(currentJumpCount < maxJump){
                        PerformJump();
                        currentJumpCount++;
                    }
                }
            }
            // Reset jump counter when grounded
            if(isGrounded) {
                currentJumpCount = 0;
            } else {
                // Increment jump counter when fall from platform
                if(currentJumpCount == 0){
                    currentJumpCount = 1;
                }
            }
            
            // Attack related
            if(m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                // Is attacking 
                // TODO: Check hitbox etc.
                // Maybe add an event trigger in the animation instead
            } else {
                isAttacking = false;
                bool attackCondition = c_attack && sword;
                if(attackCondition){
                    isAttacking = true;
                    m_Anim.SetTrigger("Attack");
                    audioManager.Play("Attack");
                }
            }

            // Dash related
            if(!isDashing) {
                // Reseting dash status when dash finish
                isDashing = false;
                currSpeed = Mathf.Clamp(currSpeed, -maxSpeed, maxSpeed);

                bool dashCondition = ((doubleTapTimer > 0 && doubleTapDashEnable) ||
                                    (c_dashButton && buttonDashEnable)) &&
                                    currentJumpCount < maxJump &&
                                    boots &&
                                    !isGrounded &&
                                    !isAttacking;

                if(c_dashLeft){
                    if(dashDirection == -1 && dashCondition) {
                        // Perform left dash
                        PerformDash(true);
                        currentJumpCount += 1;
                    } else {
                        dashDirection -= 1;
                        doubleTapTimer = doubleTapInterval;
                    }
                } else if(c_dashRight) {
                    if(dashDirection == 1 && dashCondition) {
                        // Perform right dash
                        PerformDash(false);
                        currentJumpCount += 1;
                    } else {
                        dashDirection += 1;
                        doubleTapTimer = doubleTapInterval;
                    }
                } else if(c_dashButton && dashCondition){
                    PerformDash(faceLeft);
                    currentJumpCount += 1;
                }
            } else {
                // Reduce dashing time
                dashTimer -= Time.deltaTime;
                if(dashTimer <= 0){
                    dashTimer = 0;
                    isDashing = false;
                    m_Anim.SetTrigger("DashFinish");
                }
            }
            // calculate double tap interval
            if(doubleTapDashEnable){
                if(doubleTapTimer > 0) {
                    doubleTapTimer -= 1 * Time.deltaTime;
                } else {
                    doubleTapTimer = 0;
                    dashDirection = 0;
                }
            }
            // calculate dash cooldown
            // if(dashCooldownCount > 0) {
            //     dashCooldownCount -= 1 * Time.deltaTime;
            // } else {
            //     dashCooldownCount = 0;
            // }

            // Debug.Log(m_rigidbody.velocity);

            // Update animator parameter
            m_Anim.SetBool("IsRunning", h_left || h_right);
            m_Anim.SetFloat("AirSpeed", m_rigidbody.velocity.y);
            m_Anim.SetBool("IsGrounded", isGrounded);
        }
        // ApplyMovement();
    }

    void FixedUpdate(){
        ApplyMovement();
    }

    // Apply character movment
    // Reference: https://github.com/Bardent/Rigidbody2D-Slopes-Unity/blob/master/Assets/Scripts/PlayerController.cs 
    private void ApplyMovement() {
        if(isPushed){

        }
        else if(isDashing || (isGrounded && !isJumping)) {
            newVelocity.Set(currSpeed, 0.0f);
            m_rigidbody.velocity = newVelocity;
        } else if(!isGrounded){
            newVelocity.Set(currSpeed, m_rigidbody.velocity.y);
            m_rigidbody.velocity = newVelocity;
        }
    }

    public void Attack()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Weakpoint")) {
                Boss boss = enemy.gameObject.GetComponentInParent<Boss>();
                boss.Death();
            }
            else {
                Enemy enemyHit = enemy.gameObject.transform.GetComponent<Enemy>();
                enemyHit.Death();
            }
        }
    }

    private void Flip(bool faceLeft) {
        transform.localScale = new Vector3(faceLeft ? 1 : -1, 1, 1);
    }

    private void PerformDash(bool dashLeft) {
        isDashing = true;
        dashTimer = dashDuration;
        currSpeed = dashLeft ? -dashSpeed : dashSpeed;
        m_Anim.SetTrigger("Dash");
        newVelocity.Set(currSpeed, 0.0f);
        m_rigidbody.velocity = newVelocity;
    }

    public void PerformJump() {
        // m_Anim.Play("Jump");
        m_Anim.SetTrigger("Jump");
        audioManager.Play("Jump");

        isJumping = true;
        // Reset dash double tap
        dashDirection = 0;
        newVelocity.Set(0.0f, 0.0f);
        m_rigidbody.velocity = newVelocity;
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJumpCount++;
    }

    private void DownJump() {
        if (!isGrounded) {
            return;
        }

        if (!isDownJumpGroundCheck) {
            m_Anim.SetTrigger("Jump");
            m_rigidbody.AddForce(-Vector2.up * 10);
            isGrounded = false;
            m_BoxCollider.enabled = false;
            StartCoroutine(GroundBoxColliderTimmerFuc());
        }
    }

    private IEnumerator GroundBoxColliderTimmerFuc() {
        yield return new WaitForSeconds(0.3f);
        m_BoxCollider.enabled = true;
    }

    public void Death() {
        if(!isDead && !armor){
            audioManager.Play("Die");
            var collider = GetComponent<Collider2D>();
            collider.enabled = false;
            var groundSensor = GetComponentInChildren<GroundSensor>();
            groundSensor.gameObject.SetActive(false);
            isDead = true;
            currSpeed = 0.0f;
            Debug.Log("game over");
            m_Anim.SetTrigger("Death");
        }   
    }

    public void Respawn() {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().ReloadScene();
    }

    // Debug use, draw stuff in the editor while the layer is selected
    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}