using UnityEngine;
using TMPro;

public class GarbageCounter : MonoBehaviour
{
    public TextMeshProUGUI garbageText; // TextMeshPro metin objesini buraya atayýn
    public Terrain terrain;            // Terrain objesini buraya atayýn

    void Update()
    {
        if (terrain == null || garbageText == null)
        {
            Debug.LogWarning("Terrain veya TextMeshPro metin objesi atanmadý!");
            return;
        }

        // Terrain üzerinde "garbage" tagine sahip objeleri say
        GameObject[] garbageObjects = GameObject.FindGameObjectsWithTag("Garbage");
        int garbageCount = 0;

        foreach (GameObject obj in garbageObjects)
        {
            if (IsOnTerrain(obj))
            {
                garbageCount++;
            }
        }

        // TextMeshPro metnine yaz
        garbageText.text = "Çöp Sayýsý: " + garbageCount;
    }

    // Bir objenin belirtilen Terrain üzerinde olup olmadýðýný kontrol eder
    bool IsOnTerrain(GameObject obj)
    {
        Vector3 objectPosition = obj.transform.position;
        TerrainData terrainData = terrain.terrainData;

        // Terrain'in dünya pozisyonu ve boyutunu hesapla
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrainData.size;

        // Objeyi Terrain alaný içinde kontrol et
        return objectPosition.x >= terrainPosition.x &&
               objectPosition.x <= terrainPosition.x + terrainSize.x &&
               objectPosition.z >= terrainPosition.z &&
               objectPosition.z <= terrainPosition.z + terrainSize.z;
    }
}