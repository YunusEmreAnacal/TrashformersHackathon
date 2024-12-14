using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] models; // Spawnlanacak modeller
    public Transform spawnPoint; // Modellerin spawnlanacaðý nokta

    public void SpawnModel(int modelIndex)
    {
        if (modelIndex >= 0 && modelIndex < models.Length)
        {
            Instantiate(models[modelIndex], spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Geçersiz model indexi!");
        }
    }
}