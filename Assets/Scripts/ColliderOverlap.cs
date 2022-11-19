using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOverlap : MonoBehaviour
{
    public float range = 1.0f;
    //public LayerMask layers;
    private Rigidbody2D playerRigidBody;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        // Soul colliders
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Soul"));
        foreach (Collider2D collider in colliderArray)
        {
            SoulController soul = collider.GetComponent<SoulController>();
            soul.InCircle();
        }
        // Collider for gates
        Collider2D[] gateColliderArray = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Gate"));
        foreach (Collider2D gateCollider in gateColliderArray)
        {
            GateController gate = gateCollider.GetComponent<GateController>();
            gate.InCircle();
        }
    }
}
