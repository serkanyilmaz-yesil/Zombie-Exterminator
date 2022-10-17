using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NewSector : MonoBehaviour
{
    public static NewSector newS;

    public int sectorDollar;
    public TextMeshProUGUI sectorDollarText;
    public GameObject newSector,canvas,door;

    private void Awake()
    {
        newS = this;
    }

    void Update()
    {
        sectorDollarText.text = sectorDollar.ToString();
        if (sectorDollar <= 0)
        {
            Invoke("Delay", 1.5f);
        }

    }

    void Delay()
    {
        newSector.SetActive(true);
        canvas.SetActive(false);
        door.SetActive(false);
        newSector.transform.DOScale(new Vector3(1f, 1f, 1f), .2f).SetEase(Ease.OutElastic);

    }
}
