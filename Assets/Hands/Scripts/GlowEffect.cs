using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public void GlowOn()
    {
        Material glow1 = Resources.Load<Material>("Materials/Glow 1");
        Material glow2 = Resources.Load<Material>("Materials/Glow 2");
        Material glow3 = Resources.Load<Material>("Materials/Glow 3");
        Material[] materialsPlant = new Material[] { glow1, glow2, glow3 };

        if (gameObject.CompareTag("FreePath"))
        {
            MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
            meshRender.materials = materialsPlant;
        }
        else
        {
            Material material;
            MeshRenderer meshRenderer;

            if (gameObject.CompareTag("BlueBonus"))
            {
                material = Resources.Load<Material>("Materials/Glow Blue Mashroom");
            }
            else if (gameObject.CompareTag("GreenBonus"))
            {
                material = Resources.Load<Material>("Materials/Glow Green Mashroom");
            }
            else if (gameObject.CompareTag("Thorns"))
            {
                material = Resources.Load<Material>("Materials/Glow Thorns");
            }
            else
            {
                material = Resources.Load<Material>("Materials/Glow Thorns2");
            }
            Transform option = transform.GetChild(0);
            Transform plant = transform.GetChild(1);
            
            //El prefab thorn maneja diferente jerarquia
            if (!gameObject.CompareTag("Thorns") && !gameObject.CompareTag("Thorns2"))
            {
                Transform optionChild = option.GetChild(0);
                meshRenderer = optionChild.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = material;
                }
                else
                {
                    Debug.LogWarning("El hijo de 'Bonus' no tiene un componente MeshRenderer.");
                }
            }
            else
            {
                meshRenderer = option.GetComponent<MeshRenderer>();
                meshRenderer.material = material;
            }
            MeshRenderer plantMeshRender = plant.GetComponent<MeshRenderer>();
            plantMeshRender.materials = materialsPlant;
        }
    }

    public void GlowOff()
    {
        Material material1 = Resources.Load<Material>("Materials/Material 1");
        Material material2 = Resources.Load<Material>("Materials/Material 2");
        Material material3 = Resources.Load<Material>("Materials/Material 3");
        Material[] materialsPlant = new Material[] { material1, material2, material3 } ;

        if (gameObject.CompareTag("FreePath"))
        {
            MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
            meshRender.materials = materialsPlant;
        }
        else
        {
            Material material;
            MeshRenderer meshRenderer;
            if (gameObject.CompareTag("BlueBonus"))
            {
                material = Resources.Load<Material>("Materials/Blue Mashroom");
            }
            else if (gameObject.CompareTag("GreenBonus"))
            {
                material = Resources.Load<Material>("Materials/Green Mashroom");
            }
            else if (gameObject.CompareTag("Thorns"))
            {
                material = Resources.Load<Material>("Materials/Thorns");
            }
            else
            {
                material = Resources.Load<Material>("Materials/Thorns2");
            }
            Transform option = transform.GetChild(0);
            Transform plant = transform.GetChild(1);
            if (!gameObject.CompareTag("Thorns") && !gameObject.CompareTag("Thorns2"))
            {
                Transform optionChild = option.GetChild(0);
                meshRenderer = optionChild.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = material;
                }
                else
                {
                    Debug.LogWarning("El hijo de 'Bonus' no tiene un componente MeshRenderer.");
                }
            }
            else
            {
                meshRenderer = option.GetComponent<MeshRenderer>();
                meshRenderer.material = material;
            }
            MeshRenderer plantMeshRender = plant.GetComponent<MeshRenderer>();
            plantMeshRender.materials = materialsPlant;
        }
    }
}