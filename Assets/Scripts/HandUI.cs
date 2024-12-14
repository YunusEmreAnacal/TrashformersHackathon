using TMPro;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    private void Start()
    {
        PlayerStats.Instance.OnGetCoins += OnGetCoins;
    }

    private void OnGetCoins(int val)
    {
        tmp.SetText($"{val}");
    }

    private void OnDestroy()
    {
        PlayerStats.Instance.OnGetCoins -= OnGetCoins;
    }
}
