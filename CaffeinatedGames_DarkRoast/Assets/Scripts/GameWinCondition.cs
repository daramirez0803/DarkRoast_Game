using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinCondition : MonoBehaviour
{
    private EnemyStatusReporter status;
    private bool isGameWon;

    private void Start()
    {
        status = GetComponent<EnemyStatusReporter>();
    }
    void Update()
    {
        if (status.IsDead() && !isGameWon)
        {
            isGameWon = true;
            WinLossManager.Instance.GameWon();
        }
            
    }
}
