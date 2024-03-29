using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    //=========== Animations 
    public List<Animator> animators;         //Stores all the animators that we will go through
    public List<string> animationNames;      //Holds all the names for each animator we want to call
    private List<float> _animationLenght;    //Holds hold long each animation lasts 
    private int _currentPosition;            //Tells us which animation we're playing 

    //========== Progress Indicators 
    private TextMeshPro _spaceText;                                 //Holds the data to the continue text 
    private SpriteRenderer _spaceBackground;                        //Holds the data for the background of the text 
    private bool _isDonePlaying = false;                            //Tells us if the animation has finished playing 
    
    //========== Misc 
    private float _currentY;                                        //Used to check if the screen moved far enough 
    
    //===== Game Flow 
    private enum GameState
    {
        PlayingAnimation, 
        MoveScreen,
    }
    
    private GameState _currentState = GameState.PlayingAnimation;      //Keeps track of what state we're currently in
    public string nextScene = "Level";



    // Start is called before the first frame update
    private void Start()
    {
        //Pulls all the data from the parent game object 
        _spaceText = transform.Find($"SpaceText").GetComponent<TextMeshPro>();
        _spaceBackground = transform.Find($"Space").GetComponent<SpriteRenderer>();
        _currentY = transform.position.y;
            
        //Gets the lengths for each of the animations 
        for (var i = 0; i < animators.Count; i++)
        {
            GetLenght(animators[i], animationNames[i]);
        }
        
        //Makes the indicator to continue invisible 
        SetAlpha(false);
        
        //Starts the first animation 
        PlayAnimation();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ChangeScene());
        }
        
        //Checks what state the game is currently in and updates it 
        switch (_currentState)
        {
            case GameState.PlayingAnimation:
            {
                PlayingAnimation();
                break;
            }
            case GameState.MoveScreen:
            {
                MoveScreen();
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //Checks for if the animations has concluded playing and if the player has clicked button to continue, 
    //if there are more animations we go to the next else we go to the game screen 
    private void PlayingAnimation()
    {
        //Next animation
        if (Input.GetButtonDown($"Action") && _isDonePlaying && _currentPosition < animators.Count)
        {
            _currentState = GameState.MoveScreen;
            SetAlpha(false);
            _isDonePlaying = false;
        }
        //Game Scene 
        else if (Input.GetButtonDown($"Action") && _isDonePlaying && _currentPosition == animators.Count)
        {
            SetAlpha(false);
            _isDonePlaying = false;
            StartCoroutine(ChangeScene());
        }
    }
    
    //Moves you to next scene with a Fade 
    private IEnumerator ChangeScene()
    {
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Animator>().Play($"SceneFadeIn");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(nextScene);
    }

    //When it in between animations the screen moves to the other main page, once it reaches it it starts playing next 
    //animation 
    private void MoveScreen()
    {
        if (transform.position.y < _currentY - 10)
        {
            PlayAnimation();
            _currentState = GameState.PlayingAnimation;
            _currentY = transform.position.y;
        }
        else
        {
            transform.position += Vector3.down / 10;
        }
    }
    
    
    //Sets the visibility to either clear or visible for the Button Prompt 
    private void SetAlpha(bool alpha)
    {
        _spaceText.color = alpha ? Color.white : Color.clear;
        _spaceBackground.color = alpha ? Color.black : Color.clear;
    }

    //Gets the length of time an animation lasts and adds it to the List 
    private void GetLenght(Animator anim, string animationName)
    {
        var clips = anim.runtimeAnimatorController.animationClips;
        foreach(var clip in clips)
        {
            if (clip.name != animationName) continue;
            _animationLenght.Add(clip.length);
            return;
        }
    }

    //Gets the animation to play and sets off a coroutine that will give player button access 
    private void PlayAnimation()
    {
        animators[_currentPosition].Play(animationNames[_currentPosition]);
        StartCoroutine(WaitForAnimationToFinish());
    }
    
    //2 Seconds after the jump the player gets back half their initial falling speed 
    private IEnumerator WaitForAnimationToFinish()
    {
        yield return new WaitForSeconds(_animationLenght[_currentPosition] + 1);
        _currentPosition++;
        _isDonePlaying = true;
        SetAlpha(true);
    }
}
