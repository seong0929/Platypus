using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void ToHome()
    {
        Debug.Log("Scene: Home");
        SceneManager.LoadScene("Home");
    }
    public void ToBanPicck()
    {
        Debug.Log("Scene: BanPick");
        SceneManager.LoadScene("BanPick");
    }
    public void ToBattle()
    {
        Debug.Log("Scene: Battle");
        SceneManager.LoadScene("Battle");
    }
    public void ToOption()
    {
        Debug.Log("Scene: Option");
        SceneManager.LoadScene("Option");
    }
    public void ToSelectPlayers()
    {
        Debug.Log("Scene: SelectPlayers");

        // ---- START: FOR THE TEST --- //
        GameManager gameManager;
        gameManager = FindObjectOfType<GameManager>();

        gameManager.MakeMatch();
        // ---- END: FOR THE TEST --- //

        SceneManager.LoadScene("SelectPlayers");
    }
    public void ToStrategyCoaching()
    {
        Debug.Log("Scene: StrategyCoaching");
        SceneManager.LoadScene("StrategyCoaching");
    }
    public void QuitGame()
    {
        Debug.Log("Quit game called");
        Application.Quit();
    }
}