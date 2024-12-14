using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] models; // Spawnlanacak modeller
    public Transform spawnPoint; // Modellerin spawnlanaca�� nokta

    private GameObject currentModel; // Halihaz�rda spawnlanm�� modeli tutacak referans

    public void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < models.Length)
        {
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
}