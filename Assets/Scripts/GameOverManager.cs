using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverUI;

    public void PlayAgain()
    {
        // Vuelve a cargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(Vector3 position, bool timeLimitReached)
    {
        GameOverUI.SetActive(true);
        Vector3 uiPosition;
        if (timeLimitReached)
        {
            uiPosition = new(position.x + 0.28f, 2.97f, 0.12f);
        }
        else
        {
            uiPosition = new(position.x - 11, 2.97f, 0.12f);
        }
        GameOverUI.transform.position = uiPosition;

    }
}