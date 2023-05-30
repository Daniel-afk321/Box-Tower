using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void restart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        audioSource.Play();
    }

    public void Menu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        audioSource.Play();
    }
}
