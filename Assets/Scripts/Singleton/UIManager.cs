using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MySingleton<UIManager>
{
    public override bool DoDestroyOnLoad { get; }

    [HideInInspector] public BaseTower towerFromShop;

    private BaseTower selectedTower;
    
    [SerializeField] private Text healthText;
    [SerializeField] private Text moneyText;

    private void Update()
    {
        if (towerFromShop)
        {
            UpdateTowerFromShopPositionAndCollisionColor();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (towerFromShop)
            {
                PlaceOrDestroyTowerFromShop();
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                CheckClickOnTower();
            }
            else
            {
                UnselectTower();
            }
            
        }
    }

    private void UpdateTowerFromShopPositionAndCollisionColor()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerFromShop.transform.position = new Vector3(position.x, position.y, 0);
            
        towerFromShop.SetCollisionColor(Physics2D.OverlapCircleAll(towerFromShop.transform.position, towerFromShop.hitBox.radius).Length > 1);
    }

    private void PlaceOrDestroyTowerFromShop()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Destroy(towerFromShop.gameObject);
            towerFromShop = null;
        }
        else
        {
            if (Physics2D.OverlapCircleAll(towerFromShop.transform.position, towerFromShop.hitBox.radius).Length > 1) return;
                
            towerFromShop.enabledTower = true;
            GameManager.Instance.SpendMoney(towerFromShop.cost);

            selectedTower = towerFromShop;
            towerFromShop = null;
        }
    }

    private void CheckClickOnTower()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit)
        {
            if (hit.transform.CompareTag("Tower"))
            {
                SelectTower(hit.transform.GetComponent<BaseTower>());
            }
            else
            {
                UnselectTower();
            }
        }
        else if (selectedTower)
        {
            UnselectTower();
        }
    }

    private void UnselectTower()
    {
        if (!selectedTower) return;
        
        selectedTower.EnableRangeIndicator(false);
        selectedTower = null;
    }

    private void SelectTower(BaseTower tower)
    {
        UnselectTower();
        selectedTower = tower;
        selectedTower.EnableRangeIndicator(true);
    }

    public void selectTowerInShop(BaseTower prefab)
    {
        if (!GameManager.Instance.HasEnoughMoney(prefab.cost)) return;
        
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerFromShop = Instantiate(prefab, new Vector3(position.x, position.y, 0), Quaternion.identity, GameManager.Instance.towerParent);
        
        towerFromShop.EnableRangeIndicator(true);
        towerFromShop.enabledTower = false;
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
