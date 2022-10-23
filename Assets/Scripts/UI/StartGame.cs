using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject crew;
    [SerializeField] private GameObject story;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCast()
    {
        crew.SetActive(true);
    }

    public void CloseCast()
    {
        crew.SetActive(false);
    }  
    public void OpenStory()
    {
        story.SetActive(true);
    }

    public void CloseStory()
    {
        story.SetActive(false);
    }
}