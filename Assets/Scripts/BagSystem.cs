using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] models; // Spawnlanacak modeller
    public Transform spawnPoint; // Modellerin spawnlanacaðý nokta
    public bool[] modelLocks; // Modellerin kilit durumlarýný tutar

    private GameObject currentModel; // Halihazýrda spawnlanmýþ modeli tutacak referans

    public void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < models.Length)
        {
            // Modelin kilit durumu kontrol ediliyor
            if (modelLocks[modelIndex])
            {
                Debug.LogWarning("Bu modelin kilidi henüz açýlmamýþ!");
                return;
            }

            // Önceki modeli yok et
            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            // Yeni modeli spawnla ve referansý güncelle
            currentModel = Instantiate(models[modelIndex], spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Geçersiz model indexi!");
        }
    }

    // Modelin kilidini açmak için bir metod
    public void UnlockModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < modelLocks.Length)
        {
            modelLocks[modelIndex] = false;
            Debug.Log($"Model {modelIndex} kilidi açýldý!");
        }
        else
        {
            Debug.LogError("Geçersiz model indexi!");
        }
    }
}