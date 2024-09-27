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
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private Movement moveProvider;//Movement.cs

    private void OnEnable()
    {
        triggerAction.action.performed += OnTriggerActivated;
    }

    void OnTriggerActivated(InputAction.CallbackContext context)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.transform.GetComponent<XRSimpleInteractable>();
            if (interactable != null)
            {
                moveProvider.SetTargetPosition(hit.transform.position);
                moveProvider.SetSpeed(36);
                moveProvider.SetMove(true);
            }
        }
    }

    void OnDestroy()
    {
        triggerAction.action.performed -= OnTriggerActivated;
    }

}