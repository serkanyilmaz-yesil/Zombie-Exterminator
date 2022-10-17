using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner bs;

    public Transform[] spawnObj = new Transform[9];


    public GameObject spawned;
    public float delayTime, YAxis;
    public int countSpawn;
    public int maxCount;
    bool spawn = true;

    public int current;

    [HideInInspector]
    public int spawnindex;
    [HideInInspector]
    public GameObject newSpawn;
    public Transform spawnPos;



    private void Awake()
    {
        bs = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnObj.Length; i++)
        {
            spawnObj[i] = transform.GetChild(0).GetChild(i);
        }

    }



    IEnumerator RocketSpawn()
    {

        yield return new WaitForSeconds(delayTime);

        newSpawn = Instantiate(spawned, new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z), Quaternion.identity, transform.GetChild(1));
        newSpawn.transform.DOJump(new Vector3(spawnObj[spawnindex].position.x, spawnObj[spawnindex].position.y + YAxis, spawnObj[spawnindex].position.z), 10f, 1, 0.5f).SetEase(Ease.OutQuad);

        newSpawn.transform.DOScale(new Vector3(.45f, .45f, .45f), .5f).SetEase(Ease.OutElastic);


        Destroy(transform.GetChild(2).GetChild(0).gameObject);

        current--;


        if (spawnindex < 9)
        {
            spawnindex++;
            countSpawn++;
        }
        if (spawnindex >= 9)
        {
            spawnindex = 0;
            YAxis ++;

        }

        CharMove.ctrl.placeindex--;
        if (CharMove.ctrl.placeindex < 0)
        {
            CharMove.ctrl.placeindex = 8;
            CharMove.ctrl.YAxis -= .5f;
        }


        yield return new WaitForSeconds(delayTime);
        spawn = true;

    }


    // Update is called once per frame
    void Update()
    {
        if (countSpawn < maxCount)
        {
            if (spawn)
            {
                if (current > 0)
                {
                    StartCoroutine( RocketSpawn());
                    spawn = false;


                }

            }

        }


    }

}
