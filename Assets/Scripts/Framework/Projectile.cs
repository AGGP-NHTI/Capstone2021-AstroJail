using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{
    Rigidbody rb;
    public float speed;
    public float damage;
    public float range;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = gameObject.transform.forward * speed;
        Destroy(gameObject, range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!IsServer)
        {
            return;
        }

        Actor act = other.gameObject.GetComponent<Actor>();

        if (act)
        {
            Destroy(gameObject);
        }
    }
}
