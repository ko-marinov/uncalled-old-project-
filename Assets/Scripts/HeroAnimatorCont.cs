using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimatorCont : MonoBehaviour {

    public Transform attack;

    void hitTarget()
    {
        var enemyLayer = LayerMask.NameToLayer("Enemy");
        Fight2D.Action(attack.position, 0.66f, enemyLayer, 35, false);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }


}
