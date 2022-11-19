using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOverlap : MonoBehaviour
{
    public float range = 1.0f;
    public LayerMask layers;
    private Rigidbody2D playerRigidBody;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        // add layer to different objects
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range, layers);
        foreach (Collider2D collider in colliderArray)
        {
            SoulController soul = collider.GetComponent<SoulController>();
            soul.InCircle();
            /*if(collider.TryGetComponent<SoulController>(out SoulController soul))
            {
                GameObject word = soul.transform.GetChild(0).gameObject;
                word.SetActive(true);
            }*/
        }
    }
}
