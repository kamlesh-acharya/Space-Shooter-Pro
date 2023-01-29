using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] powerups;

    //[SerializeField]
    //private GameObject _tripleShotPowerupPrefab;
    //[SerializeField]
    //private GameObject _speedPowerupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            //switch (Random.Range(0, 3))
            //{
            //    case 0: 
            //        Instantiate(_tripleShotPowerupPrefab, new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            //        break;
            //    case 1:
            //        Instantiate(_speedPowerupPrefab, new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            //        break;
            //    case 2:
            //        Instantiate(_tripleShotPowerupPrefab, new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            //        break;
            //}
            Instantiate(powerups[Random.Range(0,3)], new Vector3(Random.Range(-8f, 8f), 7f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
