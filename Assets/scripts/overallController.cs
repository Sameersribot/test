using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class overallController : MonoBehaviour
{
    public Image nextImg;
    public Sprite[] images;
    public GameObject canvasPause, openCanvas, gamOvrCanvas;
    public AudioSource btnClick;
    public int randomGen, nextGen;
    public int score, currentHighScore;
    public TextMeshProUGUI scoreText, highScoreText, gmScoreText;
    public bool gmOver = false, paused = false;

    private void Start()
    {
        nextGen = Random.Range(0, 5);
        nextImg.sprite = images[nextGen];
        currentHighScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = "High: " + currentHighScore.ToString();
    }
    private void Update()
    {
        scoreText.text =score.ToString();
    }
    public int getRandomInt()
    {
        randomGen = nextGen;
        nextGen = Random.Range(0,5);
        nextImg.sprite = images[nextGen];
        return randomGen;
    }

    public void onclickPause()
    {
        btnClick.Play();
        paused = true;
        openCanvas.SetActive(false);
        canvasPause.SetActive(true);
    }
    public void restart()
    {
        btnClick.Play();
        SceneManager.LoadScene(0);
    }
    public void pause()
    {
        btnClick.Play();
        paused = false;
        openCanvas.SetActive(true);
        canvasPause.SetActive(false);
    }
    public void gameOver()
    {
        if(currentHighScore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
        openCanvas.SetActive(false);
        gamOvrCanvas.SetActive(true);
        gmScoreText.text = "Score: " + score.ToString();
        gmOver = true;
    }
    public void soundBtn()
    {

    }
}
