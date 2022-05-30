using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
   private enum State
    {
        Walk,
        Knockback,
        Dead
    }

    private State currentState;

    [SerializeField]
    private float
        groundCheckDist,
        wallCheckDist,
        moveSpeed,
        maxHP,
        knockbackDuration,
        lastDamageTime,
        damageCooldown,
        damage,
        damagewidth,
        damageheight;

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck,
        damageCheck,
        playerCheck;

    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField]
    private Vector2 knockbackSpeed;


    private int 
        facingDirection,
        damageDirection;

    private float[] attackDetails = new float[2];

    private float 
        currentHP,
        knockbackStartTime
        ;

    private Vector2 move,
        damageBotLeft,
        damageTopRight;
    

    private bool 
        isGrounded,
        isWall
        ;

    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        facingDirection = 1;
        currentHP = maxHP;
        aliveAnim = alive.GetComponent<Animator>();
       
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Walk:
                UpdateWalk();
                break;

            case State.Knockback:
                UpdateKnockback();
                break;

            case State.Dead:
                UpdateDead();
                break;
        }

    }

    //Состояние ходьбы 
    private void EnterWalk()
    {

    }

    private void UpdateWalk()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, whatIsGround);
        isWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDist, whatIsGround);

        CheckDamage();

        if(!isGrounded || isWall)
        {
            Flip();
        }
        else
        {
            move.Set(moveSpeed * facingDirection, aliveRb.velocity.y);

            aliveRb.velocity = move;
        }
    }

    private void ExitWalk()
    {

    }

    //Состояние урона
    private void EnterKnockback()
    {
        knockbackStartTime = Time.time;
        move.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        
        aliveRb.velocity = move;
        aliveAnim.SetBool("Knockback", true);
    }

    private void UpdateKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwichState(State.Walk);
        }
    }

    private void ExitKnockback()
    {
        aliveAnim.SetBool("Knockback", false);
    }

    //Состояние cмерти
    private void EnterDead()
    {
        Destroy(gameObject);
    }

    private void UpdateDead()
    {

    }

    private void ExitDead()
    {

    }

    //Другие ф-ии
    private void SwichState(State state)
    {
        switch (currentState)
        {
            case State.Walk:
                ExitWalk();
                break;

            case State.Knockback:
                ExitKnockback();
                break;

            case State.Dead:
                ExitDead();
                break;
        }

        switch (state)
        {
            case State.Walk:
                EnterWalk();
                break;

            case State.Knockback:
                EnterKnockback();
                break;

            case State.Dead:
                EnterDead();
                break;
        }
        currentState = state;
    }

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void Damage(float[] attackDetails)
    {
        currentHP -= attackDetails[0];
        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else 
        {
            damageDirection = 1;
        }

        if(currentHP > 0.0f)
        {
            SwichState(State.Knockback);

        }
        else if (currentHP <= 0.0f)
        {
            SwichState(State.Dead);
        }
    }

    private void CheckDamage ()
    {
        if(Time.time >= lastDamageTime + damageCooldown)
        {
            damageBotLeft.Set(damageCheck.position.x - (damagewidth / 2), damageCheck.position.y - (damageheight / 2));
            damageTopRight.Set(damageCheck.position.x + (damagewidth / 2), damageCheck.position.y + (damageheight / 2));

            Collider2D hit = Physics2D.OverlapArea(damageBotLeft, damageTopRight, whatIsPlayer);
            if (hit != null) 
            {
                lastDamageTime = Time.time;
                attackDetails[0] = damage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

   


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDist));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x, wallCheck.position.y - wallCheckDist));

        Vector2 botLeft = new Vector2(damageCheck.position.x - (damagewidth / 2), damageCheck.position.y - (damageheight / 2));
        Vector2 botRight = new Vector2(damageCheck.position.x + (damagewidth / 2), damageCheck.position.y - (damageheight / 2));
        Vector2 topLeft = new Vector2(damageCheck.position.x - (damagewidth / 2), damageCheck.position.y + (damageheight / 2));
        Vector2 topRight = new Vector2(damageCheck.position.x + (damagewidth / 2), damageCheck.position.y + (damageheight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
    
}
