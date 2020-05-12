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

class Keymap
{
    public const KeyCode HURT        = KeyCode.Alpha1;
    public const KeyCode SUICIDE     = KeyCode.Alpha2;
}

public class DbgSkeletonInput : MonoBehaviour {

    public SkeletonController skeletonController;
    public Health             skeletonHealth;
    public float              dbgDamage             = 10;
	
	// Update is called once per frame
	void Update () {
        var horizontal = Input.GetAxis(InputAxes.HORIZONTAL);
        var vertical   = Input.GetAxis(InputAxes.VERTICAL);
        var jump       = Input.GetAxis(InputAxes.JUMP);
        var attack     = Input.GetAxis(InputAxes.ATTACK);
        var hurt       = Input.GetKeyDown(Keymap.HURT);
        var suicide    = Input.GetKeyDown(Keymap.SUICIDE);

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

        if (hurt)
        {
            skeletonHealth.Hit(dbgDamage,false);
        }

        if (suicide)
        {
            skeletonHealth.Hit(99999,false);
        }
	}
}
