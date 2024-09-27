using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void IntantiateGameObjectScreen(Vector3 position)
    {
        GameObject gameOverScreen = Resources.Load<GameObject>("Prefabs/UI");
        Vector3 newPosition = new(position.x, 2, position.z - 11.28f);
        Instantiate(gameOverScreen, newPosition, Quaternion.identity);
    }
}
