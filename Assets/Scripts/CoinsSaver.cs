using UnityEngine;
using UnityEngine.UI;

public class CoinsSaver : MonoBehaviour
{
    [SerializeField] private Text TextWithQuantityOfCoins;

    private int CurrentQuantityOfCoins;

    private void Awake()
    {
        CurrentQuantityOfCoins = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentCoinsQuantity);
        UpdateVisual();
    }

    public void IncreaseCoinsQuantity(int delta)
    {
        CurrentQuantityOfCoins += delta;
        UpdateVisual();
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentCoinsQuantity, CurrentQuantityOfCoins);
    }

    private void UpdateVisual()
    {
        TextWithQuantityOfCoins.text = CurrentQuantityOfCoins.ToString();
    }
}