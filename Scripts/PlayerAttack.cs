using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float OverlapRadius = 100.0f;

    private Transform nearestEnemy;
    private int enemyLayer;

    public float bulletSpeed;
    public float minimumDistance;
    public Transform attackPoint;


    //private Animator animator;


    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    //public GameObject dustTrail;

    public float attackRange;

    private Vector3 lookPos;

    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        //dustTrail.SetActive(false);

    }
    private void FixedUpdate()
    {
        if (!CharMove.ctrl.die)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, OverlapRadius, 1 << enemyLayer);
            minimumDistance = Mathf.Infinity;


            foreach (Collider collider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    nearestEnemy = collider.transform;

                }

            }

            if (minimumDistance <= 12)
            {
                PlayerHealtBar.hb.timeControl = true;
            }
            else
            {
                PlayerHealtBar.hb.timeControl = false;
                if (PlayerHealtBar.hb.currentHealt < (PlayerHealtBar.hb.maximumHealt))
                {
                    PlayerHealtBar.hb.currentHealt += Time.deltaTime;
                }

            }

        }


    }

    private void Update()
    {
        if (minimumDistance < attackRange && nearestEnemy != null)
        {
            transform.LookAt(new Vector3(nearestEnemy.position.x, 1, nearestEnemy.position.z));
            attackPoint.LookAt(nearestEnemy);
            AttackPlayer();
            //dustTrail.SetActive(false);
        }
        else
            transform.eulerAngles = new Vector3(CharMove.ctrl.transform.eulerAngles.x, CharMove.ctrl.transform.eulerAngles.y, CharMove.ctrl.transform.eulerAngles.z);

    }
    private void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            //Instantiate(projectile, attackPoint.position, attackPoint.rotation);
            Rigidbody rb = Instantiate(projectile, attackPoint.position, attackPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(attackPoint.transform.forward * bulletSpeed);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);


        }

    }

    private void ResetAttack()
    {

        alreadyAttacked = false;
    }


}
