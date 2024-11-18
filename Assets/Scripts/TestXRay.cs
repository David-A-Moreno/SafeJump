using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TestXRay : MonoBehaviour
{
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private InputActionReference triggerAction;
    [SerializeField] private XRBaseController xrController;
    [SerializeField] private Movement moveProvider;//Movement.cs
    [SerializeField] private GameOverManager gameOver;
    [SerializeField] private UIPointsManager uiPointsManager;
    [SerializeField] private AudioFX audioFX;
    [SerializeField] private TestXRay otherHand;
    private bool move = false;

    private void OnEnable()
    {
        triggerAction.action.performed += OnTriggerActivated;
    }

    void OnTriggerActivated(InputAction.CallbackContext context)
    {
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.transform != null)
            {
                var interactable = hit.transform.GetComponent<XRSimpleInteractable>();
                if (interactable != null && !move)
                {
                    move = true;
                    otherHand.SetMove(true);
                    moveProvider.SetTargetPosition(hit.transform.position);
                    moveProvider.SetMove(true);
                    int points = 0;
                    if (hit.transform.CompareTag("Thorns"))
                    {
                        moveProvider.SetGameOver();
                    }
                    else
                    {
                        audioFX.PlaySound(1);
                        if (hit.transform.CompareTag("Bonus1"))
                        {
                            points = 20;
                        }
                        else if (hit.transform.CompareTag("Bonus2"))
                        {
                            points = 8;
                        }
                        else if (hit.transform.CompareTag("Bonus3"))
                        {
                            points = 5;
                        }
                    }


                    if (points != 0)
                    {
                        gameOver.increasePoints(points);
                        uiPointsManager.ShowPointsUI(hit.point, points);
                    }
                }
            }
        }
    }

    public void MostrarInfo(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactableObject.transform.gameObject;
    }

    public void SetMove(bool move)
    {
        this.move = move;
    }

    void OnDestroy()
    {
        triggerAction.action.performed -= OnTriggerActivated;
    }
}