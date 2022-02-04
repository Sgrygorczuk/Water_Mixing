using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour
{
    //========= Color Control 
    //Preset colors at the beginning of the level 
    public Color[] colors = new[] { Color.clear, Color.clear, Color.clear, Color.clear};
    //Holds all the box sprites of the colors 
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    //Tells us the top spot that currently has color 
    public int lastActive = 0;


    // Start is called before the first frame update
    public void Start()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            //Gets the SpriteRender for the box
            _spriteRenderers.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
            //Gives it whatever pre made color I set up 
            _spriteRenderers[i].color = colors[i];
            //Finds out what position is the last to have color 
            if (_spriteRenderers[i].color != Color.clear)
            {
                lastActive = i;
            }
            
        }
    }

    //Returns the user the top color of the vial, can't mix two vials with different top colors 
    public Color GetTopColor()
    {
        return _spriteRenderers[lastActive].color;
    }

    //Adds the colors to the top and fills the space 
    public void AddColor(Color color)
    {
        lastActive++;
        _spriteRenderers[lastActive].color = color;
    }

    //If the vail is empty we just replace the clear with whatever color we want 
    public void ReplaceColor(Color color)
    {
        _spriteRenderers[lastActive].color = color;
    }

    //Subtracts the color and removes a space 
    public void SubColor()
    {
        _spriteRenderers[lastActive].color = Color.clear;
        lastActive--;
        if (lastActive < 0)
        {
            lastActive = 0;
        }
    }

    //Checks if the vail is at 0 and the color is set to be clear, if so that means the vial is empty 
    public bool IsEmpty()
    {
        return lastActive == 0 && _spriteRenderers[0].color == Color.clear;
    }
    
    //Checks if there is room in the vail for more colors 
    public bool HasSpace()
    {
        return ((3 - lastActive) > 0);
    }

    //Listens for a click from the user onto the vail, if the user doesn't have any vail in hand they select this one
    //If they have a vail in hand they will attempt to pour into the new selection 
    private void OnMouseDown()
    {
        if (GameObject.Find($"Camera").GetComponent<GameFlow>().IsHoldingVial())
        {
            GameObject.Find($"Camera").GetComponent<GameFlow>().HoldVial(transform.GetComponent<Vial>());
        }
        else
        {
            GameObject.Find($"Camera").GetComponent<GameFlow>().Pour(transform.GetComponent<Vial>());
        }
    }
}
