using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //create handle to text
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText, _torpedoText;

    [SerializeField]
    private Text _enemiesText;

    [SerializeField]
    private Text _enemiesSpawnedTotal;

    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private Text _waveText;
    private SpawnManager _spawnManager;

    [SerializeField]
    private Text _EITW;
    [SerializeField]
    private Text _ESITW;
    [SerializeField]
    private Text _EKTW;

    private bool stopFlashing = false;
    private int _wave;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
    }

    public void UpdateEnemiesActive(int enemiesActive)
    {
        _enemiesText.text = "Enemies Active: " + enemiesActive;
    }

    public void UpdateEnemiesSpawnedTotal(int enemiesSpawnedTotal)
    {
        _enemiesSpawnedTotal.text = "Enemies Spawned Total: " + enemiesSpawnedTotal;
    }


    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {

        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlashingRoutine());
    }

    IEnumerator GameOverFlashingRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void NextWave(int _wave)
    {
        _waveText.text = "WAVE " + _wave;
        StartCoroutine(NextWaveDisplay());

    }

    IEnumerator NextWaveDisplay()
    {
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _waveText.gameObject.SetActive(false);
    }

    public void UpdateEITW(int eitw)
    {
        _EITW.text = "Enemies In This Wave: " + eitw;
    }

    public void UpdateESITW(int esitw)
    {
        _ESITW.text = "Enemies Spawned In This Wave: " + esitw;
    }

    public void UpdateEKTW(int ektw)
    {
        _EKTW.text = "Enemies Killed This Wave: " + ektw;
    }
}
