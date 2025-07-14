using System.Runtime.InteropServices;
using BackendSystem.MetricSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


namespace BackendSystem
{
    public class WebLink : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void OpenExternalLink(string url);

        [Inject] private FirebaseBridge _bridge;

        public string siteUrl = "https://ithub.ru/";
        public string tgUrl = "https://t.me/ithubgame";

        public void OpenSite()
        { 
                FirebaseBridge.SendEvent("site_visit", new { value = 1 });   
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenExternalLink(siteUrl);
#else
            Application.OpenURL(siteUrl);
#endif
        }
        public void OpenTg()
        {
                FirebaseBridge.SendEvent("telegram_visit", new { value = 1 });  
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenExternalLink(tgUrl);
#else
            
            Application.OpenURL(tgUrl);
#endif
        }
    }
}