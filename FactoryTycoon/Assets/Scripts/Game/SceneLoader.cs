using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;

    public delegate void OnLevelEnter(int level);

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MainMenu() 
    {
        animator.SetTrigger("LoadScene");
        StartCoroutine(LoadSceneAfterDelay("Main"));
    }

    public void LoadTable(int level)
    {
        animator.SetTrigger("LoadScene");
        StartCoroutine(LoadSceneAfterDelay("Table" + level));
    }

    public void LoadComp(string sceneName)
    {
        //animator.SetTrigger("LoadScene");
        StartCoroutine(LoadSceneAfterDelay(sceneName));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}
