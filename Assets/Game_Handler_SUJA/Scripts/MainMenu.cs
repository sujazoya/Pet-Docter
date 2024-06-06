using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] string sceneName;
    [SerializeField] string privacyLink = "https://casino-privacypolicy.blogspot.com/2022/01/privacy-policy-thank-you-for-playing.html";
    // Start is called before the first frame update
    void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(Play);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Play()
    {
        Level_Loader.instance.LoadLevel(sceneName);
    }
     public void OpenPrivacy()
    {
        Application.OpenURL(privacyLink);
    }
   
}
