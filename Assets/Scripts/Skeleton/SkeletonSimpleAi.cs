using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSimpleAi : MonoBehaviour {

    public float distanceToAttack = 1;
    public float xMinDistanceToStop = 0.2f;
    public float yMaxDistanceToStop = 2;

    public SkeletonController skeletonController;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            return;
        }

        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);

        if (Math.Abs(myPosition.y - playerPosition.y) > yMaxDistanceToStop)
        {
            skeletonController.StopMoving();
            return;
        }

        if (Vector2.Distance(myPosition, playerPosition) <= distanceToAttack)
        {
            skeletonController.Attack();
            return;
        }

        if (Math.Abs(myPosition.x - playerPosition.x) < xMinDistanceToStop)
        {
            skeletonController.StopMoving();
        }
        else if (myPosition.x > playerPosition.x)
        {
            skeletonController.MoveLeft();
        }
        else if(myPosition.x < playerPosition.x)
        {
            skeletonController.MoveRight();
        }
	}
}
