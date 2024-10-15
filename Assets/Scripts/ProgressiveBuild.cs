using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveBuild : MonoBehaviour
{
    [SerializeField]
    private GameObject[] initialStructures = new GameObject[13];

    [SerializeField]
    private GameObject gameLight;

    [SerializeField]
    private GameObject dome;

    [SerializeField]
    private GameObject cloud;

    [SerializeField]
    private Movement movement; //Movement.cs

    [SerializeField]
    private Effects effects; //Effects.cs

    [SerializeField]
    private GameOverManager gameOverScript;

    private int stepsProgress = 0;
    private int destroyedStructures = 0;
    Vector3 position = new(0, 0, 0);
    Vector3 resetPosition = new(0, 0, 0);

    // Lista para almacenar todas las estructuras
    private List<GameObject> structures = new();

    private Coroutine currentCoroutine;

    // Constantes
    private const float positionOffsetX = 11.28f;
    private const float cloudMoveSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStructures();
    }

    void Update()
    {
        MoveCloud();
    }

    private void InitializeStructures()
    {
        Vector3 targetPosition = Vector3.zero;
        foreach (var structure in initialStructures)
        {
            var buildStructure = structure.GetComponent<BuildStructure>();
            buildStructure.SetPlayerLevel(GetInitialPlayerLevel(structure));
            bool allThorns = UnityEngine.Random.value < 0.2f;
            if (allThorns && structure != initialStructures[0] && structure != initialStructures[1] && structure != initialStructures[2])
            {
                buildStructure.SetAllThorns(allThorns);
            }
            Console.WriteLine(allThorns);
            buildStructure.BuildRandomStructure();

            if (structure == initialStructures[0])
            {
                //ActivateAllChildren(structure);
                effects.AppearOptionEffect(structure);
            }

            structure.transform.position = targetPosition;
            structures.Add(structure);
            targetPosition.x -= positionOffsetX;
        }
        position = targetPosition;
    }

    private int GetInitialPlayerLevel(GameObject structure)
    {
        int index = System.Array.IndexOf(initialStructures, structure);
        //return (index < 5) ? 1 : 3;
        return 1;
    }

    private void MoveCloud()
    {
        cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, position, cloudMoveSpeed * Time.deltaTime);
    }

    public void OneStep()
    {
        stepsProgress++;

        //Inicializar nueva estructura
        GameObject randomStructure = InstantiateStructure();
        if (randomStructure == null) return;

        //Agregarle el nivel
        //SetStructurePlayerLevel(randomStructure);

        //Probabilidad del 10% de que la opción sea NO-GO
        bool allThorns = UnityEngine.Random.value < 0.1f;
        randomStructure.GetComponent<BuildStructure>().SetAllThorns(allThorns);

        //Crear la estructura
        randomStructure.GetComponent<BuildStructure>().BuildRandomStructure();

        //Posicionar la estructura
        randomStructure.transform.position = position;

        //Posicionar el domo
        dome.transform.position = position;
        structures.Add(randomStructure);
        if (stepsProgress != 1)
        {
            RemoveFirstStructure();
        }

        //Guardar la posicion de la siguiente estructura
        position.x -= positionOffsetX;
    }

    private void SetStructurePlayerLevel(GameObject structure)
    {
        var buildStructure = structure.GetComponent<BuildStructure>();
        if (stepsProgress >= 1 && stepsProgress <= 5)
        {
            buildStructure.SetPlayerLevel(5);
        }
        else
        {
            buildStructure.SetPlayerLevel(6);
        }
    }

    public void AutomaticMoveTimer()
    {
        // Cancela la corutina actual si está en ejecución
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        // Inicia la corutina que controla el tiempo de espera según el stepsProgress
        StartCoroutine(CheckMoveStatus());
    }

    GameObject InstantiateStructure()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + "Structure");
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, resetPosition, Quaternion.identity);
            instance.transform.parent = this.transform;
            return instance;
        }
        else
        {
            Debug.LogError("Prefab not found: " + "Structure");
            return null;
        }
    }

    // Método para eliminar la primera estructura de la lista
    public void RemoveFirstStructure()
    {
        if (structures.Count > 0)
        {
            GameObject firstStructure = structures[0];
            structures.RemoveAt(0);
            Destroy(firstStructure);
            destroyedStructures += 1;
        }
        else
        {
            Debug.LogWarning("No structures to remove.");
        }
    }

    // Corutina para verificar el estado de "move" y esperar el tiempo necesario
    private IEnumerator CheckMoveStatus()
    {
        GameObject nextStructure = GetNextStructure();
        //ActivateAllChildren(nextStructure);
        effects.AppearOptionEffect(nextStructure);
        float waitTime;
        bool allThorns = nextStructure.GetComponent<BuildStructure>().GetAllThorns();
        if (!allThorns)
        {
            //waitTime = (stepsProgress >= 1 && stepsProgress <= 5) ? 2f : 0.8f;
            waitTime = 0.8f;
        }
        else
        {
            waitTime = 1.5f;
        }
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            // Si "move" es true, se cancela la corutina
            if (movement.GetMove())
            {
                yield break; // Termina la corutina si "move" está en true
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Busca el GameObject con el tag "FreePath" dentro de la estructura en la posición stepsProgress+1
        SetAutomaticMovement(nextStructure, "Thorns2", allThorns);
    }

    private GameObject GetNextStructure()
    {
        if (stepsProgress != 1)
        {
            return structures[stepsProgress - destroyedStructures];
        }
        else
        {
            return structures[stepsProgress];
        }
    }

    public void SetAutomaticMovement(GameObject targetStructure, string targetTag, bool allThorns)
    {
        GameObject target;
        if (allThorns)
        {
            target = FindChildWithTag(targetStructure, "Thorns");
        }
        else
        {
            if (FindChildWithTag(targetStructure, "Bonus1") != null)
            {
                target = FindChildWithTag(targetStructure, "Bonus1");
            }
            else
            {
                target = FindChildWithTag(targetStructure, "Bonus2");
            }
        }
        Vector3 newPosition = target.transform.position;
        newPosition.z = 0;
        movement.SetTargetPosition(newPosition);
        effects.DestroyOptionEffect(targetStructure);
        movement.WaitForDestruction();
        StartCoroutine(WaitAndExecute(targetStructure, allThorns));
    }

    private IEnumerator WaitAndExecute(GameObject targetStructure, bool allThorns)
    {
        // Destruir el efecto
        effects.DestroyOptionEffect(targetStructure);

        // Esperar 2 segundos
        yield return new WaitForSeconds(0.5f);

        // Aquí continúa el código que se ejecutará después de la espera de 2 segundos
        if (stepsProgress > 0)
        {
            if (allThorns)
            {
                movement.SetMove(true);
            }
            else
            {
                gameOverScript.GameOver(targetStructure.transform.position, true);
            }
        }
    }

    public void ActivateAllChildren(GameObject parent)
    {
        // Iteramos sobre cada transform hijo del objeto padre
        foreach (Transform child in parent.transform)
        {
            // Activamos el GameObject hijo
            child.gameObject.SetActive(true);
        }
    }

    public void DeactivateAllChildren(GameObject parent)
    {
        // Iteramos sobre cada transform hijo del objeto padre
        foreach (Transform child in parent.transform)
        {
            GameObject childGameObject = child.gameObject;

            if (childGameObject.CompareTag("Thorns") ||
                childGameObject.CompareTag("Bonus1") ||
                childGameObject.CompareTag("Bonus2") ||
                childGameObject.CompareTag("Bonus3"))
            {
                // Desactivamos el GameObject hijo
                childGameObject.SetActive(false);
            }
        }
    }

    public GameObject[] FindChildrenWithTag(GameObject parent, string tag)
    {
        // Obtenemos todos los componentes Transform dentro del padre (incluyendo los hijos)
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();

        // Lista para almacenar los GameObjects con el tag deseado
        List<GameObject> childrenWithTag = new List<GameObject>();

        // Recorremos cada transform y verificamos el tag
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag(tag))
            {
                childrenWithTag.Add(child.gameObject);
            }
        }

        // Convertimos la lista a un array y lo retornamos
        return childrenWithTag.ToArray();
    }

    // Método auxiliar para buscar un hijo por tag
    private GameObject FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }
}