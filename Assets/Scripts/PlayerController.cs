using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Animator animator;
    private float speed;
    private float horizontal;
    private float vertical;
    private bool jumping;
    private bool falling;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = 5f;
    }
	
	void Update()
    {
        Vector2 translation;
        if (jumping)
        {
            translation = new Vector2(0f, 1f) * speed / 2f;
        }
        else if (falling)
        {
            translation = new Vector2(0f, -1f) * speed / 2f;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            translation = new Vector2(horizontal, vertical) * speed;
            if (translation.magnitude > 0f)
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }        
        rb2d.MovePosition(rb2d.position + translation * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.gameObject.tag == "JumpZone")
        {            
            if (vertical > 0f)
            {
                Debug.Log("Jumping!");
                jumping = true;
                falling = false;
            }
            else if (vertical < 0f)
            {
                Debug.Log("Falling!");
                jumping = false;
                falling = true;
            }                       
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpZone")
        {
            Debug.Log("Back To Earth!");
            if (jumping || falling)
            {
                jumping = false;
                falling = false;
            }
        }
    }
}
