using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HmsPlugin;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.IAP;
using System;

public class EnesGameManager : MonoBehaviour
{
    [HideInInspector]
    public bool isAdsRemoved = false;

    void Start()
    {
        //HMSAccountManager.Instance.SignIn();
        //HMSAccountManager.Instance.OnSignInSuccess = OnSignInSuccess;
        HMSIAPManager.Instance.OnBuyProductSuccess = OnBuyProductSuccess;
        HMSIAPManager.Instance.OnBuyProductFailure = OnBuyProductFailure;
        
        DontDestroyOnLoad(gameObject);
    }

    public void ShowAchievements()
    {
        HMSAchievementsManager.Instance.ShowAchievements();
    }

    private void OnBuyProductFailure(int obj)
    {
        switch (obj)
        {
            case OrderStatusCode.ORDER_STATE_CANCEL:
                // User cancel payment.
                Debug.Log("[HMS]: User cancel payment");
                break;
            case OrderStatusCode.ORDER_STATE_FAILED:
                Debug.Log("[HMS]: order payment failed");
                break;

            case OrderStatusCode.ORDER_PRODUCT_OWNED:
                Debug.Log("[HMS]: Product owned");
                break;
            default:
                Debug.Log("[HMS:] BuyProduct ERROR" + obj);
                break;
        }
    }

    private void OnBuyProductSuccess(PurchaseResultInfo obj)
    {
        string myProductId = obj.InAppPurchaseData.ProductId;

        if (myProductId.Equals("heart"))
        {
            GameObject.Find("Player").GetComponent<Player>().health++; //soldaki object sagdaki script
            GameObject.Find("Player").GetComponent<Player>().updateHealthDisplay();
        }
        else if (myProductId.Equals("remove_ads"))
        {
            isAdsRemoved = true;
        }
    }

    public void PlayGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        if(HMSAccountManager.Instance.IsSignedIn)
            SceneManager.LoadScene("Game");
    }

    public void BuyProduct(string id)
    {
        HMSIAPManager.Instance.BuyProduct(id);
    }
}
