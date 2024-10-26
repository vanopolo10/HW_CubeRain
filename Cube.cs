using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _minDeathTime = 2;
    [SerializeField] private int _maxDeathTime = 5;
    [SerializeField] private float _voidLevel = -20f;
    
    private bool _isTouched;
    private Renderer _cubeRenderer;
    private Rigidbody _rigidbody;
    
    public event Action<Cube> Died;

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _isTouched = false;
        _cubeRenderer.material.color = Color.white;
    }

    private void Update()
    {
        if (gameObject.transform.position.y < _voidLevel)
        {
            Died?.Invoke(this);
        }
    }

    public void ResetVelocity()
    {
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Platform component = collision.gameObject.GetComponent<Platform>();
        
        if (_isTouched == false && component != null)
        {
            _isTouched = true;
            _cubeRenderer.material.color = Color.red; 
        }

        StartCoroutine(WaitAndDie(Random.Range(_minDeathTime, _maxDeathTime + 1)));
    }

    private IEnumerator WaitAndDie(int time)
    {
        yield return new WaitForSeconds(time);
        
        Died?.Invoke(this);
    }
}
