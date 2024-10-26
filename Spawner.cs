using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private float _spawnHeight = 10;
    [SerializeField] private float _spawnRate = 0.5f;
    [SerializeField] private bool _autoExpand = false; 

    private Transform _platform;
    private ObjectPool<Cube> _objectPool;

    private void Awake()
    {
        _platform = gameObject.GetComponent<Transform>();

        _objectPool = new ObjectPool<Cube>(
            OnCreatePooledItem,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPoolObject,
            _autoExpand,
            _poolSize
        );
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes(_spawnRate));
    }

    private IEnumerator SpawnCubes(float time)
    {
        int scaleMod = 2;
        float margin = 1;
        
        WaitForSeconds wait = new WaitForSeconds(time);

        while (true)
        {
            if (_objectPool.CountActive < _poolSize || _autoExpand)
            {
                Cube cube = _objectPool.Get();
                
                Vector3 scale = _platform.localScale;
                
                Vector3 spawnPosition = _platform.position + new Vector3(
                    Random.Range(-scale.x / scaleMod + margin, scale.x / scaleMod - margin),
                    _spawnHeight,
                    Random.Range(-scale.z / scaleMod + margin, scale.z / scaleMod - margin)
                );

                cube.ResetVelocity();
                cube.transform.position = spawnPosition;
            }

            yield return wait;
        }
    }

    private Cube OnCreatePooledItem()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.Died += OnCubeDied;

        return cube;
    }

    private void OnCubeDied(Cube cube)
    {
        _objectPool.Release(cube);
    }
    
    private void OnGetFromPool(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Cube cube)
    { 
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Cube cube)
    {
        cube.Died -= OnCubeDied;
        Destroy(cube.gameObject);
    }
}
