using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private float _spawnHeight = 10;
    [SerializeField] private float _spawnRate = 0.5f;
    
    private Transform _platform;
    private List<GameObject> _pool;

    void Start()
    {
        _platform = gameObject.GetComponent<Transform>();
        _pool = new List<GameObject>();
        
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject cube = Instantiate(_cubePrefab);
            cube.SetActive(false); 
            _pool.Add(cube);
        }

        InvokeRepeating(nameof(SpawnCube), 0, _spawnRate);
    }

    void SpawnCube()
    {
        GameObject cube = GetPooledCube();

        int scaleMod = 2;
        float margin = 1;
        
        if (cube != null)
        {
            Vector3 spawnPosition = _platform.position + new Vector3(
                Random.Range(-_platform.localScale.x / scaleMod + margin, _platform.localScale.x / scaleMod - margin),
                _spawnHeight,
                Random.Range(-_platform.localScale.z / scaleMod + margin, _platform.localScale.z / scaleMod - margin)
            );
            
            cube.transform.position = spawnPosition;
            cube.SetActive(true);
        }
    }

    GameObject GetPooledCube()
    {
        foreach (GameObject cube in _pool)
        {
            if (!cube.activeInHierarchy)
            {
                return cube;
            }
        }

        return null;
    }
}
