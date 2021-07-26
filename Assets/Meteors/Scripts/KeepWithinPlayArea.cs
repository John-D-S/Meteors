using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepWithinPlayArea : MonoBehaviour
{
    [SerializeField]
    private float maxXDistanceFromOrigin = 27;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 maxDistancefromOriginOffset = Vector3.right * maxXDistanceFromOrigin;
        Debug.DrawLine(Vector3.up * 100 + maxDistancefromOriginOffset, Vector3.down * 100 + maxDistancefromOriginOffset);
        Debug.DrawLine(Vector3.up * 100 - maxDistancefromOriginOffset, Vector3.down * 100 - maxDistancefromOriginOffset);
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.x > maxXDistanceFromOrigin)
        {
            gameObject.transform.position -= Vector3.right * (maxXDistanceFromOrigin * 2);
        }

        if (gameObject.transform.position.x < -maxXDistanceFromOrigin)
        {
            gameObject.transform.position += Vector3.right * (maxXDistanceFromOrigin * 2);
        }
    }
}
