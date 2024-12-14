using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;

public class VacuumCleanerController : MonoBehaviour
{
    [Header("Vacuum Settings")]
    [SerializeField] private Transform suctionPoint;
    [SerializeField] private float suctionRadius = 2f;
    [SerializeField] private float suctionForce = 10f;
    [SerializeField] private LayerMask trashLayer;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private float collectionDistance = 0.5f;

    [Header("Visual & Audio Feedback")]
    [SerializeField] private ParticleSystem suctionEffect;
    [SerializeField] private AudioSource vacuumSound;
    [SerializeField] private float particleStartSpeed = 2f;

    [Header("Collection Settings")]
    [SerializeField] private int maxCollectedItems = 10;
    [SerializeField] private Transform collectionContainer;

    private bool isVacuuming = false;
    private bool isGrabbed = false;
    private List<GameObject> collectedTrash = new List<GameObject>();

    private void Start()
    {
        // Setup XR Grab Interactable events
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Initialize effects
        if (suctionEffect != null)
            suctionEffect.Stop();
        if (vacuumSound != null)
            vacuumSound.Stop();

        // Create collection container if not assigned
        if (collectionContainer == null)
        {
            GameObject container = new GameObject("TrashContainer");
            container.transform.SetParent(transform);
            collectionContainer = container.transform;
        }
    }

    private void Update()
    {
        if (!isGrabbed || !isVacuuming)
            return;

        // Find all trash objects within suction radius
        Collider[] nearbyTrash = Physics.OverlapSphere(suctionPoint.position, suctionRadius, trashLayer);

        foreach (Collider trash in nearbyTrash)
        {
            Rigidbody trashRb = trash.GetComponent<Rigidbody>();

            if (trashRb != null && !collectedTrash.Contains(trash.gameObject))
            {
                // Calculate direction and distance to the suction point
                Vector3 directionToSuction = (suctionPoint.position - trash.transform.position).normalized;
                float distanceToSuction = Vector3.Distance(suctionPoint.position, trash.transform.position);

                // Apply suction force
                float forceMagnitude = suctionForce * (1 - (distanceToSuction / suctionRadius));
                trashRb.AddForce(directionToSuction * forceMagnitude, ForceMode.Force);

                // Check if trash is close enough to collect
                if (distanceToSuction < collectionDistance && collectedTrash.Count < maxCollectedItems)
                {
                    CollectTrash(trash.gameObject);
                }
            }
        }
    }

    private void CollectTrash(GameObject trash)
    {
        if (collectedTrash.Contains(trash))
            return;

        Rigidbody trashRb = trash.GetComponent<Rigidbody>();
        Collider trashCollider = trash.GetComponent<Collider>();

        // Disable physics and collision
        trashRb.isKinematic = true;
        trashCollider.enabled = false;

        // Store the trash in the container
        trash.transform.SetParent(collectionContainer);
        trash.transform.localScale *= 0.5f; // Optional: shrink the trash
        trash.transform.localPosition = Random.insideUnitSphere * 0.3f; // Random position inside container

        collectedTrash.Add(trash);

        // Optional: Play collection effect/sound
        PlayCollectionEffect(trash.transform.position);
    }

    public void StartVacuuming()
    {
        isVacuuming = true;

        // Start effects
        if (suctionEffect != null)
        {
            var main = suctionEffect.main;
            main.startSpeed = particleStartSpeed;
            suctionEffect.Play();
        }

        if (vacuumSound != null)
            vacuumSound.Play();
    }

    public void StopVacuuming()
    {
        isVacuuming = false;

        // Stop effects
        if (suctionEffect != null)
            suctionEffect.Stop();

        if (vacuumSound != null)
            vacuumSound.Stop();
    }

    public void EmptyVacuum()
    {
        foreach (GameObject trash in collectedTrash)
        {
            if (trash != null)
            {
                // Reset the trash object
                trash.transform.SetParent(null);
                trash.transform.localScale *= 2f; // Restore original scale

                Rigidbody trashRb = trash.GetComponent<Rigidbody>();
                Collider trashCollider = trash.GetComponent<Collider>();

                // Re-enable physics and collision
                trashRb.isKinematic = false;
                trashCollider.enabled = true;

                // Optional: Add some spread when emptying
                trashRb.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);
            }
        }

        collectedTrash.Clear();
    }

    private void PlayCollectionEffect(Vector3 position)
    {
        // Add your collection effect here (particles, sound, etc.)
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        StopVacuuming();
    }

    // Visualize the suction radius in the editor
    private void OnDrawGizmosSelected()
    {
        if (suctionPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(suctionPoint.position, suctionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(suctionPoint.position, collectionDistance);
        }
    }
}