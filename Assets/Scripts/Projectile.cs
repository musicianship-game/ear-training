using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public GameObject explosion_anim;

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        Instantiate(explosion_anim , transform.position , Quaternion.identity);
        Destroy(gameObject);
    }
}
