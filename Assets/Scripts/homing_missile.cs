using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homing_missile : MonoBehaviour {

    public GameObject explosion_anim;
    private GameObject target = null;

    public void Explode()
    {
        Instantiate(explosion_anim, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Target(GameObject the_target)
    {
        target = the_target;
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {

        }
    }
}
