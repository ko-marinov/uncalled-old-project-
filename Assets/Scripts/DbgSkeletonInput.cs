using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InputAxes
{
    public const string HORIZONTAL  = "Horizontal";
    public const string VERTICAL    = "Vertical";
    public const string JUMP        = "Jump";
    public const string ATTACK      = "Fire1";
}

public class DbgSkeletonInput : MonoBehaviour {

    public SkeletonController skeletonController;
	
	// Update is called once per frame
	void Update () {
        var horizontal = Input.GetAxis(InputAxes.HORIZONTAL);
        var vertical   = Input.GetAxis(InputAxes.VERTICAL);
        var jump       = Input.GetAxis(InputAxes.JUMP);
        var attack     = Input.GetAxis(InputAxes.ATTACK);

        if(horizontal > 0)
        {
            skeletonController.MoveRight();
        }
        else if (horizontal < 0)
        {
            skeletonController.MoveLeft();
        }
        else
        {
            skeletonController.StopMoving();
        }

        if (jump > 0)
        {
            skeletonController.Jump();
        }

        if (attack > 0)
        {
            skeletonController.Attack();
        }
	}
}
