using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // La posición objetivo a la que el objeto se moverá
    [SerializeField]
    private Vector3 targetPosition;

    // Variable booleana que determina si el objeto debe moverse
    [SerializeField]
    private bool move = false;

    [SerializeField]
    private TestXRay rightHand;

    [SerializeField]
    private TestXRay leftHand;

    [SerializeField]
    private AudioSource music;

    private bool waitForObjectDestruction = false;

    // Velocidad de movimiento
    private float speed = 20;

    private bool gameOver = false;
    //ProgressiveBuild.cs
    [SerializeField]
    private GameObject forestManagerReference;

    [SerializeField]
    private AudioFX audioFX;

    [SerializeField]
    private GameOverManager gameOverScript;

    public bool isAllThorns = false;

    // Update se llama una vez por frame
    void Update()
    {
        // Si move es true, mueve el objeto hacia la posición objetivo
        if (move && !waitForObjectDestruction)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // Mueve el objeto hacia la posición objetivo a la velocidad especificada
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        // Si el objeto ha alcanzado la posición objetivo, detén el movimiento
        if (transform.position == targetPosition)
        {
            rightHand.SetMove(false);
            leftHand.SetMove(false);
            move = false;
            waitForObjectDestruction = false;
            ProgressiveBuild progressiveBuild = forestManagerReference.GetComponent<ProgressiveBuild>();
            if (gameOver)
            {
                audioFX.PlaySound(4);
                progressiveBuild.setLostLevel();
                gameOverScript.GameOver(targetPosition, false);
            }
            else
            {
                if (!isAllThorns)
                {
                    audioFX.PlaySound(1);
                }
                progressiveBuild.AutomaticMoveTimer();
                progressiveBuild.DeactivateAllChildren();
            }
            isAllThorns = true;
        }
    }

    // Métodos para actualizar los valores desde otros scripts si es necesario
    public void SetTargetPosition(Vector3 newPosition)
    {
        if (!move)
        {
            newPosition.z -= 0.6f;
            newPosition.x += 1.2f;
            newPosition.y = transform.position.y;
            targetPosition = newPosition;
        }
    }

    public void WaitForDestruction ()
    {
        waitForObjectDestruction = true;
    }

    public void SetMove(bool shouldMove)
    {
        move = shouldMove;
        waitForObjectDestruction = false;
        forestManagerReference.GetComponent<ProgressiveBuild>().OneStep();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetIsAllThorns(bool isAllThorns)
    {
        this.isAllThorns = isAllThorns;
    }


    public void SetGameOver()
    {
        gameOver = true;
    }
    public bool GetMove()
    {
        return move;
    }
}
