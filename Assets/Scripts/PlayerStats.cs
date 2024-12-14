using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int coins;
    public int totalTrash;
    public int collectedTrash;

    public Action<int> OnGetCoins;
    public Action<int> OnCollectTrash;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    private void Start()
    {
        collectedTrash = 0;
    }

    public bool CheckCanPay(int amount)
    {
        if (amount <= coins)
        {
            return true;
        }

        return false;
    }

    public void GetCoins(int amount)
    {
        coins += amount;
        OnGetCoins?.Invoke(coins);
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
    }

    public void CollectTrash()
    {
        Debug.Log("COLLECTING TRASH");

        collectedTrash++;
        OnCollectTrash?.Invoke(collectedTrash);

        if (collectedTrash >= totalTrash) { SceneManager.LoadScene("StartScene"); }
    }
}
