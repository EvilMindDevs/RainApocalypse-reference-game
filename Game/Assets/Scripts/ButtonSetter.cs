using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetter : MonoBehaviour
{
    private EnesGameManager enesGameManager;

    public Button showAchievements_button;
    public Button buy_hearts_button;
    public Button remove_ads_button;
    public Button restart_button;
    public Button exitGame_button;
    public Button frozen_button;
    public Button exit_button_in_lose_panel;

    // Start is called before the first frame update
    void Start()
    {
        enesGameManager = GameObject.Find("EnesGameManager").GetComponent<EnesGameManager>();

        buy_hearts_button.onClick.AddListener(() =>
        {
            enesGameManager.BuyProduct(HMSIAPConstants.heart);
        });

        showAchievements_button.onClick.AddListener(() =>
        {
            enesGameManager.ShowAchievements();
        });

        remove_ads_button.onClick.AddListener(() =>
        {
            enesGameManager.BuyProduct(HMSIAPConstants.remove_ads);
        });

        restart_button.onClick.AddListener(() =>
        {
            enesGameManager.PlayGame();
        });

        exitGame_button.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        frozen_button.onClick.AddListener(() =>
        {
            enesGameManager.PlayGame();
        });

        exit_button_in_lose_panel.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
