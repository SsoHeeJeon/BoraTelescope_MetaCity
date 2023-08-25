using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameManager gamemanager;
    public Slider progressBar;
    public static string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();

        if (nextScene == null)
        {
            //nextScene = "Loading";
            if (ContentsInfo.ContentsName == "Cartoon")
            {
                nextScene = "CartoonMode";
            }
            else if (ContentsInfo.ContentsName == "Jaemilang")
            {
                nextScene = "JaemilangMode";
            }
            progressBar.value = 0;
        }

        StartCoroutine(LoadScene());
    }

    public void MoveWaitingMode()
    {
        nextScene = "WaitingMode";
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(nextScene);

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        if (op != null)
        {
            op.allowSceneActivation = false;

            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;

                if (op.progress >= 0.9f)
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                    if (progressBar.value == 1.0f)
                        op.allowSceneActivation = true;

                }
                else
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                    if (progressBar.value >= op.progress)
                    {
                        timer = 0f;
                    }
                }
            }
        }
        else if (op == null)
        {
            gamemanager.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_ChangeMode, "Fail_ChangeMode:" + nextScene, GetType().ToString());
        }
    }
}
