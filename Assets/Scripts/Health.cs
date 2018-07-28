using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHp = 100;

    public delegate void Health0Args();
    public event Health0Args OnHit;
    public event Health0Args OnDeath;

    public void Start()
    {
        currentHp = maxHp;
    }

    public void Hit(float damage){
        if (IsDead())
        {
            return;
        }

        currentHp -= damage;
        if (currentHp > 0)
        {
            OnHit();
        }
        else
        {
            currentHp = 0;
            OnDeath();
        }
    }

    public bool IsAlive()
    {
        return currentHp > 0;
    }

    public bool IsDead()
    {
        return !IsAlive();
    }

    // private

    private float currentHp;
}
