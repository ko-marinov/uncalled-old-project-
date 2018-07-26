using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour {

    private float HP = 100;

    public void Hit(float damage){
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
