using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MySingleton<UIManager>
{
    public override bool DoDestroyOnLoad { get; }

    [HideInInspector] public BaseTower selectedTower;
    [SerializeField] private Text healthText;
    [SerializeField] private Text moneyText;

    private void Update()
    {
        if (selectedTower)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedTower.transform.position = new Vector3(position.x, position.y, 0);
            
            selectedTower.SetCollisionColor(Physics2D.OverlapCircleAll(selectedTower.transform.position, selectedTower.hitBox.radius).Length > 1);
        }
        
        if (Input.GetMouseButtonDown(0) && selectedTower)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Destroy(selectedTower.gameObject);
                selectedTower = null;
            }
            else
            {
                if (Physics2D.OverlapCircleAll(selectedTower.transform.position, selectedTower.hitBox.radius).Length > 1) return;
                
                selectedTower.enabledTower = true;
                selectedTower.EnableRangeIndicator(false);
                GameManager.Instance.SpendMoney(selectedTower.cost);
                
                selectedTower = null;
            }
        }
    }

    public void selectTowerInShop(BaseTower prefab)
    {
        if (!GameManager.Instance.HasEnoughMoney(prefab.cost)) return;
        
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedTower = Instantiate(prefab, new Vector3(position.x, position.y, 0), Quaternion.identity, GameManager.Instance.towerParent);
        
        selectedTower.EnableRangeIndicator(true);
        selectedTower.enabledTower = false;
    }

    public void UpdateHealth(int value)
    {
        healthText.text = "x " + value.ToString();
    }

    public void UpdateMoney(int value)
    {

        moneyText.text = "x " + value.ToString();
    }
}
