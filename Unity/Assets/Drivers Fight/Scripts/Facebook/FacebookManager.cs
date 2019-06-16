using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{

    public Button btnLogin, btnLogout, btnName, btnShare;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    void Start()
    {
        btnLogin.onClick.AddListener(() => FacebookLogin());
        btnLogout.onClick.AddListener(() => FacebookLogout());
        btnName.onClick.AddListener(() => GetName());
        btnShare.onClick.AddListener(() => FacebookShare());
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            Debug.Log(aToken.UserId);
            foreach (string perm in aToken.Permissions)
                Debug.Log(perm);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    private void FacebookLogout()
    {
        FB.LogOut();
    }

    private void FacebookShare()
    {
        FB.FeedShare(
                linkCaption: "Regardez ce super jeux, Drivers Fight !",
                link: new System.Uri("https://github.com/ArthurJacquin/Drivers-Fight"),
                linkName: "BestGame",
                callback:OnShare);
    }

    private void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink error : " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else
        {
            Debug.Log("Share succeed");
        }
          
    }

    public void GetName()
    {
        FB.API("me?fields=name", Facebook.Unity.HttpMethod.GET, delegate (IGraphResult result)
        {
            if (result.ResultDictionary != null)
            {
                foreach (string key in result.ResultDictionary.Keys)
                {
                    Debug.Log(key + " : " + result.ResultDictionary[key].ToString());
                    if (key == "name")
                        btnName.GetComponentInChildren<Text>().text = result.ResultDictionary[key].ToString();
                }
            }
        });
    }
}
