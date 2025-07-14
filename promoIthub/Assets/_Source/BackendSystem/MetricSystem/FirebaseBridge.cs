using UnityEngine;

namespace BackendSystem.MetricSystem
{
    public  class FirebaseBridge : MonoBehaviour
    {
        public static void SendEvent(string eventName, object parameters)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        string json = JsonUtility.ToJson(parameters);
        Application.ExternalCall("sendAnalyticsEvent", eventName, json);
#else
            Debug.Log($"[DevMode] Event: {eventName}, Params: {JsonUtility.ToJson(parameters)}");
#endif
        }
    }
}