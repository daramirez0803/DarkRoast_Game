using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown difficultyDropDown;
    [SerializeField] private GameObject _OKButton;

    public enum Difficulty {Easy, Normal, Hard}

    private void Start()
    {
        difficultyDropDown.value = ((int)PersistentValues.instance.difficultyLevel);
    }

    public void SaveOptions()
    {
        PersistentValues.instance.difficultyLevel = (Difficulty)difficultyDropDown.value;
    }
}