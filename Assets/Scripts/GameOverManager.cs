using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverUI;

    [SerializeField]
    private ProgressiveBuild progressiveBuild;

    [SerializeField]
    private GameStartUI gameStartUI;

    [SerializeField]
    private TMP_Text textPoints;

    private int points = 0;

    public void increasePoints(int numberOfPoints)
    {
        points += numberOfPoints;
    }

    public int GetPoints() => points;

    public void PlayAgain()
    {
        // Vuelve a cargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(Vector3 position, bool timeLimitReached)
    {
        gameStartUI.setFirstGame(false);
        GameOverUI.SetActive(true);

        textPoints.text = "Obtuviste " + points + " puntos";

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