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
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private int _score = 0;

    /*
    // Lives
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesImage;
    */

    // Thruster Bar
    [SerializeField]
    private Image _thrusterBar;
    [SerializeField]
    private Gradient _thrusterBarGradient;

    // Ammo Bar
    [SerializeField]
    private Image _ammoBar;
    [SerializeField]
    private Gradient _ammoBarGradient;

    // Restart Level
    [SerializeField]
    TMP_Text _restartMessage;
    [SerializeField]
    private bool _gameOverFlag = false;

    private void Start()
    {
        _scoreText.text = "0";
        _gameOverText.gameObject.SetActive(false);
        _restartMessage.gameObject.SetActive(false);

        // Set Thruster Bar to Full
        UpdateThrusterBar(1, 1);
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
        _score += score;
        _scoreText.text = _score.ToString();
    }

    /*
    public void UpdateLives(int currentLives) 
    {
        _livesImage.sprite = _livesSprite[currentLives];
    }
    */

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

    public void UpdateThrusterBar(float maxFuel, float currentFuel)
    {
        float fuelPercentage = currentFuel / maxFuel;
        // Change Bar Size
        _thrusterBar.fillAmount = fuelPercentage;
        // Change Bar Color
        _thrusterBar.color = _thrusterBarGradient.Evaluate(fuelPercentage);
    }

    public void UpdateAmmoBar(float maxAmmo, float currentAmmo)
    {
        float ammoPercentage = currentAmmo / maxAmmo;
        _ammoBar.fillAmount = ammoPercentage;
        _ammoBar.color = _ammoBarGradient.Evaluate(ammoPercentage);
    }

}
