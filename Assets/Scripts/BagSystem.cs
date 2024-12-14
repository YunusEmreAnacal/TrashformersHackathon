using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] models; // Spawnlanacak modeller
    public Transform spawnPoint; // Modellerin spawnlanaca�� nokta
    public bool[] modelLocks; // Modellerin kilit durumlar�n� tutar

    private GameObject currentModel; // Halihaz�rda spawnlanm�� modeli tutacak referans

    public void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < models.Length)
        {
            // Modelin kilit durumu kontrol ediliyor
            if (modelLocks[modelIndex])
            {
                Debug.LogWarning("Bu modelin kilidi hen�z a��lmam��!");
                return;
            }

            // �nceki modeli yok et
            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            // Yeni modeli spawnla ve referans� g�ncelle
            currentModel = Instantiate(models[modelIndex], spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Ge�ersiz model indexi!");
        }
    }

    // Modelin kilidini a�mak i�in bir metod
    public void UnlockModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < modelLocks.Length)
        {
            modelLocks[modelIndex] = false;
            Debug.Log($"Model {modelIndex} kilidi a��ld�!");
        }
        else
        {
            Debug.LogError("Ge�ersiz model indexi!");
        }
    }
}