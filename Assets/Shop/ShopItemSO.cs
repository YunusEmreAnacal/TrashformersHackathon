using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/Item")]
public class ShopItemSO : ScriptableObject
{
    public string toolName;          // Tool ismi
    public int price;                // Tool fiyat�
    public GameObject toolPrefab;    // Tutulabilir prefab
}
