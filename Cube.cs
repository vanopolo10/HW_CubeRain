using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _minDeathTime = 2;
    [SerializeField] private int _maxDeathTime = 5;
    [SerializeField] private float _voidLevel = -7f;
    
    private bool _isTouched;
    private Renderer _cubeRenderer;

    private void OnEnable()
    {
        _isTouched = false;
        _cubeRenderer = GetComponent<Renderer>();
        _cubeRenderer.material.color = Color.white;
    }

    private void Update()
    {
        if (gameObject.transform.position.y < _voidLevel)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = "Platform";
        
        if (_isTouched == false && collision.gameObject.CompareTag(tag))
        {
            _isTouched = true;
            _cubeRenderer.material.color = Color.red; 

            float randomLifetime = Random.Range(_minDeathTime, _maxDeathTime + 1);
            StartCoroutine(Die(randomLifetime));
        }
    }

    private IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
