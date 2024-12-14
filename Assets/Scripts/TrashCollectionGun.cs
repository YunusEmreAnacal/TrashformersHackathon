using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using UnityEngine.InputSystem;

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

    [Header("Input Settings")]
    [SerializeField] public InputActionProperty shootAction;

    private int currentAmmo;
    private bool canShoot = true;
    private bool isReloading = false;

    private void Start()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        currentAmmo = maxAmmo;

        grabInteractable.selectEntered.AddListener((args) => shootAction.action.started += context => Fire());
        grabInteractable.selectExited.AddListener((args) => shootAction.action.started -= context => Fire());
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
        if (currentAmmo <= 0) StartReload();
        else StartCoroutine(FireRateCooldown());
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
        Debug.Log("IS RELOADING");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("RELOADED");
    }
}