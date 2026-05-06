using UnityEngine;
using UnityScene = UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void loadGame()
    {
        UnityScene.SceneManager.LoadScene(1);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
