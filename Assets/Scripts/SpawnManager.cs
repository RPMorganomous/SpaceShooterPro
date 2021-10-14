using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyLefty;

    [SerializeField]
    private GameObject _enemyRighty;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerupPrefab;

    private bool _stopSpawning = false;

    public string enemyType = "StraightDown";

    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            int randomDirection = UnityEngine.Random.Range(0, 100);

            if (randomDirection > 79)
            {
                if (randomDirection > 89)
                {
                    enemyType = "Lefty";
                    Vector3 posToSpawn = new Vector3(-8f, Random.Range(0, 6), 0);
                    GameObject newEnemy = Instantiate(_enemyLefty, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    enemyType = "Righty";
                    Vector3 posToSpawn = new Vector3(8f, Random.Range(0, 6), 0);
                    GameObject newEnemy = Instantiate(_enemyRighty, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }
            else
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            int randomPowerUp = Random.Range(0, 6);

            GameObject newPowerUp = Instantiate(_powerupPrefab[randomPowerUp],
                                                posToSpawn,
                                                Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void Update()
    {

    }

}
