using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusReporter : MonoBehaviour
{
    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void SetStatusDead()
    {
        isDead = true;
    }
}
