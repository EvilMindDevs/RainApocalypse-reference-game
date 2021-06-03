using HmsPlugin;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    Animator anim;

    float input;

    public int health;
    public Text healthDisplay;

    public GameObject losePanel;
    public AudioSource source;

    public int score;

    public Text scoreCounter;

    Pause pauseScript;

    public float startDashTime;
    private float dashTime;
    public float extraSpeed;
    private bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        updateHealthDisplay();
        scoreCounter.text = "Score: " + score.ToString();
        pauseScript = GameObject.Find("PauseImage").GetComponent<Pause>();
        HMSAdsKitManager.Instance.HideBannerAd();
        HMSAchievementsManager.Instance.OnGetAchievementsListSuccess = OnGetAchievemenListSuccess;
        HMSAchievementsManager.Instance.OnGetAchievementsListFailure = OnGetAchievementsListFailure;
    }

    void Update()
    {
        if (input != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (input > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (input < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed += extraSpeed;
            isDashing = true;
            dashTime = startDashTime;
        }

        if (dashTime <= 0 && isDashing == true)
        {
            isDashing = false;
            speed -= extraSpeed;
        }
        else
        {
            dashTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            print("ESCAPE ENTERED");
            pauseScript.showPausePanel();
        }
    }

    void FixedUpdate() //for physics usage
    {
        if (Input.GetMouseButton(0))
        {
            input = ((Input.mousePosition.x > (Screen.width / 2)) /*&& (pauseButton.transform.position.x != Input.mousePosition.x)*/) ? 1 : -1; // -1 left, +1 right, 0 nothing
            rb.velocity = new Vector2(input * speed, rb.velocity.y);
        }
        else
        {
            input = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void OnGetAchievemenListSuccess(IList<Achievement> achievementList)
    {
        Debug.Log("HMS Games: GetAchievementsList SUCCESS ");

        Debug.Log("[PLAYER-ACHIEVEMENT] score has come as: " + score);

        Achievement beginnerAch = achievementList[0];
        Achievement experiencedAch = achievementList[1];
        Achievement masterAch = achievementList[2];

        //Achievement beginnerScorer = achievementList[3]; //15
        Achievement beginnerScorer = achievementList.First(ach => ach.Id == HMSAchievementConstants.BeginnerScorer); //15
        Achievement mediumScorer = achievementList[4]; //25
        Achievement masterScorer = achievementList[5]; //50

        Debug.Log("beginnerScorer.State before = " + beginnerScorer.State);
        Debug.Log("mediumScorer.State before = " + mediumScorer.State);
        Debug.Log("masterScorer.State before = " + masterScorer.State);

        if (score >= 15 && beginnerScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(beginnerScorer.Id);
            HMSAchievementsManager.Instance.RevealAchievement(mediumScorer.Id);
            Debug.Log("beginnerScorer.State first if = " + beginnerScorer.State);
            Debug.Log("mediumScorer.State first if = " + mediumScorer.State);
            Debug.Log("masterScorer.State first if = " + masterScorer.State);
        }
        else if (score >= 25 && beginnerScorer.State == 3 && mediumScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(mediumScorer.Id);
            HMSAchievementsManager.Instance.RevealAchievement(masterScorer.Id);
            Debug.Log("beginnerScorer.State second if = " + beginnerScorer.State);
            Debug.Log("mediumScorer.State second if = " + mediumScorer.State);
            Debug.Log("masterScorer.State second if = " + masterScorer.State);
        }
        else if (score >= 50 && mediumScorer.State == 3 && masterScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(masterScorer.Id);
            Debug.Log("beginnerScorer.State third if = " + beginnerScorer.State);
            Debug.Log("mediumScorer.State third if = " + mediumScorer.State);
            Debug.Log("masterScorer.State third if = " + masterScorer.State);
        }
    }

    private void OnGetAchievementsListFailure(HMSException obj)
    {
        Debug.Log("OnGetAchievementsListFailure with code: " + obj.ErrorCode);
    }

    public void TakeDamage(int damageAmount)
    {
        source.Play();
        health -= damageAmount;
        if (health <= 0)
        {
            healthDisplay.text = "0";
            HMSAnalyticsManager.Instance.SendEventWithBundle("GameCompleted", "Score", score);
            Destroy(gameObject);

            if (!GameObject.Find("EnesGameManager").GetComponent<EnesGameManager>().isAdsRemoved)
                HMSAdsKitManager.Instance.ShowInterstitialAd();

            losePanel.SetActive(true);

            HMSAchievementsManager.Instance.GetAchievementsList();
        }
        else
        {
            updateHealthDisplay();
        }
    }

    public void updateHealthDisplay()
    {
        healthDisplay.text = health.ToString();
    }

    public void ScoreCalculator()
    {
        score++;
        scoreCounter.text = "Score: " + score.ToString();
    }
}
