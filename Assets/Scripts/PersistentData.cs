using System;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    //Variables 
    private static PersistentData _instance; //Is the instance of the object that will show up in each scene 
    public IconValues.IconValue[] iconValuesArray;

    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================
    
    //Creates the object, if one already has been created in another scene destroy this one and the make a new one 
    private void Awake()
    {
        //Checks if there already exits a copy of this, if does destroy it and let the new one be created 
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //==================================================================================================================
    // Data Update Methods 
    //==================================================================================================================

    //If player collected a fruit this will update the value to true
    public void SetLevelState(int pos)
    {
        iconValuesArray[pos] = IconValues.IconValue.CanBeAccessedWin;
        if (pos + 1 < iconValuesArray.Length)
        {
            iconValuesArray[pos + 1] = IconValues.IconValue.CanBeAccessedNoWin;
        }
    }

    //Returns the list of collected fruits, used in start of each level and check in the end scene 
    public IconValues.IconValue[] GetIconValues()
    {
        return iconValuesArray;
    }
}
