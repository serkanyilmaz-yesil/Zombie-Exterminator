using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner spawner;

    public Transform[] spawnPoint;
    public GameObject[] enemy;
    public int amountEnemy;
    public int maxAmount;

    private bool spawn;
    public float timeBetweenSpawn;
    public float spawnRestartTime;

    private void Awake()
    {
        spawner = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!CharMove.ctrl.die)
        {
            spawnRestartTime += Time.deltaTime;
            if (spawnRestartTime >= 25)
            {
                spawnRestartTime = 0;
            }

            if (amountEnemy < maxAmount)
            {
                if (spawnRestartTime < 10)
                {
                    EnemySpawn();

                }
            }


        }


    }

    void EnemySpawn()
    {
        if (!spawn)
        {
            Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPoint[Random.Range(0, spawnPoint.Length)].position, transform.rotation);
            spawn = true;
            Invoke(nameof(ResetAttack), timeBetweenSpawn);

        }
    }
    private void ResetAttack()
    {

        spawn = false;
    }

}
