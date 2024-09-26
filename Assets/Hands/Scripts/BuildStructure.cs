using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildStructure : MonoBehaviour
{
    private int playerLevel;
    private bool allThorns = false;
    public bool isActive = true;

    void Awake()
    {
        playerLevel = 1;
    }

    // Función para cambiar el nivel desde fuera del script
    public void SetPlayerLevel(int level)
    {
        playerLevel = level;
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public void SetAllThorns(bool allThorns)
    {
        this.allThorns = allThorns;
    }

    public bool GetAllThorns()
    {
        return allThorns;
    }

    public void BuildRandomStructure()
    {
        InstantiateIslands();
        InstantiateLilypadsBasedOnLevel();
        InstantiateRiver();
    }

    private void InstantiateIslands()
    {
        Vector3[] positionsIslands = { new Vector3(3.39f, 0, -9.71f), new Vector3(3.88f, 0, 9.96f) };
        InstantiateRandomIsland(positionsIslands);
    }

    private void InstantiateLilypadsBasedOnLevel()
    {
        string[] lilypadPrefabs = { "Bonus1", "Bonus2", "Bonus3", "Thorns" };
        Vector3[] positions = GetLilypadPositionsForLevel();
        int count = GetLilypadCountForLevel();

        InstantiateLilypads(lilypadPrefabs, positions, count);
    }

    private Vector3[] GetLilypadPositionsForLevel()
    {
        return playerLevel switch
        {
            0 or 1 => new Vector3[] {
                    new Vector3(0.59f, 0, -1.86f),
                    new Vector3(0.59f, 0, 0.1f),
                    new Vector3(0.59f, 0, 2.13f)
                },
            2 or 3 => new Vector3[] {
                    new Vector3(0.59f, 0, -2.4f),
                    new Vector3(0.59f, 0, -0.93f),
                    new Vector3(0.59f, 0, 0.61f),
                    new Vector3(0.59f, 0, 2.23f)
                },
            _ => new Vector3[] {
                    new Vector3(0.59f, 0, -2.93f),
                    new Vector3(0.59f, 0, -1.5f),
                    new Vector3(0.59f, 0, -0.09f),
                    new Vector3(0.59f, 0, 1.34f),
                    new Vector3(0.59f, 0, 2.76f)
                },
        };
    }

    private int GetLilypadCountForLevel()
    {
        return playerLevel switch
        {
            0 or 1 => 3,
            2 or 3 => 4,
            _ => 5,
        };
    }

    private void InstantiateRiver()
    {
        InstantiatePrefab("River", new Vector3(4, -0.2f, -0.5f));
    }

    void InstantiateRandomIsland(Vector3[] positions)
    {
        List<string> availableOptions = new() { "Island1", "Island2", "Island3" };

        //Primera Isla
        int randomIndexOption = Random.Range(0, availableOptions.Count);
        InstantiatePrefab(availableOptions[randomIndexOption], positions[0]);
        availableOptions.RemoveAt(randomIndexOption);

        //Segunda Isla
        randomIndexOption = Random.Range(0, availableOptions.Count);
        InstantiatePrefab(availableOptions[randomIndexOption], positions[1]);
    }

    void InstantiateLilypads(string[] prefabs, Vector3[] positions, int count)
    {
        if (allThorns)
        {
            InstantiateLilypadsWithoutCorrectOptions(positions);
        }
        else
        {
            InstantiateLilypadsWithCorrectOptions(prefabs, positions, count);
        }
    }

    private void InstantiateLilypadsWithCorrectOptions(string[] prefabs, Vector3[] positions, int count)
    {
        List<int> availablePositions = new();
        for (int i = 0; i < count; i++)
        {
            availablePositions.Add(i);
        }
        GameObject prefab;

        
        //Instanciar el camino libre
        int randomIndex;
        int optionPosition;

        /*
        //positions[optionPosition].y = 0.8f;
        prefab = InstantiatePrefab("Thorns 2", positions[optionPosition]);
        availablePositions.RemoveAt(randomIndex);
        prefab.SetActive(isActive);
        */

        //Instanciar obstaculo (solo uno)
        randomIndex = Random.Range(0, availablePositions.Count);
        optionPosition = availablePositions[randomIndex];
        prefab = InstantiatePrefab("Thorns", positions[optionPosition]);
        availablePositions.RemoveAt(randomIndex);
        prefab.SetActive(isActive);

        //Instanciar las otras opciones
        List<string> availableOptions = prefabs.ToList();
        int randomIndexOption;
        while (availablePositions.Count != 0)
        {
            randomIndexOption = Random.Range(0, availableOptions.Count);
            randomIndex = Random.Range(0, availablePositions.Count);
            optionPosition = availablePositions[randomIndex];
            prefab = InstantiatePrefab(availableOptions[randomIndexOption], positions[optionPosition]);
            //if (availableOptions[randomIndexOption] != "Thorns")
            //{
            availableOptions.RemoveAt(randomIndexOption);
            //}
            prefab.SetActive(isActive);
            availablePositions.RemoveAt(randomIndex);
        }

        //En caso se ser el nivel 0 o 1 solo queda una opcion mas que sera un obstaculo
        /*
        if (playerLevel == 0 || playerLevel == 1)
        {
            optionPosition = availablePositions[0];
            prefab = InstantiatePrefab("Thorns", positions[optionPosition]);
            prefab.SetActive(isActive);
        }
        else
        {
            List<string> availableOptions = prefabs.ToList();
            int randomIndexOption;
            while (availablePositions.Count != 0)
            {
                randomIndexOption = Random.Range(0, availableOptions.Count);
                randomIndex = Random.Range(0, availablePositions.Count);
                optionPosition = availablePositions[randomIndex];
                prefab = InstantiatePrefab(availableOptions[randomIndexOption], positions[optionPosition]);
                //if (availableOptions[randomIndexOption] != "Thorns")
                //{
                    availableOptions.RemoveAt(randomIndexOption);
                //}
                prefab.SetActive(isActive);
                availablePositions.RemoveAt(randomIndex);
            }
        }*/
    }

    private void InstantiateLilypadsWithoutCorrectOptions(Vector3[] positions)
    {
        GameObject prefab;

        for (int i = 0; i < positions.Length; i++)
        {
            prefab = InstantiatePrefab("Thorns", positions[i]);
            prefab.SetActive(isActive);
        }

    }

    GameObject InstantiatePrefab(string prefabName, Vector3 position)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, position, Quaternion.identity);
            instance.transform.parent = this.transform;
            return instance;
        }
        else
        {
            Debug.LogError("Prefab not found: " + prefabName);
            return null;
        }
    }
}