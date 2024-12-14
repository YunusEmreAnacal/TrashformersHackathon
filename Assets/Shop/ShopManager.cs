using UnityEngine;
using UnityEngine.UI;
using TMPro; // Text için

public class ShopManager : MonoBehaviour
{
    public ShopItemSO[] shopItems;           // Maðaza item'larý
    public Transform[] toolDisplaySlots;     // Tezgahtaki 3 pozisyon (slot)
    public TextMeshProUGUI[] priceTexts;     // Fiyat yazýlarý
    public Button[] purchaseButtons;         // Satýn alma butonlarý

    public Transform toolSpawnPoint;         // Satýn alýnan tool'un spawn olacaðý yer

    private void Start()
    {
        InitializeShop();
    }

    void InitializeShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            var item = shopItems[i];

            // Tool'u tezgahtaki slot'a yerleþtir
            Instantiate(item.toolPrefab, toolDisplaySlots[i].position, Quaternion.identity);

            // Fiyatlarý yazdýr
            priceTexts[i].text = $"{item.price} Coins";

            // Butona týklanma olayýný ekle
            int index = i; // Lambda expression için
            purchaseButtons[i].onClick.AddListener(() => PurchaseItem(index));
        }
    }

    void PurchaseItem(int index)
    {
        var item = shopItems[index];

        // Fiyatý SOLD yap
        priceTexts[index].text = "SOLD";

        // Butonu devre dýþý býrak
        purchaseButtons[index].interactable = false;

        // Tool'u oyuncunun önüne spawnla
        Instantiate(item.toolPrefab, toolSpawnPoint.position, Quaternion.identity);
    }
}

