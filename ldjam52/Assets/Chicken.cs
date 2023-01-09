using UnityEngine;
using Random = UnityEngine.Random;

public class Chicken : MonoBehaviour
{
    private Animator _animator;
    public float range = 5f;
    private Vector3 result;
    public float speed = 1f;
    private Vector3 randomPoint;
    private float timeTillNewPoint = 5f;
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsMoving", true);
        GetRandomPoint();
    }

    private void FixedUpdate()
    {
        if(timeTillNewPoint < 0)
            GetRandomPoint();
        
        var step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, randomPoint, step);

        timeTillNewPoint -= Time.deltaTime;
        
        if(Vector3.Distance(transform.position, randomPoint) < 0.01f)
            GetRandomPoint();
    }

    private void GetRandomPoint()
    {
        randomPoint = transform.position + Random.insideUnitSphere * range;
        timeTillNewPoint = 5f;
    }
    
    
}
