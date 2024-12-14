using UnityEngine;
using UnityEngine.UI;
using TMPro; // Text i�in

public class ShopManager : MonoBehaviour
{
    public ShopItemSO[] shopItems;           // Ma�aza item'lar�
    public Transform[] toolDisplaySlots;     // Tezgahtaki 3 pozisyon (slot)
    public TextMeshProUGUI[] priceTexts;     // Fiyat yaz�lar�
    public int[] prices;
    public Button[] purchaseButtons;         // Sat�n alma butonlar�

    public Transform toolSpawnPoint;         // Sat�n al�nan tool'un spawn olaca�� yer

    private void Start()
    {
        InitializeShop();
    }

    void InitializeShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            var item = shopItems[i];

            // Tool'u tezgahtaki slot'a yerle�tir
            //Instantiate(item.toolPrefab, toolDisplaySlots[i].position, Quaternion.identity);

            // Fiyatlar� yazd�r
            priceTexts[i].text = $"{item.price} Coins";

            // Butona t�klanma olay�n� ekle
            int index = i; // Lambda expression i�in
            purchaseButtons[i].onClick.AddListener(() => PurchaseItem(index));
            purchaseButtons[i].onClick.AddListener(() => Debug.Log($"{index}: BUTTON PRESSED"));
        }
    }

    void PurchaseItem(int index)
    {
        Debug.Log("XDDDD " + index);

        if (!PlayerStats.Instance.CheckCanPay(prices[index]))
        {
            return;
        }

        PlayerStats.Instance.SpendCoins(prices[index]);

        var item = shopItems[index];

        // Fiyat� SOLD yap
        priceTexts[index].text = "SOLD";

        // Butonu devre d��� b�rak
        purchaseButtons[index].interactable = false;

        FindAnyObjectByType<BagSystem>().UnlockModel(index);

        // Tool'u oyuncunun �n�ne spawnla
        Instantiate(item.toolPrefab, toolSpawnPoint.position, Quaternion.identity);
    }
}

