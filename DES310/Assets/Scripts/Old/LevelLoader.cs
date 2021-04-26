using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float delayTime = 1f;
    private void Update()
    {

    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //Anim
        transition.SetTrigger("Start");
        //Delay
        yield return new WaitForSeconds(delayTime);
        //Load
        SceneManager.LoadScene(levelIndex);
    }
}
