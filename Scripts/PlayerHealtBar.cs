using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerHealtBar : MonoBehaviour
{

    public static PlayerHealtBar hb;

    public float maximumHealt;
    public float currentHealt;
    public Image healtimage;
    public TextMeshProUGUI healtText;

    public float enem1Damage;

    private Transform cam;

    public float time;
    public bool timeControl;


    private void Awake()
    {
        hb = this;

        cam = Camera.main.transform;

        if (PlayerPrefs.HasKey("currentHealt"))
        {
            currentHealt = PlayerPrefs.GetFloat("currentHealt");
        }
        else
            currentHealt = maximumHealt;

        
    }

    private void Start()
    {
        timeControl = false;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    void Update()

    {
        HealtUi();
        ColorChange();

        if (timeControl)
        {
            time += Time.deltaTime;
            if (time >= 2f )
            {
                currentHealt -= enem1Damage;
                time = 0;
            }

        }

        if (currentHealt <= 0)
        {
            CharMove.ctrl.die = true;
            currentHealt = 0;
        }

        DisplayTime((int)currentHealt);

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


    void DisplayTime(int timeToDisplay)
    {
        healtText.text = "" + timeToDisplay;
    }

}

