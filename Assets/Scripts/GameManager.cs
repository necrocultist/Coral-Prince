using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum States
{
    Failed,
    Won,
    MainMenu, 
    Playing
}
public class GameManager : MonoBehaviour
{
    public States currentState;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private PlayerCombat pc;

    void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        currentState = States.Playing;
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.Won:
                StartCoroutine(EndScene(winPanel));
                break;
            case States.Failed :
                StartCoroutine(EndScene(losePanel));
                break;
        }
    }

    IEnumerator EndScene(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.SetActive(true);
        yield return new WaitForSeconds(2);
        currentState = States.MainMenu;
        SceneManager.LoadScene(0);
    }
}
