using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private GameManager GM;


    private void Start()
    {
        currentHP = maxHP;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void minusHP(float amount)
    {
        currentHP -= amount;
        if(currentHP <= 0.0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GM.Respawn();
        Destroy(gameObject);

       
    }
}
