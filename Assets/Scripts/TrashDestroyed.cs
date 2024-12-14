using UnityEngine;

public class TrashDestroyed : MonoBehaviour
{
    [SerializeField] private int amount = 10;
    private void OnDestroy()
    {
        PlayerStats.Instance.GetCoins(amount);
    }
}
