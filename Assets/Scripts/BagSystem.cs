using UnityEngine;

public class BagSystem : MonoBehaviour
{
    public Transform bagPosition; // �antan�n sol omuzdaki pozisyonu
    public Transform hand; // Kullan�c�n�n elinin pozisyonu
    public GameObject bagModel; // �anta modeli
    public int playerLevel = 1; // Oyuncu seviyesi
    public int maxCapacity = 5; // Ba�lang�� kapasitesi
    private int currentCapacity = 0; 
    private bool isBagInHand = false; 

    void Start()
    {
        UpdateCapacity(); 
        //bagModel.SetActive(false); 
    }

    void Update()
    {
        // El omuz pozisyonuna yak�n m� ve tutma butonu bas�l� m�?
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
                Destroy(other.gameObject); // Objeyi sahneden kald�r
                Debug.Log($"�antadaki obje say�s�: {currentCapacity}/{maxCapacity}");
            }
            else
            {
                Debug.Log("�anta dolu! Daha fazla obje alamazs�n�z.");
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