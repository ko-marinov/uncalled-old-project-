using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

    public Rigidbody2D      rigidBody2d;
    public Animator         animator;
    public float            speed;
    public float            requeredDistance;

    public void MoveTo(Vector2 position)
    {
        targetPosition = position;
        StartMoving();
    }

    public void Attack()
    {

    }

    void Start()
    {
        DbgStartMove();
    }

	void FixedUpdate()
    {
        if (!isMoving)
        {
            return;
        }

        var distance = targetPosition.x - transform.position.x;
        if (Mathf.Abs(distance) <= requeredDistance)
        {
            Debug.Log("Destination reached");
            StopMoving();
            DbgRestartMove();
        }

        var xDirection = distance >= 0 ? 1 : -1;
        var movement = new Vector2(xDirection * speed, 0);
        rigidBody2d.AddForce(movement);
    }

    #region private

    private Vector2 targetPosition;
    private bool isMoving = false;

    private void StartMoving()
    {
        isMoving = true;
        Debug.Log("Start moving");
    }

    private void StopMoving()
    {
        isMoving = false;
        Debug.Log("Stop moving");
    }

    #endregion // private

    #region debug

    private void DbgStartMove()
    {
        MoveTo(new Vector2(5, 0));
    }

    private void DbgRestartMove()
    {
        var newPosition = targetPosition;
        newPosition.Scale(new Vector2(-1, 0));
        MoveTo(newPosition);
    }

    #endregion // debug
}
