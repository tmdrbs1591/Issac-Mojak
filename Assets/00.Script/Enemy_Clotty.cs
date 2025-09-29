using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Clotty : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 5f; // 총알 속도
    [SerializeField] private float attackDelay = 1f; // 1초마다 공격

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    protected override void Attack()
    {
        Debug.Log("Clotty가 공격!");

        // 4방향 벡터
        Vector2[] dirs = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (var dir in dirs)
        {
            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }
        }
    }
}
