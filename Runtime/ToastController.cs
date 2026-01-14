using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace WeatherToast
{
    public class ToastController : MonoBehaviour
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _ShowAlert(string title, string message);
#endif

        public void ShowToast(string message)
        {
            ShowNativeMessage(message);
        }

        public void ShowToast(string title, string message)
        {
#if UNITY_IOS && !UNITY_EDITOR
            ShowIOSAlert(title, message);
#else
            ShowNativeMessage(message);
#endif
        }

        private void ShowNativeMessage(string message)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            ShowAndroidToast(message);
#elif UNITY_IOS && !UNITY_EDITOR
            ShowIOSAlert("Notification", message);
#else
            Debug.Log($"[Toast] {message}");
#endif
        }

#if UNITY_ANDROID
        private void ShowAndroidToast(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>(
                    "makeText",
                    currentActivity,
                    message,
                    toastClass.GetStatic<int>("LENGTH_LONG")
                );
                toastObject.Call("show");
            }));
        }
#endif

#if UNITY_IOS
        private void ShowIOSAlert(string title, string message)
        {
            _ShowAlert(title, message);
        }
#endif
    }
}
