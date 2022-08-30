using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(ChangeScene());
        }
    }
    
    //Updates the values in the different vials 
    private static IEnumerator ChangeScene()
    {
        GameObject.Find($"SceneTransitionCanvas").GetComponent<Animator>().Play($"SceneFadeIn");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("CutSceneOne");
    }
}
