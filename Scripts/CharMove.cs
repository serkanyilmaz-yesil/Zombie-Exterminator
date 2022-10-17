using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class CharMove : MonoBehaviour
{
    public static CharMove ctrl;

    [SerializeField]
    Joystick Joystick;
    public float speed;
    private Rigidbody rb;
    private Animator anim;
    public Transform stackPoint;
    public Transform rocketPoint;
    //public GameObject dustTrail;
    public bool die = false;
    [HideInInspector]
    public float YAxis,YAxisBullet, YAxisBullet2, YAxisBullet3, YAxisBullet4;
    public int maxCount;
    private bool maxY;

    [HideInInspector]
    public float delay1, delay2, delay3, delay4, delay5, delay6, delay7;



    public List<Transform> spawneds = new List<Transform>();
    public List<Transform> rocketSpawneds = new List<Transform>();
    public Transform[] placedObj = new Transform[9];
    public Transform[] placedBullet1 = new Transform[9];
    public Transform[] placedBullet2 = new Transform[9];
    public Transform[] placedBullet3 = new Transform[9];
    public Transform[] placedBullet4 = new Transform[9];
    public GameObject bulletStation;
    public GameObject towerStation;
    public GameObject towerStation2;
    public GameObject towerStation3;
    public GameObject towerStation4;
    public int placeindex;
    public int placeindexBullet;
    public int placeindexBullet2;
    public int placeindexBullet3;
    public int placeindexBullet4;
    public GameObject dustTrail;

    private int childName;

    //private AudioSource sound;
    //private AudioClip dieSound;
    //bool dieSoundd = false;

    //public GameObject buttons;

    public List<GameObject> coinList = new List<GameObject>();
    public GameObject coin;

    public float OverlapRadius = 100.0f;

    private Transform target;
    private int targetLayer;
    public float targetRange;

    private float minimumDistance;
    private float distance;

    private void Awake()
    {
        ctrl = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        spawneds.Add(stackPoint);
        rocketSpawneds.Add(rocketPoint);
        //dustTrail.SetActive(false);
        //sound = GetComponent<AudioSource>();
        //dieSound = Resources.Load<AudioClip>("playerDie");
        for (int i = 0; i < placedObj.Length; i++)
        {
            placedObj[i] = bulletStation.transform.GetChild(0).GetChild(i);
        }

        for (int i = 0; i < placedBullet1.Length; i++)
        {
            placedBullet1[i] = towerStation.transform.GetChild(0).GetChild(i);
        }

        for (int i = 0; i < placedBullet2.Length; i++)
        {
            placedBullet2[i] = towerStation2.transform.GetChild(0).GetChild(i);
        }
        for (int i = 0; i < placedBullet3.Length; i++)
        {
            placedBullet3[i] = towerStation3.transform.GetChild(0).GetChild(i);
        }
        for (int i = 0; i < placedBullet4.Length; i++)
        {
            placedBullet4[i] = towerStation4.transform.GetChild(0).GetChild(i);
        }



        maxY = false;

        targetLayer = LayerMask.NameToLayer("Target");
    }


    private void FixedUpdate()
    {

        if (spawneds.Count > 0)
        {
            for (int i = 1; i < spawneds.Count; i++)
            {
                var firstSpawn = spawneds.ElementAt(i - 1);
                var secondSpawn = spawneds.ElementAt(i);

                secondSpawn.position = new Vector3(Mathf.Lerp(secondSpawn.position.x, firstSpawn.position.x, Time.deltaTime * 20f),
                                        Mathf.Lerp(secondSpawn.position.y, firstSpawn.position.y + 1f, Time.deltaTime * 20f), firstSpawn.position.z);
            }
        }

        if (rocketSpawneds.Count > 0)
        {
            for (int i = 1; i < rocketSpawneds.Count; i++)
            {
                var firstSpawn = rocketSpawneds.ElementAt(i - 1);
                var secondSpawn = rocketSpawneds.ElementAt(i);

                secondSpawn.position = new Vector3(Mathf.Lerp(secondSpawn.position.x, firstSpawn.position.x, Time.deltaTime * 20f),
                                        Mathf.Lerp(secondSpawn.position.y, firstSpawn.position.y + 2f, Time.deltaTime * 20f), firstSpawn.position.z);
            }
        }

        Target();


        if (!die)
        {
            Hareket();
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, OverlapRadius, 1 << targetLayer);
        minimumDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders)
        {
            distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                target = collider.transform;
            }
        }
        
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -131f, 131f);
        pos.z = Mathf.Clamp(pos.z, -122f, 73.5f);
        transform.position = pos;

        if (die)
        {
            speed = 0;
            rb.velocity = Vector3.zero;
            anim.SetBool("run", false);
            anim.SetBool("idle", false);
            anim.SetBool("die", true);

        }

    }

    void Hareket()
    {
        rb.velocity = new Vector3(Joystick.Horizontal * speed * Time.deltaTime, rb.velocity.y, Joystick.Vertical * speed * Time.deltaTime);
        if (Joystick.Horizontal != 0f || Joystick.Vertical != 0f)
        {
            if (rb.velocity !=Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
            anim.SetBool("run", true);
            dustTrail.SetActive(true);
        }
        else
        {
            anim.SetBool("run", false);
            dustTrail.SetActive(false);
        }
    }

    void Target()
    {
        if (target != null)
        {
            if (minimumDistance < targetRange)
            {
                if (target.CompareTag("NewSector"))
                {
                    for (var index = spawneds.Count - 1; index >= 1; index--)
                    {

                        if (target.gameObject.GetComponent<NewSector>().sectorDollar > 0)
                        {
                            target.GetComponent<NewSector>().sectorDollar--;

                            spawneds[index].DOJump(target.position, 10f, 1, .1f).SetDelay(delay1).SetEase(Ease.Unset);
                            delay1 += 0.2f;

                            spawneds.ElementAt(index).parent = target;
                            Destroy(spawneds[index].gameObject, 3);
                            spawneds.RemoveAt(index);
                        }


                    }



                }
                else
                    delay1 = 0;

                if (target.CompareTag("DollarCoin"))
                {
                    if (GameManager.gm.coin > 0)
                    {
                        GameObject coinClone = Instantiate(coin);
                        coinClone.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                        coinList.Add(coinClone);

                        for (var index = coinList.Count - 1; index >= 0; index--)
                        {
                            coinList[index].transform.DOJump(target.position, 20f, 1, .1f).SetDelay(delay2).SetEase(Ease.Unset);
                            delay2 += 0.05f;
                            DollarSpawner.sp.maxCount += GameManager.gm.coin / 20;

                            GameManager.gm.coin--;

                            Destroy(coinList[coinList.Count - 1], 3);
                            coinList.RemoveAt(index);
                        }
                    }
                }
                else
                    delay2 = 0;



                if (target.CompareTag("BulletStation"))
                {
                    maxY = false;
                    /*
                    sadece yukarý
                    if (target.childCount > 0)
                    {
                        YAxis = target.GetChild(target.childCount - 1).position.y;
                    }
                    else
                    {
                        YAxis = target.position.y;
                    }
                    */
                    for (var index = spawneds.Count - 1; index >= 1; index--)
                    {
                        spawneds[index].DOJump(new Vector3(placedObj[placeindex].position.x, placedObj[placeindex].position.y + YAxis, placedObj[placeindex].position.z), 20f, 1, .1f).SetDelay(delay3).SetEase(Ease.Unset);
                        delay3 += 0.2f;

                        spawneds[index].name = spawneds[index].name + childName;
                        childName++;
                        spawneds.ElementAt(index).parent = target.GetChild(2);
                        spawneds.RemoveAt(index);
                        if (placeindex < 8)
                        {
                            placeindex++;
                        }
                        else
                        {
                            placeindex = 0;
                            YAxis += .5f;
                        }
                        target.GetComponent<BulletSpawner>().current++;

                    }
                }
                else
                    delay3 = 0;



                if (target.CompareTag("RocketStation") && rocketSpawneds.Count < maxCount)
                {
                    if (target.childCount > 0)
                    {
                        var spawnRocket = target.GetChild(target.childCount - 1);
                        rocketSpawneds.Add(spawnRocket);
                        spawnRocket.parent = null;


                        if (target.parent.GetComponent<BulletSpawner>().countSpawn > 0)
                        {
                            target.parent.GetComponent<BulletSpawner>().countSpawn--;
                        }

                        target.parent.GetComponent<BulletSpawner>().spawnindex--;
                        if (target.parent.GetComponent<BulletSpawner>().spawnindex < 0)
                        {
                            target.parent.GetComponent<BulletSpawner>().spawnindex = 8;
                        }
                    }
                    if (rocketSpawneds.Count >= maxCount && !maxY)
                    {
                        if (target.parent.GetComponent<BulletSpawner>().YAxis > 0)
                        {
                            target.parent.GetComponent<BulletSpawner>().YAxis--;
                        }
                        maxY = true;
                    }



                }
                if (target.CompareTag("DollarStation") && spawneds.Count < maxCount)
                {
                    if (target.childCount > 0)
                    {
                        var spawn = target.GetChild(target.childCount - 1);
                        spawneds.Add(spawn);
                        spawn.parent = null;


                        if (target.parent.GetComponent<DollarSpawner>().countSpawn > 0)
                        {
                            target.parent.GetComponent<DollarSpawner>().countSpawn--;
                        }

                        target.parent.GetComponent<DollarSpawner>().spawnindex--;
                        if (target.parent.GetComponent<DollarSpawner>().spawnindex < 0)
                        {
                            target.parent.GetComponent<DollarSpawner>().spawnindex = 8;
                        }
                    }
                    if (spawneds.Count >= maxCount && !maxY)
                    {
                        if (target.parent.GetComponent<DollarSpawner>().YAxis > 0)
                        {
                            target.parent.GetComponent<DollarSpawner>().YAxis -= .5f;
                        }
                        maxY = true;
                    }
                }

                if (target.CompareTag("TowerStation"))
                {
                    for (var index = rocketSpawneds.Count - 1; index >= 1; index--)
                    {
                        rocketSpawneds[index].DOJump(new Vector3(placedBullet1[placeindexBullet].position.x, placedBullet1[placeindexBullet].position.y + YAxisBullet, placedBullet1[placeindexBullet].position.z), 20f, 1, .1f).SetDelay(delay4).SetEase(Ease.Unset);
                        delay4 += 0.2f;

                        rocketSpawneds.ElementAt(index).parent = target.GetChild(1);
                        rocketSpawneds.RemoveAt(index);
                        if (placeindexBullet < 8)
                        {
                            placeindexBullet++;
                        }
                        else
                        {
                            placeindexBullet = 0;
                            YAxisBullet += 1f;
                        }

                        target.GetComponent<TowerAttack>().current++;

                    }
                }
                else
                    delay4 = 0;


                if (target.CompareTag("TowerStation2"))
                {
                    for (var index = rocketSpawneds.Count - 1; index >= 1; index--)
                    {
                        rocketSpawneds[index].DOJump(new Vector3(placedBullet2[placeindexBullet2].position.x, placedBullet2[placeindexBullet2].position.y + YAxisBullet2, placedBullet2[placeindexBullet2].position.z), 20f, 1, .1f).SetDelay(delay5).SetEase(Ease.Unset);
                        delay5 += 0.2f;

                        rocketSpawneds.ElementAt(index).parent = target.GetChild(1);
                        rocketSpawneds.RemoveAt(index);
                        if (placeindexBullet2 < 8)
                        {
                            placeindexBullet2++;
                        }
                        else
                        {
                            placeindexBullet2 = 0;
                            YAxisBullet2 += 1f;
                        }
                        target.GetComponent<Tower2Attack>().current++;

                    }



                }
                else
                    delay5 = 0;


                if (target.CompareTag("TowerStation3"))
                {
                    for (var index = rocketSpawneds.Count - 1; index >= 1; index--)
                    {
                        rocketSpawneds[index].DOJump(new Vector3(placedBullet3[placeindexBullet3].position.x, placedBullet3[placeindexBullet3].position.y + YAxisBullet3, placedBullet3[placeindexBullet3].position.z), 20f, 1, .1f).SetDelay(delay6).SetEase(Ease.Unset);
                        delay6 += 0.2f;

                        rocketSpawneds.ElementAt(index).parent = target.GetChild(1);
                        rocketSpawneds.RemoveAt(index);
                        if (placeindexBullet3 < 8)
                        {
                            placeindexBullet3++;
                        }
                        else
                        {
                            placeindexBullet3 = 0;
                            YAxisBullet3 += 1f;
                        }
                        target.GetComponent<Tower3Attack>().current++;

                    }



                }
                else
                    delay6 = 0;


                if (target.CompareTag("TowerStation4"))
                {
                    for (var index = rocketSpawneds.Count - 1; index >= 1; index--)
                    {
                        rocketSpawneds[index].DOJump(new Vector3(placedBullet4[placeindexBullet4].position.x, placedBullet4[placeindexBullet4].position.y + YAxisBullet4, placedBullet4[placeindexBullet4].position.z), 20f, 1, .1f).SetDelay(delay7).SetEase(Ease.Unset);
                        delay7 += 0.2f;

                        rocketSpawneds.ElementAt(index).parent = target.GetChild(1);
                        rocketSpawneds.RemoveAt(index);
                        if (placeindexBullet4 < 8)
                        {
                            placeindexBullet4++;
                        }
                        else
                        {
                            placeindexBullet4 = 0;
                            YAxisBullet4 += 1f;
                        }
                        target.GetComponent<Tower4Attack>().current++;

                    }



                }
                else
                    delay7 = 0;

            }


        }

    }


}
