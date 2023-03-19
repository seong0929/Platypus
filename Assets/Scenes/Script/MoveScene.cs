using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void ToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void ToBanPicck()
    {
        SceneManager.LoadScene("BanPick");
    }
    public void ToBattle()
    {
        SceneManager.LoadScene("Battle");
    }
    public void ToOption()
    {
        SceneManager.LoadScene("Option");
    }
    public void ToSelectPlayers()
    {
        SceneManager.LoadScene("SelectPlayers");
    }
    public void ToStrategyCoaching()
    {
        SceneManager.LoadScene("StrategyCoaching");
    }
}