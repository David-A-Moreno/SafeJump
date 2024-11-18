using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField]
    private GameObject gameStartUI;

    [SerializeField]
    private AudioSource music;

    private static bool firstGame = true;

    public void showGameStartUI()
    {
        if (firstGame)
        {
            gameStartUI.SetActive(true);
            music.volume = 0.05f;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
