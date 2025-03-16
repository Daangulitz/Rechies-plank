using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public void BacktoGame()
    {
        SceneManager.LoadScene("Main");
    }
}
