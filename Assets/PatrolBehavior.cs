using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private float _delayBeforeNewPoint;

    [SerializeField] private List<Transform> _points;

    private int _currentPoint;
    private float _currentTime;
    private float _delayTime;
    private Transform _startPosition;
    private Transform _nextPosition;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Moving", true);
        _animator.SetBool("Idle",false);

       
    }

    // Update is called once per frame
    void Update()
    {
         _startPosition = _points[_currentPoint];
         _nextPosition = _points[(_currentPoint + 1) % _points.Count]; 

        _currentTime += Time.deltaTime;
        float travelTime = GetTravelTime(_startPosition, _nextPosition);
        float step = _speed * Time.deltaTime;
        
        var direction = _nextPosition.position - _startPosition.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.position = Vector3.MoveTowards(transform.position, _nextPosition.position, step);

       

        if (_currentTime >= travelTime)
        {
            _animator.SetBool("Moving",false);
            _animator.SetBool("Idle",true);
            
            _delayTime += Time.deltaTime;

            if (_delayTime >= _delayBeforeNewPoint)
            { 
                  
                _currentPoint = (_currentPoint + 1) % _points.Count;
                _currentTime = 0f;
                _delayTime = 0f;
                
                
                _animator.SetBool("Idle",false);
                _animator.SetBool("Moving",true);
            }
           
        }
    }

    private float GetTravelTime(Transform current, Transform target)
    {
        var distance = Vector3.Distance(current.position, target.position);
        return distance / _speed;
    }
}