using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOfFortuneSector : MonoBehaviour {

    public virtual void Activate()
    {
        Debug.Log("Sector " + this.ToString() + " activated");
    }
}
