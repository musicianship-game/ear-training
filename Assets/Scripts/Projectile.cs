using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;

    public void explode()
    {
        Destroy(this);
    }

}
