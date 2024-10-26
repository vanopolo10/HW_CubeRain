using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] public int MinDeathTime = 2;
    [SerializeField] public int MaxDeathTime = 5;
    
    private bool _isTouched;
    private Renderer _cubeRenderer;
    private Rigidbody _rigidbody;
    
    public void ResetVelocity()
    {
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isTouched = false;
        _cubeRenderer = GetComponent<Renderer>();
        _cubeRenderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody component = collision.gameObject.GetComponent<Rigidbody>();
        
        if (_isTouched == false && component != null)
        {
            _isTouched = true;
            _cubeRenderer.material.color = Color.red; 
        }
    }
}
