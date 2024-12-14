using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] models; // Spawnlanacak modeller
    public Transform spawnPoint; // Modellerin spawnlanacaðý nokta

    private GameObject currentModel; // Halihazýrda spawnlanmýþ modeli tutacak referans

    public void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < models.Length)
        {
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
}