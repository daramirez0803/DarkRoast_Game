using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance { get; private set; }
    public float currentHealth;
    public float difficultyMultiplier;
    private AudioSource healthDrinkSound;
    public AudioClip clip1;

    private bool isDead = false;

    void Start()
    {
        currentHealth = PersistentValues.instance.GetMaxHealth();
        UpdateHealthUI();
        healthDrinkSound = GetComponent<AudioSource>();
    }

    void OnEnable() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one PlayerHealth instance found.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Set the difficulty multiplier based off the selection made in the Options Menu and saved to PersistentValues
        if ( (int)PersistentValues.instance.difficultyLevel == 0)
        {
            difficultyMultiplier = 0.75f;
        }

        if ((int)PersistentValues.instance.difficultyLevel == 1)
        {
            difficultyMultiplier = 1.00f;
        }

        if ((int)PersistentValues.instance.difficultyLevel == 2)
        {
            difficultyMultiplier = 1.25f;
        }
    }
    public void TakeDamage(float amount)
    {
        amount = amount * difficultyMultiplier; // damage adjusted by difficulty
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, PersistentValues.instance.GetMaxHealth());
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        if (currentHealth < PersistentValues.instance.GetMaxHealth())
        {
            amount = amount / difficultyMultiplier; // healing adjusted by difficulty
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, PersistentValues.instance.GetMaxHealth());
            UpdateHealthUI();
            healthDrinkSound.clip = clip1;
            healthDrinkSound.volume = 1f;
            healthDrinkSound.Play();
        }
    }

    void UpdateHealthUI()
    {
        UIManager.Instance.SetHealth(currentHealth / PersistentValues.instance.GetMaxHealth());
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            WinLossManager.Instance.GameOver();
        }
    }
}
