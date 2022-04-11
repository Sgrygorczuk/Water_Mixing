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

    //Updates the state, if state is
    //              true moves from CanBeAccessedNoWin to CanBeAccessedWin     
    //              false it moves from CantBeAccessed to CanBeAccessedNoWin
    public void SetIconValue(bool state)
    {
        currentIconState = state ? IconValue.CanBeAccessedWin : IconValue.CanBeAccessedNoWin;
    }
}
