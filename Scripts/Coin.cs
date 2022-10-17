using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Transform player;

    public Ease easeType;

    // Start is called before the first frame update
    void Start()
    {

        Invoke("Delay", .5f);

        Destroy(gameObject, 5);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player = GameObject.Find("Player").transform;
        Invoke("LerpDelay", 1.5f);
    }

    void Delay()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        transform.DOJump(player.position, 15, 1, .5f).SetEase(easeType);

    }

    void LerpDelay()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, 10 * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameManager.gm.coin++;

    }
}
