using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Attack();
        }
    }

    void Attack() {
        // Play attack animation
        animator.SetTrigger("Attacking");

        // detect whats in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange, enemyLayers
        );

        // damage them
        foreach(Collider2D enemy in hitEnemies) {
            Debug.Log("We hit" + enemy.name);
        }
    }

    void OnDrawGizmosSelected(){
        if (attackPoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
// https://youtu.be/sPiVz1k-fEs?t=786 melee combat tutorial
