using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadingFillBar;

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        operation.allowSceneActivation = false;

        _loadingScreen.gameObject.SetActive(true);

        while (operation.progress < 0.9f)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingFillBar.fillAmount = progressValue;
            Debug.Log("Progress Value: " + progressValue);
            Debug.Log("FillBar: " + _loadingFillBar.fillAmount);
            yield return null;
        }

        float fakeLoadingTime = 2f;
        float currentProgress = 0.9f;

        while (currentProgress < 1f)
        {
            currentProgress += Time.deltaTime / fakeLoadingTime;
            _loadingFillBar.fillAmount = Mathf.Clamp01(currentProgress);
            yield return null;
        }

        _loadingFillBar.fillAmount = 1f;

        operation.allowSceneActivation = true;
    }
}