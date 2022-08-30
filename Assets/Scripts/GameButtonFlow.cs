using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtonFlow : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //======== Sound Button 
    private Sprite _soundOnSprite;  //Sprite Used to show sound is on
    private Sprite _soundOffSprite; //Sprite used to show sound is off 
    private Image _soundImage;      //Image that will take the sprites  
    
    //Used to set different states for sound 
    private enum SoundState  
    {
        On,
        Off
    }

    private SoundState _currentSoundState = SoundState.On; //Used to keep track of what is the current sound state 
    
    //======== Level Transitions 
    public string levelName;        //Used to reload level 
    public string nextLevelName;    //Used to move to the next level in line 

    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    private void Start()
    {
        //Connects the sprites  
        var sprites =  Resources.LoadAll<Sprite>("Sprites/Icons");
        print(sprites.Length);
        _soundOnSprite = sprites[0];
        _soundOffSprite = sprites[1];

        //Connects the image 
        _soundImage = GameObject.Find("GameFlow").transform.Find("PlayingCanvas").transform.Find("SoundButton")
            .GetComponent<Image>();
        _soundImage.sprite = _soundOnSprite;
    }

    //==================================================================================================================
    // Button Methods  
    //==================================================================================================================
    //Based on what is the current sound state either turns all of the audio on/off and updates button Image to reflect it 
    public void SoundChange()
    {
        switch (_currentSoundState)
        {
            case SoundState.On:
            {
                _soundImage.sprite = _soundOffSprite;
                _currentSoundState = SoundState.Off;
                //TODO Turn Off Sound 
                break;
            }
            case SoundState.Off:
            {
                _soundImage.sprite = _soundOnSprite;
                _currentSoundState = SoundState.On;
                //TODO Turn On Soun09
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    //Sends the player back to map, used from settings bar or once the level is beaten 
    public void GoToMap()
    {
        StartCoroutine(ChangeScene($"MapScene"));
    }

    //Send player to the next level once the level is beaten 
    public void GoToNextLevel()
    {
        StartCoroutine(ChangeScene(nextLevelName));
    }

    //Reloads the level from the settings bar 
    public void ReloadLevel()
    {
        StartCoroutine(ChangeScene(levelName));
    }
    
    
    //Uses the Transition Canvas to Fade everything away before moving to the next level  
    private static IEnumerator ChangeScene(string levelName)
    {
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Canvas>().sortingOrder = 13;
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Animator>().Play($"SceneFadeIn");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(levelName);
    }
}
