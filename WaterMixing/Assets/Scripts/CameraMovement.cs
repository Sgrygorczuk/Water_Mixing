using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private List<Transform> _path;
    private int _currentPoint = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Creates an empty list
        _path = new List<Transform>();
        //Finds the Path Game Object 
        var pathCopy = GameObject.Find("Path").GetComponent<Transform>();
        //Gets the var for how many children the path has 
        var children = pathCopy.transform.childCount;
        //Adds all the points from the Path Game Object to the local stored list 
        for (var i = 0; i < children; ++i)
        {
            _path.Add(pathCopy.transform.GetChild(i).transform);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        print(transform.position + " " + _path[_currentPoint].position);
        //If the transform position is not the same as current_points position move towards it 
        if (transform.position != _path[_currentPoint].position)
        {
            transform.position =  Vector3.MoveTowards(transform.position, _path[_currentPoint].position, 0.1f);
        }
        //Else if we reached a position move to the next point in the list 
        else
        {
            if (_currentPoint < _path.Count - 1)
            {
                _currentPoint++;
            }
        }
    }
}
