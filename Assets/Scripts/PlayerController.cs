using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public enum CHAR_STATE { idle, walk, Jump, Attack, Block };

    private bool isGrounded = false;
    private Rigidbody2D     heroRigidbody;    
    private SpriteRenderer  spriteRenderer;
    private Animator        animator;
       
    // Use this for initialization
    void Start() {
        heroRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }
    void FixedUpdate()
    {
        CheckGround();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(State);
        if(isGrounded) State = CHAR_STATE.idle;
        if (isGrounded && Input.GetKey(KeyCode.C)) Block();       
        if (State != CHAR_STATE.Block && Input.GetButton("Horizontal")) Walk();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetMouseButtonDown(0)) Attack(); 
        
    }

    private void Attack()
    {
        State = CHAR_STATE.Attack;
    }

    private void Block()
    {
        State = CHAR_STATE.Block;
    }

    private void Walk()
    {
        if(isGrounded)
            State = CHAR_STATE.walk;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, GetSpeed() * Time.deltaTime);
        spriteRenderer.flipX = dir.x < 0.0F;
       
    }

    private float GetSpeed()
    {
        return Input.GetKey(KeyCode.LeftShift)? speed * 2 : speed;
    }

    private void Jump()
    {
        State = CHAR_STATE.Jump;
        heroRigidbody.velocity = new Vector2(0, 0);
        heroRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.05F);
        isGrounded = colliders.Length > 1 ;
        if (!isGrounded)
            State = CHAR_STATE.Jump;
    }

    private CHAR_STATE State
    {
        get { return (CHAR_STATE)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
}
