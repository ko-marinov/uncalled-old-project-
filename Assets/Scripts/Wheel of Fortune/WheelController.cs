using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour {

    public GameObject wheel;

    public float maxAngularSpeed;
    public float angularAcceleration;
    public float angularDeceleration;

    public void Roll()
    {
        if(currentState != WHEEL_STATE.STOPPED)
        {
            return;
        }

        currentState = WHEEL_STATE.ACCELERATING;
    }

    public void Stop()
    {
        if(currentState == WHEEL_STATE.STOPPED || currentState == WHEEL_STATE.DECELERATING)
        {
            return;
        }

        currentState = WHEEL_STATE.DECELERATING;
    }

    //---------------------------{ PRIVATE }---------------------------//

    private enum WHEEL_STATE { STOPPED, ACCELERATING, DECELERATING, ROLL }

    private WHEEL_STATE currentState = WHEEL_STATE.STOPPED;
    private float       currentSpeed = 0;

    void Update ()
    {
        var isRoll = Input.GetKeyDown(KeyCode.Alpha9);
        var isStop = Input.GetKeyDown(KeyCode.Alpha0);
        if (isRoll)
        {
            Roll();
        } else if (isStop)
        {
            Stop();
        }

		switch(currentState)
        {
            case WHEEL_STATE.STOPPED:
                return;
            case WHEEL_STATE.ACCELERATING:
                currentSpeed += angularAcceleration * Time.deltaTime;
                if (currentSpeed >= maxAngularSpeed)
                {
                    currentSpeed = maxAngularSpeed;
                    currentState = WHEEL_STATE.ROLL;
                }
                break;
            case WHEEL_STATE.DECELERATING:
                currentSpeed -= angularDeceleration * Time.deltaTime;
                if(currentSpeed <= 0)
                {
                    currentSpeed = 0;
                    currentState = WHEEL_STATE.STOPPED;
                }
                break;
            case WHEEL_STATE.ROLL:
                break;
        }

        wheel.transform.Rotate(0, 0, -currentSpeed * Time.deltaTime);
	}
}
