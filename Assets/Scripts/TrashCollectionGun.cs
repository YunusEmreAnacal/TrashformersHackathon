using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class TrashCollectionGun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private XRGrabInteractable grabInteractable;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource emptySound;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadTime = 1.5f;

    private int currentAmmo;
    private bool canShoot = true;
    private bool isReloading = false;

    private void Start()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        currentAmmo = maxAmmo;
    }

    public void Fire()
    {
        if (!canShoot || isReloading || currentAmmo <= 0)
        {
            if (emptySound != null)
                emptySound.Play();
            return;
        }

        // Spawn and fire projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        // Play effects
        if (muzzleFlash != null)
            muzzleFlash.Play();
        if (fireSound != null)
            fireSound.Play();

        currentAmmo--;
        StartCoroutine(FireRateCooldown());
    }

    private IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void StartReload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
            StartCoroutine(ReloadSequence());
    }

    private IEnumerator ReloadSequence()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

public class TrashCollectingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float collectionRadius = 1f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask trashLayer;
    [SerializeField] private float pullForce = 15f;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollecting)
        {
            // Stop the projectile and start collection
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;

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