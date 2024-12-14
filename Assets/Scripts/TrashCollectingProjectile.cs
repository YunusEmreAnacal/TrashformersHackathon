using System.Collections;
using UnityEngine;

public class TrashCollectingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float collectionRadius = 1f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask trashLayer;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private float collectionSpeed = 5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem collectionEffect;
    [SerializeField] private ParticleSystem trailEffect;
    [SerializeField] private AudioSource collectionSound;

    private bool isCollecting = false;
    private Transform collectionPoint;
    private ArrayList collectedTrash = new ArrayList();

    private void Start()
    {
        collectionPoint = transform;
        if (trailEffect != null)
            trailEffect.Play();

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (isCollecting)
        {
            // Pull nearby trash towards the collection point
            Collider[] nearbyTrash = Physics.OverlapSphere(transform.position, collectionRadius, trashLayer);

            foreach (Collider trash in nearbyTrash)
            {
                if (!collectedTrash.Contains(trash.gameObject))
                {
                    StartCoroutine(CollectTrash(trash.gameObject));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((floorLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        Debug.Log($"{other.gameObject.layer} --- {floorLayer.value}");

        if (!isCollecting)
        {
            // Stop the projectile and start collection
            Rigidbody rb = GetComponent<Rigidbody>();
            //rb.isKinematic = true;

            if (collectionEffect != null)
                collectionEffect.Play();
            if (collectionSound != null)
                collectionSound.Play();
            if (trailEffect != null)
                trailEffect.Stop();

            isCollecting = true;
            StartCoroutine(CollectionSequence());
        }
    }

    private IEnumerator CollectTrash(GameObject trash)
    {
        collectedTrash.Add(trash);
        Rigidbody trashRb = trash.GetComponent<Rigidbody>();
        Collider trashCollider = trash.GetComponent<Collider>();

        // Pull the trash towards the collection point
        while (trash != null && Vector3.Distance(trash.transform.position, collectionPoint.position) > 0.1f)
        {
            Vector3 direction = (collectionPoint.position - trash.transform.position).normalized;
            trashRb.linearVelocity = direction * collectionSpeed;
            yield return null;
        }

        if (trash != null)
        {
            // Destroy or deactivate the collected trash
            Destroy(trash);
            // Or if you want to pool objects instead:
            // trash.SetActive(false);
        }
    }

    private IEnumerator CollectionSequence()
    {
        // Collect for a certain duration
        yield return new WaitForSeconds(3f);

        // Clean up and destroy the projectile
        if (collectionEffect != null)
            collectionEffect.Stop();

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Visualize the collection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
