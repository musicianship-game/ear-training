using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

    public PlayerController player;

    private BellBoss bell_boss;

    // Use this for initialization
    void Start () {
        bell_boss = GetComponentInParent<BellBoss>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Attack()
    {
        // attack animation goes here
        bell_boss.AttackConcludedSignal(this.gameObject);
    }

    public void ResonatorUpdate(bool b)
    {
        if (b)
        {
            bell_boss.BellHitSignal(this.gameObject);
        }
        else if (!b)
        {
            bell_boss.PlayerMissedSignal(this.gameObject);
        }
    }
}
