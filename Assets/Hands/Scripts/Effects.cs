using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
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
}
