using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private bool inGame = true;
    private Vector3 targetScale = Vector3.one;

    public void SetTargetScale(float target)
    {
        targetScale = new Vector3 ( target, target, target );
    }

    public void GameOver()
    {
        inGame = false;
    }

    public void DestroyOptionEffect(GameObject parent)
    {
        // Iterar sobre todos los hijos del GameObject
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("Thorns") ||
                child.CompareTag("Bonus1") ||
                child.CompareTag("Bonus2") ||
                child.CompareTag("Bonus3"))
            {
                // Obtener el nombre del hijo para construir la ruta del prefab
                string childTag = child.tag;  // Usar el tag del hijo, aseg�rate de asignar el tag adecuado
                string prefabPath = $"Prefabs/DestroyEffects/{childTag}DestroyEffect";

                // Cargar el prefab desde la ruta especificada
                GameObject destroyEffectPrefab = Resources.Load<GameObject>(prefabPath);

                if (destroyEffectPrefab != null)
                {
                    // Crear la nueva posici�n del prefab (manteniendo la posici�n de X y Z, pero Y en 0.9)
                    Vector3 spawnPosition = new(child.position.x, 0.9f, child.position.z);

                    // Instanciar el prefab en la nueva posici�n
                    GameObject destroyEffectInstance = Instantiate(destroyEffectPrefab, spawnPosition, Quaternion.identity);

                    // Destruir el prefab despu�s de un tiempo (si es necesario), por ejemplo, despu�s de 5 segundos
                    Destroy(destroyEffectInstance, 5f);
                }
                else
                {
                    Debug.LogWarning($"Prefab not found at path: {prefabPath}");
                }

                // Destruir el hijo original
                Destroy(child.gameObject);
            }
        }
    }

    public void AppearOptionEffect(GameObject parent)
    {
        if (inGame)
        {
            // Iterar sobre todos los hijos del GameObject
            foreach (Transform child in parent.transform)
            {
                if (child.CompareTag("Thorns") ||
                    child.CompareTag("Bonus1") ||
                    child.CompareTag("Bonus2") ||
                    child.CompareTag("Bonus3"))
                {
                    // Activar el objeto hijo
                    child.gameObject.SetActive(true);

                    // Iniciar la animaci�n de aparici�n
                    StartCoroutine(ScaleUp(child));
                }
            }
        }
    }

    // Coroutine para escalar el objeto progresivamente
    private IEnumerator ScaleUp(Transform child)
    {
        Vector3 initialScale = new Vector3(0.3f, 0.3f, 0.3f); // Escala inicial
        float duration = 0.08f; // Duraci�n de la animaci�n en segundos
        float elapsed = 0.0f;

        // Escalar progresivamente desde initialScale hasta targetScale en el tiempo 'duration'
        while (elapsed < duration)
        {
            child.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la escala final sea exactamente la objetivo
        child.localScale = targetScale;
    }
}
