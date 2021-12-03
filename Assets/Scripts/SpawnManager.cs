using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _enemyDodger;

    [SerializeField]
    private GameObject _enemyLefty;

    [SerializeField]
    private GameObject _enemyRighty;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerupPrefab;

    [SerializeField]
    private UIManager _uiManager;

    private Component _gameManagerComponentScript;
   [SerializeField]
   Player player;

    private bool _stopSpawning = false;

    public string enemyType = "StraightDown";

    private int waveCounter = 1;
    public int enemiesSpawnedTotal = 0;
    public int enemiesActiveCounter;
    private int waveEnemyAdd = 1;
    public int enemiesThisWave = 1;
    public int enemiesKilledThisWave;
    public int enemiesSpawnedThisWave = 0;

    void Start()
    {
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NULL.");
        }
    }

    public void StartSpawning()
    { 
        enemiesKilledThisWave = 0;
        enemiesThisWave = waveCounter;
        _uiManager.UpdateEITW(enemiesThisWave);
        _stopSpawning = false;
        _uiManager.UpdateEnemiesSpawnedTotal(enemiesSpawnedTotal);
        enemiesSpawnedThisWave = 0;
        _uiManager.UpdateESITW(enemiesSpawnedThisWave);
        StartCoroutine(SpawnEnemyRoutine());
    }

    public void StartSpawingPowerups()
    {
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        _uiManager.NextWave(waveCounter);
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            int randomDirection = UnityEngine.Random.Range(0, 100);

            if (randomDirection > 79)
            {
                if (randomDirection > 89)
                {
                    enemyType = "Lefty";
                    Vector3 posToSpawn = new Vector3(10f, Random.Range(0, 6), 0);
                    GameObject newEnemy = Instantiate(_enemyLefty, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    enemyType = "Righty";
                    Vector3 posToSpawn = new Vector3(-10f, Random.Range(0, 6), 0);
                    GameObject newEnemy = Instantiate(_enemyRighty, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }
            else
            {
                int randomEnemy = UnityEngine.Random.Range(0, 100);

                if (randomEnemy > 90)
                {
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(_enemyDodger, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }
            
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            if (enemiesSpawnedThisWave > (enemiesThisWave - 1))
            {
                _stopSpawning = true;
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (waveCounter > 0)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            int randomPowerUpBalance = UnityEngine.Random.Range(0, 100);

            if (randomPowerUpBalance > 79)
            {
                if (randomPowerUpBalance > 89)
                {
                    GameObject newPowerUpFireBall = Instantiate(_powerupPrefab[7],
                                        posToSpawn,
                                        Quaternion.identity);
                }
                else
                {
                    GameObject newPowerUpBlackHole = Instantiate(_powerupPrefab[6],
                                        posToSpawn,
                                        Quaternion.identity);
                }
            }

            if (randomPowerUpBalance < 80)
            {
                if (player.GetComponent<Player>()._ammo == 0)
                {
                    GameObject newPowerUp = Instantiate(_powerupPrefab[3], posToSpawn, Quaternion.identity);
                }
                else
                {
                    int randomPowerUp = Random.Range(0, 6);

                    GameObject newPowerUp = Instantiate(_powerupPrefab[randomPowerUp],
                                                        posToSpawn,
                                                        Quaternion.identity);
                }
            }



            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }



    public IEnumerator StartNewWave()
    {
        waveCounter++;
        enemiesThisWave = waveCounter;
        _uiManager.UpdateEITW(enemiesThisWave);
        _uiManager.NextWave(waveCounter); 
        enemiesSpawnedThisWave = 0;
        enemiesKilledThisWave = 0;
        _uiManager.UpdateESITW(enemiesSpawnedThisWave);
        _uiManager.UpdateEKTW(enemiesKilledThisWave);
        _stopSpawning = false;
        StartSpawning();
        yield return new WaitForSeconds(1.0f);
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void Update()
    {

    }

}
