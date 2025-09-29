using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tears : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject effect;

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
        if (collision.CompareTag("Wall"))
            SpawnEffect();
    }
    private void SpawnEffect()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
} 
