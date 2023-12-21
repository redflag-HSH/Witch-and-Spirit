using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ElementType ElementType;
    public float speed;
    public Entity Owner;
    public int Damage = 1;
    public float DestroyDistance = 50f;
    public float DistanceCheckTime = 1f;
    public bool DestroyOnCollision = true;

    [HideInInspector] public Vector3 CameraOffset;


    protected virtual void Update()
    {
        transform.position += Time.deltaTime * speed * transform.forward;

        var movement = Time.deltaTime * 6f;

        transform.position += -CameraOffset * movement;
        CameraOffset *= (1 - movement);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        print("Trigger " + other.gameObject);
        if (other.TryGetComponent(out Entity ent) && (Owner is Player) != (ent is Player))
        {
            ent.Damage(Damage, ElementType);
            if(DestroyOnCollision) Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
    }
}
