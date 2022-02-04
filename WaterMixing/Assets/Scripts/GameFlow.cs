using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    //====== Misc 
    public Vial _heldVial = null; //Tells us if a user has already clicked on a vial

    //Checks if there is a vial in hand, otherwise the use will select it as it's current vial 
    public bool IsHoldingVial()
    {
        return _heldVial == null;
    }

    //Copies whatever vail the user clicked to be the currently held vial 
    public void HoldVial(Vial vial)
    {
        _heldVial = vial;
    }

    //Checks if the second vail the user has clicked matches the color, if it does it pours the color 
    //Otherwise it disengages and the hand is now empty 
    public void Pour(Vial vial)
    {
        //Repeat till all the colors have been moved between vials  
        while (true)
        {
            if (vial.IsEmpty())
            {
                vial.ReplaceColor(_heldVial.GetTopColor());
                _heldVial.SubColor();
                continue;
            }
            
            //If there is space and the colors match add to the second clicked and remove from the one in hand 
            if (vial.HasSpace() && _heldVial.GetTopColor() == vial.GetTopColor())
            {
                vial.AddColor(_heldVial.GetTopColor());
                _heldVial.SubColor();
                continue;
            }
            
            _heldVial = null;
            break;
        }
    }
}
