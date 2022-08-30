using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vial : MonoBehaviour
{
    //========= Color Control 
    //Preset colors at the beginning of the level 
    public Color[] colors = { Color.clear, Color.clear, Color.clear, Color.clear};
    //Holds all the box sprites of the colors 
    private readonly List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    //Tells us the top spot that currently has color 
    public int lastActive;
    
    //======== Components 
    private SpriteRenderer _spriteRenderer;     //Sprite renderer to change 
    //The sprites can be different for each level 
    public Sprite openVailSprite;               //What the vial looks like at the start
    public Sprite fullVailSprite;               //What the vial looks like when it's filled with same color 
    private Animator _animator;                 //Controls the animation of the vial 
    private Vector3 _originalPosition;          //Tells us where the vial original sat 
    private Vector3 _targetPosition;            //Tells us where the vial should move to to pour 

    //=========== Flags 
    private bool _isMoving;         //Tells us if the vial is moving 
    private bool _goingBack;        //Tells us which directions the vial is moving 
    private bool _isActive;         //Tells us if the vial is actively being used to pour preventing it from being touched 
    private bool _isClosed;         //Tells us if the vial is filled with the same color and the sprite is changed 
    
    //========= Vials 
    private Vial _pour; //The vial that we will pour into
    private Vial _hold; //The vial that is being poured from 

    //============================================== METHODS ===========================================================
    
    // Start is called before the first frame update
    public void Start()
    {
        for (var i = 0; i < 4; i++)
        {
            //Gets the SpriteRender for the box
            _spriteRenderers.Add(transform.Find($"SpriteMask").GetChild(i).GetComponent<SpriteRenderer>());
            //Gives it whatever pre made color I set up 
            _spriteRenderers[i].color = colors[i];
            //Finds out what position is the last to have color 
            if (_spriteRenderers[i].color != Color.clear)
            {
                lastActive = i;
            }
        }
        
        //Initializing the components 
        _originalPosition = transform.position;
        _animator = GetComponent<Animator>();
        _spriteRenderer = transform.Find("VailSprite").GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = openVailSprite;
    }
    
    //============================================== MOVING ============================================================

    //Used primarily to move the vail to and from the vial we will be pouring into 
    public void Update()
    {
        if (!_isMoving) return;
        //Moves the vial 
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, 3.0f * Time.deltaTime);
        
        //If we reached the vail that we will pour into, make it look like it's pouring into the vial 
        if (transform.position == _targetPosition && !_goingBack)
        {
            _animator.Play($"Vail");
            _isMoving = false;
            _animator.Play($"VailTilt_1_0");
            StartCoroutine(Drain());
        }
        //If we reach back the original position turn everything off 
        else if (transform.position == _targetPosition && _goingBack)
        {
            _animator.Play($"Vail");
            _isActive = false;
            _isMoving = false;
            _goingBack = false;
        }
    }
    
    //Gives the position of the pour vial that we will move towards 
    public Vector3 GetHoverPoint()
    {
        return transform.Find($"HoverPoint").position;
    }

    //Used to initiate the moving sequence of the vial 
    public void StartMoving(Vector3 target, Vial pour, Vial hold)
    {
        _pour = pour;
        _hold = hold;
        _isMoving = true;
        _targetPosition = target;
        _isActive = true;
    }

    //Updates the values in the different vials 
    private IEnumerator Drain()
    {
        yield return new WaitForSeconds(0.3f);
        //Pours and gets rid of the references 
        GameFlow.Pour(_pour, _hold);
        _pour = null;
        _hold = null;
        yield return new WaitForSeconds(1);
        //Sets the vial to move back where it started 
        _targetPosition = _originalPosition;
        _isMoving = true;
        _goingBack = true;
    }
    
    //Starts the animation that indicates the vial is selected 
    public void SelectedAnimation()
    {
        _animator.Play($"VailSelected");
    }
    
    //Stops the vial from moving around 
    public void StopAnimation()
    {
        _animator.Play($"Vail");
    }
    
    //================================================= VIAL MIXING ====================================================

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

        //When the bottle is done close it up 
        if (lastActive == 3 && CheckIfClosed())
        {
            StartCoroutine(CloseBottle());
        }
    }

    //Closes the bottle by changing the sprite and playing a SFX 
    private IEnumerator CloseBottle()
    {
        _animator.Play($"Vail_Change");
        yield return new WaitForSeconds(1);
        _spriteRenderer.sprite = fullVailSprite;
        //TODO Play sound effect 
        yield return new WaitForSeconds(1);
        GameObject.Find($"GameFlow").GetComponent<GameFlow>().AddFull();
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

    //Checks if the bottle is full but not with the same color 
    private bool IsFull()
    {
        return lastActive == 3;
    }
    
    //Checks if there is room in the vail for more colors 
    public bool HasSpace()
    {
        return ((3 - lastActive) > 0);
    }
    
    //Checks if the bottle is filled all with the same color, if so sets the flag and update the sprite 
    private bool CheckIfClosed()
    {
        var color = _spriteRenderers[0].color;
        if (_spriteRenderers.Any(sprite => sprite.color != color))
        {
            return false;
        }
        _isClosed = true;
        return _isClosed;
    }
    
    //============================================ USER INTERACTION  ===================================================

    //Listens for a click from the user onto the vail, if the user doesn't have any vail in hand they select this one
    //If they have a vail in hand they will attempt to pour into the new selection 
    private void OnMouseDown()
    {
        if (GameObject.Find($"GameFlow").GetComponent<GameFlow>().IsHoldingVial() && !_isClosed && !_isActive)
        {
            if(IsEmpty()) return;
            GameObject.Find($"GameFlow").GetComponent<GameFlow>().HoldVial(transform.GetComponent<Vial>());
        }
        else
        {
            if(_isActive || IsFull()) return;
            GameObject.Find($"GameFlow").GetComponent<GameFlow>().MoveToPour(transform.GetComponent<Vial>());
        }
    }
}
