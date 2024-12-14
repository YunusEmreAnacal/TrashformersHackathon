using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int coins;
    public int totalTrash;
    public int collectedTrash;

    public Action<int> OnGetCoins;

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
}
