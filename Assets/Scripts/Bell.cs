using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

    public PlayerController player;
    public float frequency = 0;
    public bool targetable = false;
    public float attack_dur = 1.5f;
    public float attack_start;

    private BellBoss bell_boss;
    private ThisTarget this_target;
    private bool attacking = false;

    // Use this for initialization
    void Start () {
        bell_boss = GetComponentInParent<BellBoss>();
        this_target = GetComponent<ThisTarget>();
        this_target.frequency = frequency;
        this_target.targetable = targetable;
    }
	
	// Update is called once per frame
	void Update () {
        this_target.targetable = targetable;
        if (attacking)
        {
            if (Time.time >= attack_start + attack_dur)
            {
                attacking = false;
                bell_boss.AttackConcludedSignal(this.gameObject);
            }
        }
    }

    void Attack()
    {
        attack_start = Time.time;
        attacking = true;
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
