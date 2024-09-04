using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    private PlayerControls playerControls;
    private InputAction nextDialogue;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        playerControls = new PlayerControls();
        nextDialogue = playerControls.Menu.Confirm;
        nextDialogue.Enable();
        StartDialogue();
    }

    void OnEnable() {
        if (nextDialogue != null) {
            nextDialogue.Enable();
        }
    }

    void OnDisable() {
        if (nextDialogue != null) {
            nextDialogue.Disable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || nextDialogue.triggered)
        {
            if (index > lines.Length - 1) {
                return;
            }
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = -1;
        NextLine();
    }

    IEnumerator TypeLine()
    {
        if (index > lines.Length - 1) {
            yield break;
        }
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            if (textSpeed != 0) {
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            } else {
                textComponent.text = lines[index];
            }

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
