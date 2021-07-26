using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    [SerializeField] private bool destroyObjectOnHit = true;
    [SerializeField] private string targetTag = "Player";
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            Ihittable hittable = other.gameObject.GetComponent<Ihittable>();
            if (hittable != null)
            {
                hittable.Hit();
                Destroy(gameObject);
            }
        }
    }
}
