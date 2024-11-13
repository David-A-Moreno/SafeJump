using System.Collections.Generic;
using UnityEngine;

public class LilyPadManager : MonoBehaviour
{
    private Dictionary<int, List<Vector3[]>> lilyPadPositions = new Dictionary<int, List<Vector3[]>>()
    {
        // Opciones de lilyPads con 4 posiciones
        [4] = new List<Vector3[]>
        {
            new Vector3[] { new Vector3(0.59f, 0, -2.4f), new Vector3(0.59f, 0, -0.93f), new Vector3(0.59f, 0, 0.61f), new Vector3(0.59f, 0, 2.23f) },
            new Vector3[] { new Vector3(0.42f, 0, -2.55f), new Vector3(0.59f, 0, -0.85f), new Vector3(0.59f, 0, 0.59f), new Vector3(0.012f, 0, 2.34f) },
            new Vector3[] { new Vector3(0.98f, 0, -2.4f), new Vector3(0.19f, 0, -0.93f), new Vector3(0.98f, 0, 0.61f), new Vector3(0.19f, 0, 2.23f) },
            new Vector3[] { new Vector3(0.89f, 0.2f, -2.4f), new Vector3(0, 0, -0.93f), new Vector3(0, 0, 0.61f), new Vector3(0.8f, 0.2f, 2.23f) },
            new Vector3[] { new Vector3(1.55f, 0, -2.4f), new Vector3(1.04f, 0, -0.93f), new Vector3(0.65f, 0, 0.65f), new Vector3(0.13f, 0, 2.23f) },
            new Vector3[] { new Vector3(0.59f, 0.28f, -2.53f), new Vector3(0.59f, 0, -1.07f), new Vector3(0.59f, 0.28f, 0.79f), new Vector3(0.59f, 0, 2.23f) }
        },
        // Opciones de lilyPads con 3 posiciones
        [3] = new List<Vector3[]>
        {
            new Vector3[] { new Vector3(0.59f, 0, -1.86f), new Vector3(0.59f, 0, 0.1f), new Vector3(0.59f, 0, 2.13f) },
            new Vector3[] { new Vector3(0.59f, 0, -1.35f), new Vector3(0.59f, 0, 0.1f), new Vector3(0.59f, 0, 1.58f) },
            new Vector3[] { new Vector3(0.59f, 0, -2.18f), new Vector3(1.12f, 0, 0.1f), new Vector3(0.59f, 0, 2.32f) },
            new Vector3[] { new Vector3(0.59f, 0.27f, -2f), new Vector3(0.59f, 0, 0.1f), new Vector3(0.59f, 0.27f, 1.82f) },
            new Vector3[] { new Vector3(1f, 0.27f, -1.86f), new Vector3(-0f, 0.29f, 0.1f), new Vector3(1.61f, 0.27f, 2.13f) }
        },
        // Opciones de lilyPads con 2 posiciones
        [2] = new List<Vector3[]>
        {
            new Vector3[] { new Vector3(0.59f, 0f, -1f), new Vector3(0.59f, 0f, 1f) },
            new Vector3[] { new Vector3(0.59f, 0f, -1.7f), new Vector3(0.59f, 0f, 1.7f) },
            new Vector3[] { new Vector3(0.59f, 0.3f, -0.8f), new Vector3(0.59f, 0f, 0.8f) },
            new Vector3[] { new Vector3(0.59f, 0f, -1.5f), new Vector3(1.3f, 0f, 1.5f) }
        },
        [1] = new List<Vector3[]>
        {
            new Vector3[] { new Vector3(0.59f, 0, 0) }
        }
    };

    private Vector3[] currentPositions;
    private int currentCount;
    private float currentScale;

    void Awake()
    {
        // Para niveles 1 a 3, establecer número y posición predeterminada, sin variación de escala
        currentCount = 3;
        currentPositions = new Vector3[] { new Vector3(0.59f, 0, -1.86f), new Vector3(0.59f, 0, 0.1f), new Vector3(0.59f, 0, 2.13f) };
        currentScale = 1.0f; // Escala predeterminada
    }


    public void InitializeLevel(int level)
    {
        if (level == 1)
        {
            // Para niveles 1 a 3, establecer número y posición predeterminada, sin variación de escala
            currentCount = 3;
            currentPositions = lilyPadPositions[currentCount][0];
            currentScale = 1.0f; // Escala predeterminada
        }
        else if (level == 2)
        {
            currentCount = 3;
            RandomPosition(currentCount);
        }
        else if (level == 3)
        {
            currentCount = 3;
            RandomPosition(currentCount);
            RandomScale();
        }
        else
        {
            RandomCount();
            RandomPosition(currentCount);
            RandomScale();
        }
    }

    public void RandomScale()
    {
        currentScale = Random.Range(0.85f, 1.15f); // Ejemplo de escala variable para niveles altos
    }

    public void RandomCount()
    {
        currentCount = Random.Range(1, 5);
    }

    public void RandomPosition(int count)
    {
        int optionIndex = Random.Range(0, lilyPadPositions[currentCount].Count);
        currentPositions = lilyPadPositions[currentCount][optionIndex];
    }

    public int GetCurrentLilyPadCount()
    {
        return currentCount;
    }

    public Vector3[] GetCurrentLilyPadPositions()
    {
        return currentPositions;
    }

    public float GetCurrentLilyPadScale()
    {
        return currentScale;
    }

    public void ResetManager()
    {
        currentCount = 3;
        currentPositions = lilyPadPositions[currentCount][0];
        currentScale = 1.0f;
    }
}
