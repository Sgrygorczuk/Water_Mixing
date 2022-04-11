using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIcons : MonoBehaviour
{
    //======== Icon Settings 
    public bool isLevel;            //Tells us if it's an icon of a level or cutscene 
    private IconValues _iconValues; //Tells us if the icon is off, on or beaten 
    public int iconValue;           //Used for testing 

    //======== Next Level 
    public string levelName;    //

    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================
    
    private void Awake()
    {
        _iconValues = gameObject.AddComponent<IconValues>();
        SetIconValue();
    }

    private void Start()
    {
        SetSprite();
    }
    
    //==================================================================================================================
    // Setting Functions 
    //==================================================================================================================
    
    //Used in Map Flow to set all icons on awake 
    public void SetIconValue()
    {
        _iconValues.currentIconState = iconValue switch
        {
            0 => IconValues.IconValue.CantBeAccessed,
            1 => IconValues.IconValue.CanBeAccessedNoWin,
            2 => IconValues.IconValue.CanBeAccessedWin,
            _ => _iconValues.currentIconState
        };
    }

    //Sets the sprite based on if it's a level or cutscene and on state of players progress in the game 
    private void SetSprite()
    {
        //Connects to sprites and the sprite rendered 
        var sprites = Resources.LoadAll<Sprite>("Sprites/Map_Icons");
        var spriteRenderer = transform.Find($"Sprite").GetComponent<SpriteRenderer>();

        //Based on if it's a level or a cutscene choose different set of sprites then choose the sprite based on 
        //the IconValue 
        if (isLevel)
        {
            spriteRenderer.sprite = _iconValues.currentIconState switch
            {
                IconValues.IconValue.CantBeAccessed => sprites[7],
                IconValues.IconValue.CanBeAccessedNoWin => sprites[3],
                IconValues.IconValue.CanBeAccessedWin => sprites[2],
                _ => spriteRenderer.sprite
            };
        }
        else
        {
            spriteRenderer.sprite = _iconValues.currentIconState switch
            {
                IconValues.IconValue.CantBeAccessed => sprites[4],
                IconValues.IconValue.CanBeAccessedNoWin => sprites[0],
                IconValues.IconValue.CanBeAccessedWin => sprites[1],
                _ => spriteRenderer.sprite
            };
        }
    }

    //==================================================================================================================
    // User Interaction 
    //==================================================================================================================
    
    //If player taps on the icon is sends them to 
    private void OnMouseDown()
    {
        if (levelName.Length > 0 && _iconValues.currentIconState != IconValues.IconValue.CantBeAccessed)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
