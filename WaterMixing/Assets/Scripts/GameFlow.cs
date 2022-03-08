using System;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    //====== Misc 
    public Vial heldVial; //Tells us if a user has already clicked on a vial
    
    //Checks if there is a vial in hand, otherwise the use will select it as it's current vial 
    public bool IsHoldingVial()
    {
        return heldVial == null;
    }

    //Copies whatever vail the user clicked to be the currently held vial 
    public void HoldVial(Vial vial)
    {
        heldVial = vial;
        heldVial.SelectedAnimation();
    }
    
    //Grabs the pouring vial, determines if we can pour into them if so start the process otherwise disengage  
    public void MoveToPour(Vial vial)
    {
        //Checks that we didn't click on the same vial 
        if (vial == heldVial) return;

        //If the pouring vial can be poured into we do it 
        if (heldVial.GetTopColor() == vial.GetTopColor() || vial.IsEmpty())
        {
            heldVial.StartMoving(vial.GetHoverPoint(), vial, heldVial);
        }
        //Otherwise we stop the whole thing and let go of both vials 
        else
        {
            heldVial.StopAnimation();
        }
        
        heldVial = null;
    }

    //Checks if the second vail the user has clicked matches the color, if it does it pours the color 
    //Otherwise it disengages and the hand is now empty 
    public static void Pour(Vial pouring, Vial held)
    {
        //Puts th vial local vars so that the player can start clicking on other things 
        var internalHeldVial = held ? held : throw new ArgumentNullException(nameof(held));
        var internalPourVial = pouring ? pouring : throw new ArgumentNullException(nameof(pouring));

        //Repeat till all the colors have been moved between vials  
        while (true)
        {
            if (internalPourVial.IsEmpty())
            {
                internalPourVial.ReplaceColor(internalHeldVial.GetTopColor());
                internalHeldVial.SubColor();
                continue;
            }

            //If there is space and the colors match add to the second clicked and remove from the one in hand 
            if (internalPourVial.HasSpace() && internalHeldVial.GetTopColor() == internalPourVial.GetTopColor())
            {
                internalPourVial.AddColor(internalHeldVial.GetTopColor());
                internalHeldVial.SubColor();
                continue;
            }
            break;
        }
    }
}
