using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Attack : MonoBehaviour
{

    public static Enemy1Attack enem;


    private NavMeshAgent agent;
    public float OverlapRadius = 500.0f;
    private float distance;
    private Transform player;

    private Animator anim;
    private Color orjColor;
    public bool die;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float attackRange;


    private float damage;
    private float damageRocket;



    private void Awake()
    {
        enem = this;
        agent = GetComponent<NavMeshAgent>();

    }


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        orjColor = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        EnemySpawner.spawner.amountEnemy++;

        damage = GameManager.gm.bulletDamage;
        damageRocket = GameManager.gm.damageRocket;
        die = false;
    }
    private void FixedUpdate()
    {
        if (!CharMove.ctrl.die && !die)
        {

            distance = Vector3.Distance(transform.position, player.position);

            if (distance < attackRange)
            {
                transform.LookAt(player);
                AttackPlayer();

            }
            else
                anim.SetBool("attack", false);


            agent.SetDestination(player.position);

        }

        die = gameObject.GetComponent<EnemyHealtBar>().die;

        if (die)
        {
            Destroy(gameObject);
        }


    }
    private void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            anim.SetBool("attack", true);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);


        }

    }

    private void ResetAttack()
    {

        alreadyAttacked = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            gameObject.GetComponent<EnemyHealtBar>().currentHealt -= damage;
            StartCoroutine(BulletCollision());
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Rocket"))
        {
            gameObject.GetComponent<EnemyHealtBar>().currentHealt -= damageRocket;
            StartCoroutine(BulletCollision());
            Destroy(other.gameObject);
        }

    }


    IEnumerator BulletCollision()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.white;
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = orjColor;


    }
}
