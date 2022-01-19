using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    public override bool DoDestroyOnLoad { get; }

    [Header("Parent GameObject")]
    public Transform particleParent;

    [Header("Money and life")]
    [SerializeField] private int health;
    [SerializeField] private int money;

    public int GetHealth()
    {
        return health;
    }
    public void LooseHealth(int damage)
    {
        Debug.Log(damage);
        
        health = Mathf.Clamp(health - damage, 0, 999);
        if (health == 0)
        {
            Debug.Log("YOU'RE DEAD !!!");
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public bool HasEnoughMoney(int amount)
    {
        return money >= amount;
    }

    public void AddMoney()
    {
        money++;
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
    }
}
