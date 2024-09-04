using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : MonoBehaviour
{
    public Image StaminaBar;

    public float Stamina;
    public float AttackCost = 15f;
    public float DodgeCost = 10f;
    public float StaminaRegenerationRate = 20f;
    private float LastStaminaDepletionTime = 0f;
    public float StaminaRegenerationDelay = 1f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Stamina = PersistentValues.instance.GetMaxStamina();
        UpdateStaminaBar();
    }

    void Update()
    {
        animator.SetFloat("Stamina", Stamina);
        UpdateStaminaBar();
    }

    public void HandleAttacking()
    {
        ReduceStamina(AttackCost);
    }

    public void HandleDodge() {
        ReduceStamina(DodgeCost);
    }

    void ReduceStamina(float amount)
    {
        Stamina -= amount;
        Stamina = Mathf.Max(Stamina, 0);
        LastStaminaDepletionTime = Time.time;
    }

    void RegenerateStamina(float amount)
    {
        Stamina += amount;
        Stamina = Mathf.Min(Stamina, PersistentValues.instance.GetMaxStamina());
    }

    void UpdateStaminaBar()
    {
        if (Time.time - LastStaminaDepletionTime > StaminaRegenerationDelay) {
            RegenerateStamina(StaminaRegenerationRate * Time.deltaTime);
        }
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = Stamina / PersistentValues.instance.GetMaxStamina();
        }
    }
}
