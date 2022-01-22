using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    public override bool DoDestroyOnLoad { get; }

    [Header("Parent GameObject")]
    public Transform particleParent;
    public Transform towerParent;
    
    [Header("Color")]
    public SingleColorScriptable noCollisionColor;
    public SingleColorScriptable collisionColor;

    [Header("Money and life")]
    [SerializeField] private int health = 10000;
    [SerializeField] private int money = 90000;

    [SerializeField] private UIManager ui;
    
    private void Start()
    {
        if (!ui)
        {
            ui = GameObject.FindObjectOfType<UIManager>();
            if (!ui)
            {
                Debug.Log("No UIManager in scene");
            }
        }
        else
        {
            ui.UpdateHealth(health);
            ui.UpdateMoney(money);
        }
    }

    public void LooseHealth(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, 9999999);
        
        ui.UpdateHealth(health);
        
        if (health == 0)
        {
            Debug.Log("YOU'RE DEAD !!!");
        }
    }

    public bool HasEnoughMoney(int amount)
    {
        return money >= amount;
    }

    public void AddMoney()
    {
        money++;
        ui.UpdateMoney(money);
    }

    public void EndOfWaveMoneyReward()
    {
        money += 100 + EnemiesManager.Instance.waveNumber;
        ui.UpdateMoney(money);
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        ui.UpdateMoney(money);
    }
}
