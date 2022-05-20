using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
namespace ZipZap
{
    public class Preloader : MonoBehaviour
    {
        private List<string> _countryCodes = new List<string>() { "AT", "BE", "BG", "CR", "CY", "CZ", "DK", "EE", "FI", "FR", "DE", "GR", "GB", "HU", "IE", "IT", "LV", "LT", "LU", "MT", "NL", "PL", "PT", "RO", "SK", "SI", "ES", "SE" };
        public GameObject loader;
        public GameObject consentScreen;
        private string _country = "none";
        private AsyncOperation _loading;
        private IEnumerator Start()
        {
            //PlayerPrefs.DeleteAll();
            InitPrefs();
            StartCoroutine(GetRequest("https://pro.ip-api.com/json?fields=countryCode&key=XgpabbSfRJYL8H6"));
            InitAppSettings();
            //Get();
            yield return null;
            //yield break;
            //Begin to load the Scene you specify
            _loading = SceneManager.LoadSceneAsync(1);
            //Don't let the Scene activate until you allow it to
            _loading.allowSceneActivation = false;
            //Debug.Log("Pro :" + _loading.progress);
            //When the load is still in progress, output the Text and progress bar
            while (!_loading.isDone)
            {
                loader.transform.GetComponent<Image>().fillAmount = _loading.progress;
                //Output the current progress
                //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
                // Check if the load has finished
                if (_loading.progress >= 0.9f && (_country != "none" || _country == "unknow"))
                {
                    loader.transform.GetComponent<Image>().fillAmount = 1;
                    if (HasConsent()) _loading.allowSceneActivation = true;
                    else
                    {
                        if (isGDPR()) consentScreen.SetActive(true);
                        else _loading.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
        private void InitAppSettings()
        {
#if NO_LOG
        Debug.unityLogger.logEnabled = false;
#endif

            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
        }
        public void OnPrivacyPolicy() =>
            Application.OpenURL("https://sites.google.com/view/color-run-stickman-3d-pp");
        public void OnTermsOfService() =>
            Application.OpenURL("https://sites.google.com/view/color-run-stickman-3d-terms");
        private void InitPrefs()
        {
            //PlayerPrefs.DeleteAll();
            if (!PlayerPrefs.HasKey("level")) PlayerPrefs.SetInt("level", 1);
            if (!PlayerPrefs.HasKey("consent")) PlayerPrefs.SetInt("consent", 0);
            if (!PlayerPrefs.HasKey("gunPlayer")) PlayerPrefs.SetInt("gunPlayer", 0);
            if (!PlayerPrefs.HasKey("capPlayer")) PlayerPrefs.SetInt("capPlayer", 0);
            if (!PlayerPrefs.HasKey("coinsPlayer")) PlayerPrefs.SetFloat("coinsPlayer", 0);
        }
        IEnumerator GetRequest(string uri)
        {
            if (HasConsent())
            {
                _country = "ES";
                yield break;
            }
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError: _country = "unknow"; break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        _country = "unknow";
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        _country = "unknow";
                        break;
                    case UnityWebRequest.Result.Success:
                        UserInfo data = JsonUtility.FromJson<UserInfo>(webRequest.downloadHandler.text);
                        _country = data.countryCode;
                        break;
                }
                print(_country);
            }
        }
        private bool HasConsent()
        {
            if (PlayerPrefs.GetInt("consent") == 0) return false;
            else return true;
        }
        public bool isGDPR()
        {
            //Debug.Log("COUNTRY CODE = " + _country);
            return _countryCodes.Contains(_country);
        }
        public void Accept()
        {
            PlayerPrefs.SetInt("consent", 1);
        }
    }
    [System.Serializable]
    public class UserInfo
    {
        public string countryCode;
    }
}
