using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class TrashSpearController : MonoBehaviour
{
    [Header("Spear Settings")]
    [SerializeField] private Transform spearTip;
    [SerializeField] private float attachRadius = 0.1f;
    [SerializeField] private LayerMask trashLayer;
    [SerializeField] private XRGrabInteractable grabInteractable;

    [Header("Visual Feedback")]
    [SerializeField] private Material defaultTipMaterial;
    [SerializeField] private Material activeTipMaterial;
    [SerializeField] private MeshRenderer tipRenderer;

    [Header("Input Settings")]
    [SerializeField] private InputActionProperty dropAction;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownDuration = 1f; // Time in seconds before can collect again

    private GameObject attachedTrash;
    private Rigidbody attachedTrashRb;
    private Collider attachedTrashCollider;
    private Vector3 attachPoint;
    private bool isGrabbed = false;
    private bool isInCooldown = false;

    private void Start()
    {
        // Setup XR Grab Interactable events
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Initialize tip material
        if (tipRenderer != null)
            tipRenderer.material = defaultTipMaterial;

        dropAction.action.started += (args) => ReleaseTrash();
    }

    private void Update()
    {
        if (!isGrabbed || attachedTrash != null || isInCooldown)
            return;

        // Check for trash objects near the tip
        Collider[] nearbyColliders = Physics.OverlapSphere(spearTip.position, attachRadius, trashLayer);
        if (nearbyColliders.Length > 0)
        {
            Debug.Log("Trash is collected!!!");
            AttachTrash(nearbyColliders[0].gameObject);
        }
    }

    private void AttachTrash(GameObject trash)
    {
        attachedTrash = trash;
        attachedTrashRb = trash.GetComponent<Rigidbody>();
        attachedTrashCollider = trash.GetComponent<Collider>();

        if (attachedTrashRb != null)
        {
            // Store the local attach point
            attachPoint = spearTip.InverseTransformPoint(attachedTrash.transform.position);

            // Disable physics on the trash object
            attachedTrashRb.isKinematic = true;
            attachedTrashCollider.enabled = false;

            // Parent the trash to the spear tip
            attachedTrash.transform.SetParent(spearTip);
        }

        // Visual feedback
        if (tipRenderer != null)
            tipRenderer.material = activeTipMaterial;
    }

    public void ReleaseTrash()
    {
        if (attachedTrash == null)
            return;

        // Unparent the trash
        attachedTrash.transform.SetParent(null);

        // Re-enable physics
        if (attachedTrashRb != null)
        {
            attachedTrashRb.isKinematic = false;
            attachedTrashCollider.enabled = true;

            // Optional: Add some force when releasing
            attachedTrashRb.AddForce(spearTip.forward * 2f, ForceMode.Impulse);
        }

        // Reset references
        attachedTrash = null;
        attachedTrashRb = null;
        attachedTrashCollider = null;

        // Visual feedback
        if (tipRenderer != null)
            tipRenderer.material = defaultTipMaterial;

        // Start cooldown
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        isInCooldown = true;

        // Optional: Visual feedback for cooldown
        if (tipRenderer != null)
        {
            // You could create a specific cooldown material or modify the default material
            Color originalColor = tipRenderer.material.color;
            tipRenderer.material.color = Color.gray; // Or any color to indicate cooldown

            yield return new WaitForSeconds(cooldownDuration);

            tipRenderer.material.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(cooldownDuration);
        }

        isInCooldown = false;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    // Optional: Visualize the attachment radius in the editor
    private void OnDrawGizmosSelected()
    {
        if (spearTip != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spearTip.position, attachRadius);
        }
    }
}