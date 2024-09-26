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
        Material[] materialBonus = new Material[3];

        if (gameObject.CompareTag("FreePath"))
        {
            MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
            meshRender.materials = materialsPlant;
        }
        else
        {
            Transform option = transform.GetChild(0);
            Transform plant = transform.GetChild(1);
            Material material;
            MeshRenderer meshRenderer = option.GetComponent<MeshRenderer>();
            

            if (gameObject.CompareTag("BlueBonus") || gameObject.CompareTag("GreenBonus"))
            {
                if (gameObject.CompareTag("BlueBonus"))
                {
                    material = Resources.Load<Material>("Materials/Glow Blue Mashroom");
                }
                else
                {
                    material = Resources.Load<Material>("Materials/Glow Green Mashroom");
                }
                meshRenderer.material = material;
            }
            else if (!gameObject.CompareTag("Thorns"))
            {
                for (int i = 0; i < 3; i++)
                {
                    materialBonus[i] = Resources.Load<Material>($"Materials/{gameObject.tag}GlowMaterial{i+1}");
                }
                meshRenderer.materials = materialBonus;
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
        Material[] materialBonus = new Material[3];

        if (gameObject.CompareTag("FreePath"))
        {
            MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
            meshRender.materials = materialsPlant;
        }
        else
        {
            Transform option = transform.GetChild(0);
            Transform plant = transform.GetChild(1);
            Material material;
            MeshRenderer meshRenderer = option.GetComponent<MeshRenderer>();


            if (gameObject.CompareTag("BlueBonus") || gameObject.CompareTag("GreenBonus"))
            {
                if (gameObject.CompareTag("BlueBonus"))
                {
                    material = Resources.Load<Material>("Materials/Blue Mashroom");
                }
                else
                {
                    material = Resources.Load<Material>("Materials/Green Mashroom");
                }
                meshRenderer.material = material;
            }
            else if (!gameObject.CompareTag("Thorns"))
            {
                for (int i = 0; i < 3; i++)
                {
                    materialBonus[i] = Resources.Load<Material>($"Materials/{gameObject.tag}Material{i + 1}");
                }
                meshRenderer.materials = materialBonus;
            }

            MeshRenderer plantMeshRender = plant.GetComponent<MeshRenderer>();
            plantMeshRender.materials = materialsPlant;
        }
    }
}