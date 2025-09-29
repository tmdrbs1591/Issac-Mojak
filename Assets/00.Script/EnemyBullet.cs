using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject effect;
    [SerializeField] private float damage = 1f;

    private void Start()
    {
        StartCoroutine(EffectDelay());
    }

    IEnumerator EffectDelay()
    {
        yield return new WaitForSeconds(delay);
        SpawnEffect();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽에 맞으면 효과 발생
        if (collision.CompareTag("Wall"))
        {
            SpawnEffect();
        }
        
    }

    private void SpawnEffect()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
