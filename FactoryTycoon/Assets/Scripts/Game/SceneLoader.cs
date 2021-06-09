﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;

    public delegate void OnLevelEnter(int level);
    public static event OnLevelEnter OnLevelEnterEvent;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MainMenu() 
        {
        animator.SetTrigger("LoadScene");
        StartCoroutine(LoadScene("Main"));
    }

    public void LoadTable(int index)
    {
        OnLevelEnterEvent?.Invoke(index);

        animator.SetTrigger("LoadScene");
        StartCoroutine(LoadScene("Table" + index));
    }

    public void LoadComp(string sceneName)
    {
        //animator.SetTrigger("LoadScene");
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}