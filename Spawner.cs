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
    [SerializeField] private float _voidLevel = -20f;

    private Transform _platform;
    private ObjectPool<Cube> _objectPool;

    private void Awake()
    {
        _platform = gameObject.GetComponent<Transform>();

        _objectPool = new ObjectPool<Cube>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            _autoExpand,
            _poolSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCube(_spawnRate));
        StartCoroutine(CheckObjectsBelowMinHeight());
    }
    
    IEnumerator CheckObjectsBelowMinHeight(float updateRate = 0.1f)
    {
        while (true)
        {
            var cubes = FindObjectsByType<Cube>(FindObjectsSortMode.None);
            foreach (Cube cube in cubes)
            {
                if (cube.gameObject.activeInHierarchy && cube.transform.position.y < _voidLevel)
                {
                    _objectPool.Release(cube);
                }
            }

            yield return new WaitForSeconds(updateRate);
        }
    }

    private IEnumerator SpawnCube(float time)
    {
        int scaleMod = 2;
        float margin = 1;

        while (true)
        {
            Cube cube = _objectPool.Get();

            Vector3 spawnPosition = _platform.position + new Vector3(
                Random.Range(-_platform.localScale.x / scaleMod + margin, _platform.localScale.x / scaleMod - margin),
                _spawnHeight,
                Random.Range(-_platform.localScale.z / scaleMod + margin, _platform.localScale.z / scaleMod - margin)
            );

            cube.ResetVelocity();
            cube.transform.position = spawnPosition;

            StartCoroutine(ReturnCubeToPoolAfterTime(cube, Random.Range(cube.MinDeathTime, cube.MaxDeathTime + 1)));

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator ReturnCubeToPoolAfterTime(Cube cube, float time)
    {
        yield return new WaitForSeconds(time);
        _objectPool.Release(cube); 
    }


    private Cube CreatePooledItem()
    {
        return Instantiate(_cubePrefab);
    }

    private void OnTakeFromPool(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Cube cube)
    {
        Destroy(cube.gameObject);
    }
}
