using UnityEngine;
public class IconValues : MonoBehaviour
{
    //This is a data structure used between multiple scripts 
    public enum IconValue
    {
        CantBeAccessed,       //The player hasn't unlocked this level yet
        CanBeAccessedNoWin,   //The most recent level player can play that they haven't beat    
        CanBeAccessedWin      //Player beat this level and can replay it
    }
    
    public IconValue currentIconState = IconValue.CantBeAccessed;     //Current state of the icon
    
    public void SetIconValue(IconValue iconValue)
    {
        currentIconState = iconValue;
    }
}
