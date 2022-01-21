using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveSprite : MonoBehaviour
{
    private List<GameObject> tacksRenderer = new List<GameObject>();
    public SpriteRenderer TackSpriteRenderer;
    public Vector2 minMaxX;
    public Vector2 minMaxY;
    
    public void AddSprite(int qte)
    {
        tacksRenderer.Add(TackSpriteRenderer.gameObject);
        for (int i = 0; i < qte-1; i++)
        {
            float randX = Random.Range(minMaxX.x, minMaxX.y);
            float randY = Random.Range(minMaxY.x, minMaxY.y);
            Vector3 pos = new Vector3(randX, randY) + transform.position;
            SpriteRenderer temp = Instantiate(TackSpriteRenderer, pos, Quaternion.identity, TackSpriteRenderer.transform.parent);
            temp.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            
            tacksRenderer.Add(temp.gameObject);
        }
        TackSpriteRenderer.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    public void RemoveSprite()
    {
        if (tacksRenderer.Count < 1)
        {
            Debug.Log("empty tack pile not destroyed"  + GetComponent<TackPile>().tackQte);
            return;
        }
        GameObject temp = tacksRenderer[0];
        tacksRenderer.RemoveAt(0);
        temp.SetActive(false);
    }
}
