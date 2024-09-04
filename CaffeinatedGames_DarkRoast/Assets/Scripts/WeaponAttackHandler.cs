using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackHandler : MonoBehaviour
{
    private float flatDamage = 10;
    private float attackMultiplier = 1.0f;
    public float strMultiplier = 1.0f;
    public float dexMultiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") {
            //Debug.Log("hit " + other.gameObject.name + " for " + Damage() + " damage");
            other.GetComponent<ZombieAI>().TakeDamage(Damage());
        }
    }

    public float Damage()
    {
        float attack = PersistentValues.instance.GetAttack();
        return (flatDamage + attack) * attackMultiplier;
    }

    public void HitboxActive(float attackMultiplier) {
        this.attackMultiplier = attackMultiplier;
        GetComponent<Collider>().enabled = true;
    }

    public void HitboxInactive() {
        GetComponent<Collider>().enabled = false;
    }
}
