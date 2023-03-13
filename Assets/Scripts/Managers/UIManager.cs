using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private int _score = 0;

    [Header("Thruster Bar")]
    [SerializeField]
    private Image _thrusterBar;
    [SerializeField]
    private Gradient _thrusterBarGradient;

    [Header("Ammo Bar")]
    [SerializeField]
    private Image _ammoBar;
    [SerializeField]
    private Gradient _ammoBarGradient;

    [Header("Game Over")]
    [SerializeField]
    private TMP_Text _restartMessage;
    [SerializeField]
    private bool _gameOverFlag = false;
    [SerializeField]
    private TMP_Text _youWonText;
    [SerializeField]
    private TMP_Text _youLostText;

    [Header("Wave Start")]
    [SerializeField]
    private TMP_Text _waveNumberText;
    [SerializeField]
    private TMP_Text _waveCountdownText;

    [Header("Wave End")]
    [SerializeField]
    private TMP_Text _waveCompleteText;
    [SerializeField]
    private TMP_Text _waveCompleteStatsText;

    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;
    private WaveManager _waveManager;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogWarning("Spawn Manager is Null!");
        }

        _scrollingBackground = GameObject.Find("Scrolling Background").GetComponent<ScrollingBackground>();
        if (_scrollingBackground == null)
        {
            Debug.LogWarning("Scrolling Background is Null!");
        }

        _waveManager = GameObject.Find("Wave Manager").GetComponent <WaveManager>();
        if(_waveManager == null)
        {
            Debug.LogWarning("Wave Manager is Null");
        }

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

    public void GameOver(bool didWin)
    {
        _waveCompleteText.gameObject.SetActive(false);
        _waveCompleteStatsText.gameObject.SetActive(false);

        _gameOverText.gameObject.SetActive(true);
        _restartMessage.gameObject.SetActive(true);

        if( didWin ) 
        {
            _youWonText.gameObject.SetActive(true);
        }
        else
        {
            _youLostText.gameObject.SetActive(true);
        }

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

    // Wave Start
    public void StartNewWave(int waveNumber)
    {
        _waveCompleteText.gameObject.SetActive(false);
        _waveCompleteStatsText.gameObject.SetActive(false);

        _waveNumberText.text = "Wave " + waveNumber.ToString();
        _waveNumberText.gameObject.SetActive(true);

        StartCoroutine(NewWaveCountdown());
    }

    private IEnumerator NewWaveCountdown()
    {
        int countdown = 3;

        while (countdown > 0) 
        { 
            _waveCountdownText.text = countdown.ToString();
            _waveCountdownText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        _waveNumberText.gameObject.SetActive(false);
        _waveCountdownText.gameObject.SetActive(false);
        _waveManager.StartNewWave();        
    }

    // Wave End
    public void EndCurrentWave(int currentLevel, int total, int destroyed)
    {
        
        _waveCompleteText.gameObject.SetActive(true);
        _waveCompleteText.text = "Wave " + currentLevel.ToString() +" Complete";

        // This displays total enemy spawned and total enemy destroyed.  It may be
        // an added feature later.
        // _waveCompleteStatsText.text = "Destroyed: " + destroyed.ToString() + "/" + total.ToString();
        // _waveCompleteStatsText.gameObject.SetActive(true);
    }
}
