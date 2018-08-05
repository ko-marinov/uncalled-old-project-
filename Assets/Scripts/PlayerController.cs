using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public Transform attack;
    public Transform blockZone;
    public float attackRadius;
    public Health health;
    public enum CHAR_STATE { idle, walk, Jump, Attack, Block, Hurt };

   // private HeroAnimatorCont heroAnimatorCont;
    private bool            isGrounded = false;
    private Rigidbody2D     heroRigidbody;    
    private SpriteRenderer  spriteRenderer;
    private Animator        animator;
    private bool            isHitten = false;

    // Use this for initialization
    void Start() {
        health.OnHit += Hurt;
        health.OnDeath += Death;
        health.IsBlocked += IsBlock;
        //heroAnimatorCont = GetComponentInChildren<HeroAnimatorCont>();
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
        if (isGrounded && !isHitten)
            State = CHAR_STATE.idle;
        if (isGrounded && Input.GetKey(KeyCode.C)) Block();       
        if (State != CHAR_STATE.Block && Input.GetButton("Horizontal")) Walk();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetMouseButtonDown(0)) Attack();        
    }

    private void Attack()
    {
       State = CHAR_STATE.Attack;
    }

    private void Hurt()
    {
        //Debug.Log("Hurt");
        if(!IsBlock())
            StartCoroutine(recoveryAnim());
        State = CHAR_STATE.Hurt;
        //Debug.Log(State);
    }

    IEnumerator recoveryAnim()
    {
        isHitten = true;
        yield return new WaitForSeconds(0.1f);
        isHitten = false;
        //Debug.Log(isHitten);
    }

    private void Death()
    {
        health.Heal(100);
    }

    private bool IsBlock()
    {
        return State == CHAR_STATE.Block;
    }

    private void Block()
    {
        blockZone.GetComponent<BoxCollider2D>().enabled = Input.GetKey(KeyCode.C);
        State = CHAR_STATE.Block;
    }

    private void Walk()
    {
        if(isGrounded)
            State = CHAR_STATE.walk;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, GetSpeed() * Time.deltaTime);
        spriteRenderer.flipX = dir.x < 0.0F;
        //TODO Переписать отзеркаливание по оси Х области поражения. 
        if(dir.x < 0.0F && attack.localPosition.x > 0 || dir.x > 0.0F && attack.localPosition.x < 0)
            attack.localPosition = new Vector3( -attack.localPosition.x , attack.localPosition.y, attack.localPosition.z);
        if (dir.x < 0.0F && blockZone.localPosition.x > 0 || dir.x > 0.0F && blockZone.localPosition.x < 0)
            blockZone.localPosition = new Vector3(-blockZone.localPosition.x, blockZone.localPosition.y, blockZone.localPosition.z);
        //----------------------------------------
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
