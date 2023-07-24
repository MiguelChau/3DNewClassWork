/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    [Header("WayPoints")]
    public GameObject[] wayPoints;
    public float minDistance = 1f;
    public float speed = 1f;

    private int _index = 0;

    private void Update()
    {
        if (Vector3.Distance(transform.position, wayPoints[_index].transform.position) < minDistance)
        {
            _index++;
            if (_index >= wayPoints.Length)
            {
                _index = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[_index].transform.position, Time.deltaTime * speed);
        transform.LookAt(wayPoints[_index].transform.position);
    }   
}
*/