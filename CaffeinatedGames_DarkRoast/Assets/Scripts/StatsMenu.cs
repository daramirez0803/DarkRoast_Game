using TMPro;
using System;
using UnityEngine;

[Serializable]
public class Stat {
    private int baseValue;
    private int scaling;
    public string pointField;
    public GameObject label;
    public GameObject valueText;
    public GameObject buttonLabel;


    public Stat(int baseValue, int scaling, string pointField, GameObject label, GameObject valueText, GameObject buttonLabel) {
        this.baseValue = baseValue;
        this.scaling = scaling;
        this.pointField = pointField;
        this.label = label;
        this.valueText = valueText;
        this.buttonLabel = buttonLabel;
    }

    public void setBaseAndScaling(int baseValue, int scaling) {
        this.baseValue = baseValue;
        this.scaling = scaling;
    }

    private int getPoints() {
        return (int)PersistentValues.instance[pointField];
    }

    private void setPoints(int value) {
        PersistentValues.instance[pointField] = value;
    }

    public void UpdateUI() {
        int points = getPoints();
        label.GetComponent<TMPro.TMP_Text>().text = "(lvl. " + getPoints() + ")";
        buttonLabel.GetComponent<TMPro.TMP_Text>().text = "Cost: " + StatsMenu.NextLevelCost().ToString();
        valueText.GetComponent<TMPro.TMP_Text>().text = "current: " + (points * scaling + baseValue).ToString() + ", next: " + ((points + 1) * scaling + baseValue).ToString();
    }

    public void LevelUp() {
        setPoints(getPoints() + 1);
        UpdateUI();
    }

    public void Reset() {
        setPoints(1);
    }
}

public class StatsMenu : MonoBehaviour
{
    public static int HEALTH_SCALING = 10;
    public static int STAMINA_SCALING = 5;

    public static int BASE_HEALTH = 30;
    public static int BASE_STAMINA = 20;
    public static int BASE_ATTACK = 5;
    private static int BASE_COST = 25;
    private static int SCALING_COST = 15;

    public Stat health;    
    public Stat stamina;
    public Stat attack;
    public GameObject currencyCount;
    public GameObject currentLevel;

    public void Start () {
        Debug.Log("Stats menu started");
        currencyCount.GetComponent<TMPro.TMP_Text>().text = "Coffee Beans: " + PersistentValues.instance.currency.ToString();
        currentLevel.GetComponent<TMPro.TMP_Text>().text = "Level " + PersistentValues.instance.playerLevel.ToString();
        health.setBaseAndScaling(BASE_HEALTH, HEALTH_SCALING);
        stamina.setBaseAndScaling(BASE_STAMINA, STAMINA_SCALING);
        attack.setBaseAndScaling(BASE_ATTACK, 1);
        UpdateUI();
    }
    public void OnEnable() {
        Debug.Log("Stats menu enabled");
        UpdateUI();
    }



    private void UpdateUI() {
        currencyCount.GetComponent<TMPro.TMP_Text>().text = "Coffee Beans: " + PersistentValues.instance.currency.ToString();
        currentLevel.GetComponent<TMPro.TMP_Text>().text = "Level " + PersistentValues.instance.playerLevel.ToString();
        health.UpdateUI();
        stamina.UpdateUI();
        attack.UpdateUI();
    }

    public static int NextLevelCost() {
        return BASE_COST + (PersistentValues.instance.playerLevel - 1) * SCALING_COST;
    }

    private void LevelStat(Stat stat) {
        if (PersistentValues.instance.currency >= NextLevelCost()) {
            PersistentValues.instance.currency -= NextLevelCost();
            stat.LevelUp();
            PersistentValues.instance.playerLevel++;
            UpdateUI();
        }
    }

    public void LevelHealth() {
        LevelStat(health);
    }

    public void LevelStamina() {
        LevelStat(stamina);
    }

    public void LevelAttack() {
        LevelStat(attack);
    }

    public void Respec() {
        int levelsLost = (PersistentValues.instance.playerLevel - 1);
        health.Reset();
        stamina.Reset();
        attack.Reset();

        PersistentValues.instance.playerLevel = 1;
        int baseCostRefund = levelsLost * BASE_COST;
        int scalingCostRefund = levelsLost * (levelsLost - 1) / 2 * SCALING_COST;

        PersistentValues.instance.currency += baseCostRefund + scalingCostRefund;

        currencyCount.GetComponent<TMPro.TMP_Text>().text = "Coffee Beans: " + PersistentValues.instance.currency.ToString();
        UpdateUI();
    }
}