using UnityEngine;

public class BagSystem : MonoBehaviour
{
    public Transform bagPosition; // Çantanýn sol omuzdaki pozisyonu
    public Transform hand; // Kullanýcýnýn elinin pozisyonu
    public GameObject bagModel; // Çanta modeli
    public int playerLevel = 1; // Oyuncu seviyesi
    public int maxCapacity = 5; // Baþlangýç kapasitesi
    private int currentCapacity = 0; 
    private bool isBagInHand = false; 

    void Start()
    {
        UpdateCapacity(); 
        //bagModel.SetActive(false); 
    }

    void Update()
    {
        // El omuz pozisyonuna yakýn mý ve tutma butonu basýlý mý?
        if (Vector3.Distance(hand.position, bagPosition.position) < 0.2f && Input.GetButton("Grab"))
        {
            TakeBag();
        }
    }

    void TakeBag()
    {
        if (!isBagInHand)
        {
            //bagModel.SetActive(true);
            isBagInHand = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Garbage etiketli bir obje mi?
        if (other.CompareTag("Garbage") && isBagInHand)
        {
            if (currentCapacity < maxCapacity)
            {
                currentCapacity++;
                Destroy(other.gameObject); // Objeyi sahneden kaldýr
                Debug.Log($"Çantadaki obje sayýsý: {currentCapacity}/{maxCapacity}");
            }
            else
            {
                Debug.Log("Çanta dolu! Daha fazla obje alamazsýnýz.");
            }
        }
    }

    public void LevelUp()
    {
        playerLevel++;
        UpdateCapacity();
    }

    private void UpdateCapacity()
    {
        maxCapacity = playerLevel * 5; 
    }
}