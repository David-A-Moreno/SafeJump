using System.Collections;
using UnityEngine;
using TMPro;

public class UIPointsManager : MonoBehaviour
{
    public void ShowPointsUI(Vector3 position, int points)
    {
        // Cargar el prefab desde Resources
        GameObject uiPrefab = Resources.Load<GameObject>("Prefabs/UI Points");
        if (uiPrefab != null)
        {
            // Instanciar el prefab en la posición especificada
            GameObject uiInstance = Instantiate(uiPrefab, position, Quaternion.identity);

            // Configurar el texto del prefab con el número de puntos obtenidos
            TextMeshProUGUI textComponent = uiInstance.transform.Find("Points/Panel/Text").GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "+"+points.ToString();
            }

            // Iniciar la rutina para mover el objeto
            StartCoroutine(MoveUIPoints(uiInstance));
        }
    }

    private IEnumerator MoveUIPoints(GameObject uiInstance)
    {
        // Definir las posiciones objetivo en los ejes X e Y
        Vector3 targetPosition = uiInstance.transform.position;
        targetPosition.x -= 11.28f; // Posición objetivo en el eje X
        targetPosition.y = 2f;      // Posición objetivo en el eje Y

        // Velocidades de movimiento para cada eje
        float speedX = 55f;
        float speedY = 2f;

        // Mover el objeto hasta que alcance las posiciones objetivo en X e Y
        while (Vector3.Distance(uiInstance.transform.position, targetPosition) > 0.1f)
        {
            uiInstance.transform.position = new Vector3(
                Mathf.MoveTowards(uiInstance.transform.position.x, targetPosition.x, speedX * Time.deltaTime),
                Mathf.MoveTowards(uiInstance.transform.position.y, targetPosition.y, speedY * Time.deltaTime),
                uiInstance.transform.position.z
            );
            yield return null;
        }

        Destroy(uiInstance);
    }
}
