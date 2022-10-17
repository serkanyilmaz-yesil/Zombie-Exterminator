using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DollarSpawner : MonoBehaviour
{
    public static DollarSpawner sp;

    public Transform[] spawnObj = new Transform[9];
    public GameObject spawned,animObj;
    public float delayTime, YAxis;
    public int countSpawn ;
    public int maxCount;
    bool spawn = true;

    [HideInInspector]
    public int spawnindex;
    [HideInInspector]
    public GameObject newSpawn;
    public Transform spawnPos;

    private void Awake()
    {
        sp = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnObj.Length; i++)
        {
            spawnObj[i] = transform.GetChild(0).GetChild(i);
        }

    }

    

    public void DollarSpawn()
    {
        newSpawn = Instantiate(spawned, new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z), Quaternion.identity, transform.GetChild(1));
        newSpawn.transform.DOJump(new Vector3(spawnObj[spawnindex].position.x, spawnObj[spawnindex].position.y + YAxis, spawnObj[spawnindex].position.z), 10f, 1, 0.5f).SetEase(Ease.OutQuad);

        if (spawnindex < 9)
        {
            spawnindex++;
            countSpawn++;
        }
        if (spawnindex >= 9)
        {
            spawnindex = 0;
            YAxis += .5f;

        }
        animObj.GetComponent<Animator>().enabled = true;
        maxCount --;


    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        spawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (maxCount > 0)
        {
            animObj.GetComponent<Animator>().enabled = true;
            if (spawn)
            {
                DollarSpawn();
                spawn = false;
                StartCoroutine(Delay());
            }

        }
        else
        {
            animObj.GetComponent<Animator>().enabled = false;

        }
    }
}
