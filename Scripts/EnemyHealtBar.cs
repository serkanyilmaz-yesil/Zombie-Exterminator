using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealtBar : MonoBehaviour
{

    public static EnemyHealtBar enemyBar;

    public float maximumHealt;
    public float currentHealt;
    public Image healtimage;
    public GameObject coin;
    public int coinCount;


    public bool die;
    public GameObject destroyEffect;

    private void Awake()
    {
        enemyBar = this;


        currentHealt = maximumHealt;
        die = false;
    }



    void Update()

    {
        HealtUi();
        ColorChange();

        if (currentHealt <=0 && !die)
        {
            Instantiate(destroyEffect, new Vector3(transform.position.x, transform.position.y+1f, transform.position.z) , Quaternion.identity);
            for (int i = 0; i < coinCount; i++)
            {
                Instantiate(coin, new Vector3(transform.position.x , transform.position.y + 2, transform.position.z),Quaternion.identity);
            }


            die = true;

        }


    }




    private void HealtUi()
    {
        healtimage.fillAmount = currentHealt / maximumHealt;
    }
    void ColorChange()
    {
        Color color = Color.Lerp(Color.red, Color.green, (currentHealt / maximumHealt));
        healtimage.color = color;
    }

    private void OnDestroy()
    {
        GameManager.gm.dieEnemy++;

    }

}

