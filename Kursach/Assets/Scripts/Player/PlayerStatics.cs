using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void minusHP(float amount)
    {
        currentHP -= amount;
        if(currentHP <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
