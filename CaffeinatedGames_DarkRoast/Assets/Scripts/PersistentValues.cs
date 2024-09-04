using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentValues : MonoBehaviour
{
    public OptionsMenu.Difficulty difficultyLevel = OptionsMenu.Difficulty.Normal;
    public float masterVolumeFloat = 100f;

    public int playerLevel = 1;
    public int playerHealthPoints = 1;
    public int playerStaminaPoints = 1;
    public int playerAttackPoints = 1;
    public int currency = 0;
    public static PersistentValues instance { get; private set; }

    public object this[string fieldName]
    {
        get
        {
            var field = this.GetType().GetField(fieldName);
            if (field == null)
            {
                Debug.LogError("Field not found: " + fieldName + " in PersistentValues");
            }
            return field.GetValue(this);
        }
        set
        {
            var field = this.GetType().GetField(fieldName);
            if (field == null)
            {
                Debug.LogError("Field not found: " + fieldName + " in PersistentValues");
            }
            field.SetValue(this, value);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Enable() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public int GetMaxHealth() {
        return StatsMenu.BASE_HEALTH + playerHealthPoints * StatsMenu.HEALTH_SCALING;
    }

    public int GetMaxStamina() {
        return StatsMenu.BASE_STAMINA + playerStaminaPoints * StatsMenu.STAMINA_SCALING;
    }

    public int GetAttack() {
        return StatsMenu.BASE_ATTACK + playerAttackPoints;
    }

    public void SetCurrencyToZero()
    {
        currency = 0;
    }

}
