using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    public GameObject explosion_anim;
    private GameObject target = null;
    public float speed = 0.0f;

    private void Update()
    {
        if (target!=null && target.GetComponent<Enemy>().dying)
        {
            target = null;
        }
        if (target!=null)
        {
            Vector2 my_pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 traget_pos = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 direction = traget_pos - my_pos;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f);
            transform.rotation = rotation;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    public void Explode()
    {
        Instantiate(explosion_anim, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Target(GameObject the_target)
    {
        target = the_target;
        Debug.Log("target acquired: " + target.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null && collision.gameObject == target)
        {
            PlayerCloud.score += 1;
            target.GetComponent<Enemy>().GetHit();
            Explode();
        }
    }
}
