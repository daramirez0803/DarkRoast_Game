using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro elements

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image healthBarFill;
    public TextMeshProUGUI currencyText;
    public GameObject canvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("More than one UIManager instance found!");
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start()
    {
        SetHealth(1);
        UpdateCurrencyText();
    }

    public void SetHealth(float healthNormalized)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = healthNormalized;
        }
    }

    public void UpdateCurrencyText()
    {
        currencyText.text = "Coffee Beans: " + PersistentValues.instance.currency.ToString();
    }

    public void HideHUD() 
    {
        canvas.SetActive(false);
    }

    public void ShowHUD()
    {
        canvas.SetActive(true);
        UpdateCurrencyText();
    }
}
