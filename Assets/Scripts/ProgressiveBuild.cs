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

    [SerializeField]
    private GameStartUI gameStartUI;

    [SerializeField]
    private LilyPadManager lilyPadManager;

    private GameObject lastStructure = null;

    private int stepsProgress = 0;
    private int structuresCreated = 0;
    private int destroyedStructures = 0;
    private int playerLevel = 1;
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
        gameStartUI.showGameStartUI();
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
            if (playerLevel != GetInitialPlayerLevel(structure))
            {
                playerLevel = GetInitialPlayerLevel(structure);
                lilyPadManager.InitializeLevel(playerLevel);
            }
            bool allThorns = UnityEngine.Random.value < 0.2f;
            if (allThorns && structure != initialStructures[0] && structure != initialStructures[1] && structure != initialStructures[2])
            {
                buildStructure.SetAllThorns(allThorns);
            }
            Console.WriteLine(allThorns);
            buildStructure.BuildRandomStructure();

            if (!gameStartUI.getFirstGame())
            {
                appearFirstOptions();
            }

            structure.transform.position = targetPosition;
            structures.Add(structure);
            targetPosition.x -= positionOffsetX;
        }
        position = targetPosition;
        structuresCreated = 13;
    }

    public void appearFirstOptions()
    {
        effects.AppearOptionEffect(initialStructures[0]);
    }

    private int GetInitialPlayerLevel(GameObject structure)
    {
        int index = System.Array.IndexOf(initialStructures, structure);
        return (index < 11) ? 1 : 2;
    }

    private void MoveCloud()
    {
        cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, position, cloudMoveSpeed * Time.deltaTime);
    }

    public void OneStep()
    {
        stepsProgress++;
        structuresCreated++;
        //Inicializar nueva estructura
        GameObject randomStructure = InstantiateStructure();
        if (randomStructure == null) return;

        //Agregar nivel al juego
        SetPlayerLevel();

        //Probabilidad del 10% de que la opcion sea NO-GO
        bool allThorns = UnityEngine.Random.value < 0.15f;
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

        if (allThorns && playerLevel > 3)
        {
            // Cambia el patrón y escala de nenúfares al encontrar una opción "no-go"
            lilyPadManager.InitializeLevel(playerLevel);
        }

    }

    private void SetPlayerLevel()
    {
        int newLevel;

        if (structuresCreated >= 31 && structuresCreated < 41)
        {
            newLevel = 4;
        }
        else if (structuresCreated >= 21 && structuresCreated < 31)
        {
            newLevel = 3;
        }
        else if (structuresCreated >= 11 && structuresCreated < 21)
        {
            newLevel = 2;
        }
        else
        {
            newLevel = 5;
        }

        if (playerLevel != newLevel)
        {
            playerLevel = newLevel;
            lilyPadManager.InitializeLevel(playerLevel);
        }
    }


    public void AutomaticMoveTimer()
    {
        // Cancela la corutina actual si est� en ejecuci�n
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        // Inicia la corutina que controla el tiempo de espera seg�n el stepsProgress
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

    // M�todo para eliminar la primera estructura de la lista
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
            if (playerLevel == 1) 
            {
                waitTime = 2.5f;
                movement.SetSpeed(20);
            }
            else if (playerLevel == 2)
            {
                waitTime = 1.5f;
                movement.SetSpeed(28);
            }
            else if (playerLevel == 3)
            {
                waitTime = 1f;
                movement.SetSpeed(40);
            }
            else
            {
                waitTime = 0.65f;
                movement.SetSpeed(55);
            }
            //waitTime = (stepsProgress >= 1 && stepsProgress <= 5) ? 2f : 0.8f;
            //waitTime = 0.5f;

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
                yield break; // Termina la corutina si "move" est� en true
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Busca el GameObject con el tag "FreePath" dentro de la estructura en la posici�n stepsProgress+1
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
            else if (FindChildWithTag(targetStructure, "Bonus2") != null)
            {
                target = FindChildWithTag(targetStructure, "Bonus2");
            }
            else
            {
                target = FindChildWithTag(targetStructure, "Bonus3");
            }
            lastStructure = targetStructure;
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

        // Aqu� contin�a el c�digo que se ejecutar� despu�s de la espera de 2 segundos
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

    public void DeactivateAllChildren()
    {
        if (lastStructure != null)
        {
            // Iteramos sobre cada transform hijo del objeto padre
            foreach (Transform child in lastStructure.transform)
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
            lastStructure = null;
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

    // M�todo auxiliar para buscar un hijo por tag
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

    public int GetLevel()
    {
        return playerLevel;
    }
}