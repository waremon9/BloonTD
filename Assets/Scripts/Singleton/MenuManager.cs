using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MySingleton<MenuManager>
{
    public override bool DoDestroyOnLoad
    {
        get { return true; }
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Level1");
    }
    
    public void OnOptionsClicked()
    {
        
    }
    
    public void OnStatsClicked()
    {
        
    }
}
