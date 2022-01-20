using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public BaseTower towerPrefab;

    public void ClickOnButton()
    {
        if (UIManager.Instance.selectedTower != null) return;

        if (towerPrefab)
        {
            UIManager.Instance.selectTowerInShop(towerPrefab);
        }
        else
        {
            Debug.LogError("No tower prefab on button "+ name);
        }
    }
}
