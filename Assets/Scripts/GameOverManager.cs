using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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

    [SerializeField]
    private LilyPadManager lilyPadManager;

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

    public void SaveMetricsToJSON()
    {
        // Obtener las métricas
        GameMetrics metrics = new GameMetrics
        {
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            lostLevel = progressiveBuild.GetLostLevel(),
            nogoOptionsAvoided = progressiveBuild.nogoOptionsAvoided,
            bestGoStreak = progressiveBuild.GetBestGoStreak()
        };

        // Convertir el objeto en una cadena JSON
        string json = JsonUtility.ToJson(metrics, true);

        // Definir la ruta donde guardar el archivo
        string path = Path.Combine(Application.persistentDataPath, "gameMetrics.json");

        // Guardar el archivo
        File.AppendAllText(path, json + "\n");

        Debug.Log("Métricas guardadas en: " + path);
    }

    public void GameOver(Vector3 position, bool timeLimitReached)
    {
        gameStartUI.setFirstGame(false);
        GameOverUI.SetActive(true);

        textPoints.text = points.ToString();

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
        SaveMetricsToJSON();
    }
}