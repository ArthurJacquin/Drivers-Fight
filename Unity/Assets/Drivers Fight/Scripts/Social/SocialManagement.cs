using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialManagement : MonoBehaviour
{
    public Button btnFacebook, btnTwitter;

    void Start()
    {
        btnFacebook.onClick.AddListener(() => FacebookShare());
        btnTwitter.onClick.AddListener(() => TwitterShare());
    }

    private void FacebookShare()
    {
        string FACEBOOK_ADDRESS = "https://www.facebook.com/sharer.php?u=https%3A%2F%2Fwww.facebook.com%2FDrivers-Fight-2198519913530683%2F";
        Application.OpenURL(FACEBOOK_ADDRESS);
    }

    private void TwitterShare()
    {
        string twitterAdresse = "http://twitter.com/intent/tweet";
        string facebookPage = "https://www.facebook.com/Drivers-Fight-2198519913530683/";
        string texte = "Soit toi aussi un.e driver !";
        Application.OpenURL(twitterAdresse +
       "?text=" + WWW.EscapeURL(texte + "\n" + "Visite la page du jeu :\n" + facebookPage));
    }
}
