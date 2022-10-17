using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower4Attack : MonoBehaviour
{
    public static Tower4Attack tower4;

    public float OverlapRadius = 100.0f;

    private Transform nearestEnemy;
    private int enemyLayer;

    public float bulletSpeed;
    public Transform attackPoint;

    public int current;



    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    //public GameObject dustTrail;

    public float attackRange;

    private Vector3 lookPos;


    private void Awake()
    {
        tower4 = this;
    }
    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");

    }
    private void FixedUpdate()
    {
        if (!CharMove.ctrl.die)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, OverlapRadius, 1 << enemyLayer);
            float minimumDistance = Mathf.Infinity;


            foreach (Collider collider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    nearestEnemy = collider.transform;

                }

            }
            if (minimumDistance < attackRange)
            {
                attackPoint.LookAt(nearestEnemy);

                if (current > 0)
                {
                    AttackPlayer();
                }
            }

        }




    }
    private void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            current--;

            StartCoroutine(AttackDelay());

            Destroy(transform.GetChild(1).GetChild(0).gameObject, 2);

            CharMove.ctrl.placeindexBullet4--;
            if (CharMove.ctrl.placeindexBullet4 < 0)
            {
                CharMove.ctrl.placeindexBullet4 = 8;
                CharMove.ctrl.YAxisBullet4--;
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);


        }

    }

    private void ResetAttack()
    {

        alreadyAttacked = false;
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 5; i++)
        {
            Rigidbody rb = Instantiate(projectile, attackPoint.position, attackPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(attackPoint.transform.forward * bulletSpeed);
            yield return new WaitForSeconds(1);
        }

    }

}
