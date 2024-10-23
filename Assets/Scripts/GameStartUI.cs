using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField]
    private GameObject gameStartUI;

    private static bool firstGame = true;

    public void showGameStartUI()
    {
        if (firstGame)
        {
            gameStartUI.SetActive(true);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log(firstGame);
    }

    public void setFirstGame (bool first)
    {
        firstGame = first;
    }

    public bool getFirstGame()
    {
        return firstGame;
    }
}
