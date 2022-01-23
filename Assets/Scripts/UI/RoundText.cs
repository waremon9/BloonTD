using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text.enabled = false;
    }

    public IEnumerator AnimateRoundText()
    {
        text.enabled = true;
        
        text.text = "ROUND " + EnemiesManager.Instance.waveNumber;

        int charsQte = text.textInfo.characterInfo.Length;
        int begin = 0;
        int end = 0;
        text.firstVisibleCharacter = 0;

        while (end < charsQte)
        {
            end++;
            text.maxVisibleCharacters = end;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);
        
        while (begin < charsQte)
        {
            begin++;
            text.firstVisibleCharacter = begin;
            yield return new WaitForSeconds(0.1f);
        }

        text.enabled = false;
    }
}
