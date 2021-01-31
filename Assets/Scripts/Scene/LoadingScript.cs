using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Main", LoadSceneMode.Single);
        //SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        StartCoroutine(LoadAllScenes());
        
    }
    

    private AsyncOperation uiAo;
    private AsyncOperation mainAo;

    IEnumerator LoadAllScenes()
    {
        //Debug.Log("Loader: Start loading ui");
        uiAo = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        uiAo.allowSceneActivation = false;

        yield return new WaitWhile(() => uiAo.progress < 0.9f);

        // Scene loaded, time to activate
        uiAo.allowSceneActivation = true;

        //Debug.Log("Loader: Start loading main");
        mainAo = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        yield return new WaitWhile(() => mainAo.progress < 0.9f);
        
        // Scene loaded, time to activate
        yield return new WaitWhile(() => !mainAo.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("_Load"));

        yield return null;
    }

    private float GetProgress()
    {
        float progress = 0;

        if (uiAo != null)
            progress += Mathf.Clamp01(uiAo.progress / 0.9f);

        if (mainAo != null)
            progress += Mathf.Clamp01(mainAo.progress / 0.9f);

        return Mathf.Clamp01(progress / 2f);
    }

    IEnumerator AsynchronousLoad(AsyncOperation ao)
    {
        yield return null;

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (ao.progress >= 0.9f)
            {
                //ao.allowSceneActivation = true;
                Debug.Log("Completed");
                break;
            }

            yield return null;
        }
    }




}
