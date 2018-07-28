using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

    public Rigidbody2D      rigidBody2d;
    public Animator         animator;
    public Health           health;

    public float            speed                       = 3;
    public float            jumpSpeed                   = 12;
    public float            lowJumpMultiplier           = 2;
    public float            fallMultiplier              = 2.5f;
    public float            destroyAfterDeathDelay       = 5;

    public void MoveLeft()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.MOVE);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = -1.0f;
            FaceLeft();
        }
    }

    public void MoveRight()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.MOVE);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = 1.0f;
            FaceRight();
        }
    }

    public void StopMoving()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.STOP);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = 0.0f;
        }
    }

    public void Jump()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.JUMP);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            rigidBody2d.velocity += Vector2.up * jumpSpeed;
        }
    }

    public void Attack()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.ATTACK);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = 0.0f;
        }
    }

    #region private

    private float horizontal = 0.0f;
    private bool isFacingRight = true;
    private SkeletonState currentState = SkeletonState.IdleState;

    private void Start()
    {
        health.OnHit += Hit;
        health.OnDeath += Die;
    }

    private void Update()
    {
        var newState = currentState.Update(this);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
        }
    }

    private void FixedUpdate()
    {
        JumpFaster();
        rigidBody2d.velocity = new Vector2(speed * horizontal, rigidBody2d.velocity.y);
    }

    private void FaceLeft()
    {
        if (!isFacingRight)
        {
            return;
        }
        Flip();
    }

    private void FaceRight()
    {
        if (isFacingRight)
        {
            return;
        }
        Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void JumpFaster()
    {
        if (rigidBody2d.velocity.y > 0)
        {
            rigidBody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody2d.velocity.y < 0)
        {
            rigidBody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Hit()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.HIT);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = 0.0f;
        }
    }

    private void Die()
    {
        var newState = currentState.HandleInput(this, SkeletonCommand.DEATH);
        if (newState != null)
        {
            currentState = newState;
            currentState.Enter(this);
            horizontal = 0.0f;
            Destroy(gameObject, destroyAfterDeathDelay);
        }
    }

    #endregion // private
}

enum SkeletonCommand
{
    MOVE,
    STOP,
    JUMP,
    ATTACK,
    HIT,
    DEATH
};

abstract class SkeletonState
{
    public abstract void Enter(SkeletonController skeleton);
    public abstract SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command);

    public virtual SkeletonState Update(SkeletonController skeleton) { return null; }

    public static SkeletonState IdleState = new SkeletonStateIdleState();
    public static SkeletonState MoveState = new SkeletonStateMoveState();
    public static SkeletonState JumpState = new SkeletonStateJumpState();
    public static SkeletonState AttackState = new SkeletonStateAttackState();
    public static SkeletonState HitState = new SkeletonStateHitState();
    public static SkeletonState DeathState = new SkeletonStateDeathState();
}

class SkeletonStateIdleState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
        skeleton.animator.SetBool("IsMoving", false);
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        switch(command)
        {
            case SkeletonCommand.MOVE:
                return MoveState;
            case SkeletonCommand.JUMP:
                return JumpState;
            case SkeletonCommand.ATTACK:
                return AttackState;
            case SkeletonCommand.HIT:
                return HitState;
            case SkeletonCommand.DEATH:
                return DeathState;
        }

        return null;
    }
}

class SkeletonStateMoveState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
        skeleton.animator.SetBool("IsMoving", true);
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        switch (command)
        {
            case SkeletonCommand.STOP:
                return IdleState;
            case SkeletonCommand.JUMP:
                return JumpState;
            case SkeletonCommand.ATTACK:
                return AttackState;
            case SkeletonCommand.HIT:
                return HitState;
            case SkeletonCommand.DEATH:
                return DeathState;
        }

        return null;
    }
}

class SkeletonStateJumpState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        switch (command)
        {
            case SkeletonCommand.HIT:
                return HitState;
            case SkeletonCommand.DEATH:
                return DeathState;
        }

        if (!IsOnGround(skeleton.rigidBody2d))
        {
            return null;
        }

        return IdleState;
    }

    private bool IsOnGround(Rigidbody2D rigidbody)
    {
        return rigidbody.velocity.y == 0;
    }
}

class SkeletonStateAttackState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
        skeleton.animator.SetBool("IsAttack", true);
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        switch (command)
        {
            case SkeletonCommand.HIT:
                return HitState;
            case SkeletonCommand.DEATH:
                return DeathState;
        }

        return null;
    }

    public override SkeletonState Update(SkeletonController skeleton)
    {
        if (skeleton.animator.GetBool("IsAttack"))
        {
            return null;
        }
        return IdleState;
    }
}

class SkeletonStateHitState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
        skeleton.animator.SetBool("IsHit", true);
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        if (command == SkeletonCommand.DEATH)
        {
            return DeathState;
        }
        return null;
    }

    public override SkeletonState Update(SkeletonController skeleton)
    {
        if (skeleton.animator.GetBool("IsHit"))
        {
            return null;
        }
        return IdleState;
    }
}

class SkeletonStateDeathState : SkeletonState
{
    public override void Enter(SkeletonController skeleton)
    {
        skeleton.animator.SetBool("IsDead", true);
    }

    public override SkeletonState HandleInput(SkeletonController skeleton, SkeletonCommand command)
    {
        return null;
    }
}