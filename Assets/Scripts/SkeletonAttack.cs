using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour {

    public float damage = 20;
    public float radius = 0.5f;
    public Vector3 localAttackPoint;

    // private

    private void AttackOnAnimEvent()
    {
        var layerMask = LayerMask.GetMask("Player");
        var playerCollider = Physics2D.OverlapCircle(GetGlobalAttackPoint(), radius, layerMask);
        if (playerCollider == null)
        {
            return;
        }

        var playerHealth = playerCollider.GetComponent<Health>();
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.Hit(damage);
    }

    private Vector3 GetGlobalAttackPoint()
    {
        var globalAttackPoint = localAttackPoint;
        globalAttackPoint.x *= transform.localScale.x;
        globalAttackPoint += transform.position;
        return globalAttackPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(GetGlobalAttackPoint(), radius);
    }
}
