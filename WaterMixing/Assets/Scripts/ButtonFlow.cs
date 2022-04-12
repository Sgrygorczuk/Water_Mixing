using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFlow : MonoBehaviour
{

    public string nextLevelName;

    public void GoToMap()
    {
        StartCoroutine(ChangeScene($"MapScene"));
    }

    public void GoToNextLevel()
    {
        StartCoroutine(ChangeScene(nextLevelName));
    }
    
    //Updates the values in the different vials 
    private static IEnumerator ChangeScene(string levelName)
    {
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Canvas>().sortingOrder = 13;
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Animator>().Play($"SceneFadeIn");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(levelName);
    }
}
