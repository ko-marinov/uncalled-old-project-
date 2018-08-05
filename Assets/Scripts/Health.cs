using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float maxHp = 100;

    public delegate void Health0Args();
    public event Health0Args OnHit;
    public event Health0Args OnDeath;
    public delegate bool Health1Args();
    public event Health1Args IsBlocked;

    public Image healthBar;

    public void Start()
    {
        currentHp = maxHp;
    }

    public void Hit(float damage, bool inBlock){
        if (IsDead())
        {
            return;
        }
        if (inBlock && IsBlocked())
            damage /= 10;
        currentHp -= damage;
        healthBar.fillAmount = currentHp / maxHp;
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

    public void Heal(float ammount)
    {
        currentHp += ammount;
        if (currentHp > maxHp)
            currentHp = maxHp;
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
