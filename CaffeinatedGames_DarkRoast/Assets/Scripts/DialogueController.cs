using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueBox;
    public bool exitDialogue = false;
    public bool exitedCoffeeShop = false;
    public GameObject eventTriggered;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(true);
            exitDialogue = true;
            exitedCoffeeShop = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false);
            exitDialogue = false;
        }
    }
}
