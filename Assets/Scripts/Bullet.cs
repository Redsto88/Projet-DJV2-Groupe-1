using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AProjectile
{

    public Transform target;
    
    private Vector3 destination;

    [SerializeField] private float speed = 10f;

    void Start()
    {
        if (target != null)
        {
            destination = target.position + Vector3.up;
            transform.LookAt(destination);
        }
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;   
    }

    void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name);
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.gameObject.GetComponent<ADamageable>().ApplyDamage(10f);
            Destroy(gameObject);
        }
        else if (other.gameObject.TryGetComponent<Portal>(out var portal))
        {
            //nothing
        }
        else {
            Destroy(gameObject);
        }
    }
}
