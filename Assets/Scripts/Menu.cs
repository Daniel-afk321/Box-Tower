using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Text scoreText;
    private AudioSource audioSource;
    private void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
        audioSource = GetComponent<AudioSource>();
    }

   public void ToGame()
    {
        SceneManager.LoadScene("Game");
        audioSource.Play();
    }
    public void ToExit()
    {
        Application.Quit();
        audioSource.Play();
    }
}
