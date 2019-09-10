using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour  //  лоадСкрин при загрузке игры с прогрессБаром 100% загрузка
{
    public GameObject LoadScreen;
    public Slider Slider;
    public TextMeshProUGUI ProgressText;

    public void PlayGame()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!operation.isDone)
        {
            LoadScreen.SetActive(true);
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Slider.value = progress;
            ProgressText.text = progress * 100f+"%";
            yield return null;
        }
    }

    public void QuiteGame()
    {
        Application.Quit();
    }
}
