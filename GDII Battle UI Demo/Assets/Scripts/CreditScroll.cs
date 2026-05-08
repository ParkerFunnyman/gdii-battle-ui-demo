using UnityEngine;
using UnityEngine.InputSystem;
using UnityScene = UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    public float scrollSpeed = 45.0f;

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        if ((Keyboard.current.escapeKey.wasPressedThisFrame) || (Keyboard.current.backspaceKey.wasPressedThisFrame))
        {
            UnityScene.SceneManager.LoadScene(0);
        }
    }
}