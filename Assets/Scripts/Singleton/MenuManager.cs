using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MySingleton<MenuManager>
{
    private bool _DoDestroyOnLoad = true;
    public override bool DoDestroyOnLoad
    {
        get { return _DoDestroyOnLoad; }
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
