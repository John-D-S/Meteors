using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour, Ihittable
{
    public Vector2 initialForce = Vector2.zero; 
    [HideInInspector] public Spawner spawner;
    public float sizeMultiplier = 2;
    public int size = 1;
    [SerializeField] private int splitAmount;
    public int Size
    {
        set
        {
            transform.localScale = Vector3.one * (value * sizeMultiplier);
            size = value;
        }
        get
        {
            return size;
        }
    }

    private int ScoreValue
    {
        get
        {
            return Mathf.RoundToInt(100 / size);
        }
    }
    
    private Rigidbody2D _rigidbody2D;

    private void OnValidate()
    {
        Size = size;
    }

    public void HitGround()
    {
        spawner.PlayerSpawns -= 1;
        Destroy(gameObject);
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            HitGround();
        }
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D)
        {
            _rigidbody2D.AddForce(initialForce, ForceMode2D.Impulse);
        }
        Size = size;
    }

    public void Hit()
    {
        if (size > 1)
        {
            float instantiationAngleDifference = 360 / splitAmount;
            Vector2 movementDirection = _rigidbody2D.velocity.normalized;
            List<GameObject> newMeteors = new List<GameObject>();
            for (int i = 0; i < splitAmount; i++)
            {
                Vector2 newMeteorOffset = Quaternion.Euler(0, 0, instantiationAngleDifference * i) * movementDirection * Size;
                newMeteors.Add(Instantiate(gameObject, transform.position + (Vector3)newMeteorOffset,quaternion.identity));
                Meteor newMeteorComponent = newMeteors[i].GetComponent<Meteor>();
                if (newMeteorComponent != null)
                {
                    newMeteorComponent.Size -= 1;
                    newMeteorComponent.initialForce = newMeteorOffset;
                }
            }
        }
        spawner.AddToScore(ScoreValue);
        Destroy(gameObject);
    }
}
