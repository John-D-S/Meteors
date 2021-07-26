using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private float bulletLifetime = 5;
    private Rigidbody2D _rigidbody2D;

    private void OnCollisionStay2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    private IEnumerator DespawnCountdown()
    {
        yield return new WaitForSeconds(bulletLifetime);
        Destroy(gameObject);
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity += (Vector2)transform.up * bulletSpeed;
        StartCoroutine(DespawnCountdown());
    }
}
