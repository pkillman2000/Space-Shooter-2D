using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Score
    [SerializeField]
    TMP_Text _scoreText;
    [SerializeField]
    TMP_Text _gameOverText;

    // Lives
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesImage;

    // Restart Level
    [SerializeField]
    TMP_Text _restartMessage;
    [SerializeField]
    private bool _gameOverFlag = false;

    private void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartMessage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_gameOverFlag && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives) 
    {
        _livesImage.sprite = _livesSprite[currentLives];
    }

    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartMessage.gameObject.SetActive(true);
        _gameOverFlag = true;
        StartCoroutine(FlickerGameOver());
    }

    private IEnumerator FlickerGameOver()
    {
        while (true)
        {            
            yield return new WaitForSeconds(0.1f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _gameOverText.gameObject.SetActive(true);
        }
    }
}
