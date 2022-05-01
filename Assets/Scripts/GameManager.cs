using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    FadeManager fadeManager;

    private void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();
    }


    public void startGame()
    {
        StartCoroutine("IStartGame");
    }
    IEnumerator IStartGame()
    {
        while (fadeManager.onFade == true)
        {
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
       
        SceneManager.LoadScene(1);
    }

    void openOption()
    {

    }

    void exitGame()
    {

    }
}
