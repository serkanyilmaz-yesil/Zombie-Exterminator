using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public int bulletDamage;
    public int damageRocket;
    public int coin;
    public TextMeshProUGUI coinText,levelText,startText,endText,nextLevelTimeText;
    public int startLevel,endLevel;
    public int level;
    public GameObject nextLevelPanel;
    public float nextLevelTime;
    public bool nextLevel;
    public GameObject restartButton;
    public bool bossDie;

    public int dieEnemy;
    public Image mask;

    private void Awake()
    {
        gm = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
        nextLevelPanel.SetActive(false);
        nextLevel = false;
        restartButton.SetActive(false);
        bossDie = false;
        GameAnalytics.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Save();
        coinText.text = coin.ToString();
        startText.text = startLevel.ToString();
        endText.text = endLevel.ToString();
        levelText.text = "Level : " + startLevel.ToString();

        GetCurrentFill();

        if (dieEnemy >= EnemySpawner.spawner.maxAmount)
        {
            nextLevel = true;
            bossDie = true;
        }

        NextLevel();

        if (CharMove.ctrl.die)
        {
            Invoke("RestartDelay", 3);
        }
    }


    public void Save()
    {
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.SetInt("startLevel", startLevel);
        PlayerPrefs.SetInt("endLevel", endLevel);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("maximumEnemy", EnemySpawner.spawner.maxAmount);
        PlayerPrefs.SetInt("dieEnemy", dieEnemy);
        PlayerPrefs.SetInt("maxDollar", DollarSpawner.sp.maxCount + DollarSpawner.sp.countSpawn + BulletSpawner.bs.countSpawn + BulletSpawner.bs.current);


    }


    public void Load()
    {
        if (PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        if (PlayerPrefs.HasKey("startLevel"))
        {
            startLevel = PlayerPrefs.GetInt("startLevel");
        }
        if (PlayerPrefs.HasKey("endLevel"))
        {
            endLevel = PlayerPrefs.GetInt("endLevel");
        }
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        if (PlayerPrefs.HasKey("maximumEnemy"))
        {
            EnemySpawner.spawner.maxAmount = PlayerPrefs.GetInt("maximumEnemy");
        }
        if (PlayerPrefs.HasKey("dieEnemy"))
        {
            dieEnemy = PlayerPrefs.GetInt("dieEnemy");
        }

        if (PlayerPrefs.HasKey("maxDollar"))
        {
            DollarSpawner.sp.maxCount = PlayerPrefs.GetInt("maxDollar");
        }


    }

    void GetCurrentFill()
    {
        float fillAmount = (float)dieEnemy / (float)EnemySpawner.spawner.maxAmount;
        mask.fillAmount = fillAmount;
    }

    
    void DisplayTime(int timeToDisplay)
    {
        nextLevelTimeText.text = "Next Attack :" + timeToDisplay;
    }

    public void NextLevel()
    {
        if (nextLevel)
        {

            nextLevelPanel.SetActive(true);
            nextLevelTime -= Time.deltaTime;
            DisplayTime((int)nextLevelTime);
            if (nextLevelTime <= 0)
            {
                AdManager.ads.ShowIntersitialAds();

                nextLevelPanel.SetActive(false);
                nextLevelTime = 20;
                startLevel++;
                endLevel++;
                level++;
                dieEnemy = 0;
                EnemySpawner.spawner.amountEnemy = 0;
                EnemySpawner.spawner.maxAmount += 5;

                nextLevel = false;

            }
            bossDie = false;

        }
    }

    public void RestartLevel()
    {
        dieEnemy = 0;
        EnemySpawner.spawner.amountEnemy = 0;
        SceneManager.LoadScene(0);
    }

    void RestartDelay()
    {
        restartButton.SetActive(true);

    }

}
