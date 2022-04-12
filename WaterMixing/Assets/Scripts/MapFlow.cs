using System;
using UnityEngine;

public class MapFlow : MonoBehaviour
{
    private Vector2 _mouseClickPos;
    private Vector2 _mouseCurrentPos;

    public float lowerBound = 0.1f;
    public float upperBound = 34f;

    private void Awake()
    {
        var persistentData = GameObject.Find($"PersistentData").GetComponent<PersistentData>().GetIconValues();
        var levelList = GameObject.Find($"Map_Background").transform.Find($"Levels").gameObject;
        print(levelList.transform.childCount);
        for (var i = 0; i < levelList.transform.childCount - 1; i++)
        {
            levelList.transform.GetChild(i).GetComponent<LevelIcons>().SetIconValue(persistentData[i]);
            levelList.transform.GetChild(i).GetComponent<LevelIcons>().SetSprite();
        }
    }

    private void Update()
    {
        CameraDrag();
    }

    private void CameraDrag()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_mouseClickPos == default)
            {
                if (Camera.main != null) _mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Camera.main != null) _mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var distance = _mouseCurrentPos - _mouseClickPos;

            if (transform.position.x - distance.x <= -lowerBound || transform.position.x - distance.x >= upperBound)
            {
                distance.x = 0;
            }

            transform.position += new Vector3(-distance.x, 0, 0);
        }
     
        // If LMB is released, stop moving the camera
        if (Input.GetKeyUp(KeyCode.Mouse0))
            _mouseClickPos = default;
    }
    
}
