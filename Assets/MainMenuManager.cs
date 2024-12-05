using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private string sceneToLoad;

    public void StartGame()
    {
        StartCoroutine(LoadSceneASync());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneASync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
