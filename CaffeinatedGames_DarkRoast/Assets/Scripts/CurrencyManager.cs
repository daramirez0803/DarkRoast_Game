using UnityEngine;
using TMPro; // Make sure to include this namespace

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public TextMeshProUGUI currencyText; // Use TextMeshProUGUI instead of Text
    private int currency = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one CurrencyManager instance found!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCurrencyText();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyText();
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = "Currency: " + currency.ToString();
    }
}
