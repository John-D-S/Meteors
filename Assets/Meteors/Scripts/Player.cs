using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, Ihittable
{
    [SerializeField] private float thrust = 1;
    [SerializeField] private float torque = 1;

    [SerializeField] private float bulletStartPosition = 0.5f;
    [SerializeField] private float shootingCooldownTime = 0.5f;
    [SerializeField] private GameObject bullet;
    [HideInInspector] public Spawner spawner;
    
    private Rigidbody2D _rigidbody2D;
    private bool canFire = true;

    private bool hasRespawned = false;
    
    private void ThrustForward(float force)
    {
        _rigidbody2D.AddForce(transform.up * force);    
    }

    private void Turn(float torque)
    {
        _rigidbody2D.AddTorque(-torque);
    }

    public void Hit()
    {
        if (spawner && !hasRespawned)
        {
            spawner.SpawnPlayer();
            hasRespawned = true;
        }
        Destroy(gameObject);
    }
    
    private IEnumerator Fire()
    {
        canFire = false;
        GameObject instantiatedBullet = Instantiate(bullet, transform.position + transform.up * bulletStartPosition, transform.rotation);
        instantiatedBullet.GetComponent<Rigidbody2D>().velocity += _rigidbody2D.GetRelativePointVelocity(Vector2.up * bulletStartPosition);
        yield return new WaitForSeconds(shootingCooldownTime);
        canFire = true;
    } 
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    //private Vector2 shootDirection = Vector2.right;

    private void Update()
    {
        //bool isFacingInShootDirection = Vector2.Dot(transform.up, shootDirection) >= 0; 
        if (Input.GetButtonDown("Jump") && canFire == true/* || isFacingInShootDirection && canFire == true*/)
        {
            StartCoroutine(Fire());
        }
    }

    private void FixedUpdate()
    {
        ThrustForward(Mathf.Clamp01(Input.GetAxisRaw("Vertical"))* thrust);
        Turn(Input.GetAxisRaw("Horizontal") * torque);   
    }
}
