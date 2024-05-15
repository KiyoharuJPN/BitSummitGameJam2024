using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneButtonUI : MonoBehaviour
{
    public string ChangeStageTo;
    public void SelectScene()
    {
        if(ChangeStageTo != "")
        {
            SceneManager.LoadScene(ChangeStageTo);
        }
    }
}
