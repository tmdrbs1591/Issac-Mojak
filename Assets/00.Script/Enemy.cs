using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour, Damaged
{
    [SerializeField] protected float currentHP;
    [SerializeField] protected float maxHP;

    public SpriteRenderer spriteRenderer;
    abstract protected void Attack();

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        StartCoroutine(Hit());

        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
