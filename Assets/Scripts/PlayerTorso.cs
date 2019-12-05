using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorso : MonoBehaviour {

    private PlayerController parent;

	void Start () {
        parent = transform.parent.gameObject.transform.parent.GetComponent<PlayerController>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy_projectile")
        {
            Projectile that = collision.gameObject.GetComponent<Projectile>();
            parent.ImHit(that);
        }
    }
}
