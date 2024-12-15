using TMPro;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private TextMeshProUGUI trashTmp;

    private void Start()
    {
        PlayerStats.Instance.OnGetCoins += OnGetCoins;
        PlayerStats.Instance.OnCollectTrash += OnCollectTrash;

        trashTmp.SetText($"{0}/{PlayerStats.Instance.totalTrash}");
        tmp.SetText($"{PlayerStats.Instance.coins}");
    }

    private void OnGetCoins(int val)
    {
        tmp.SetText($"{val}");
    }

    private void OnCollectTrash(int collectedTrash)
    {
        trashTmp.SetText($"{collectedTrash}/{PlayerStats.Instance.totalTrash}");
    }

    private void OnDestroy()
    {
        PlayerStats.Instance.OnGetCoins -= OnGetCoins;
        PlayerStats.Instance.OnCollectTrash -= OnCollectTrash;

        
    }
}
