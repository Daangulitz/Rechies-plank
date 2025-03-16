using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathscene : MonoBehaviour
{
    [SerializeField] private GameObject Doll1;
    [SerializeField] private GameObject Doll2;
    public GameObject SFX;
    
    private void OnTriggerEnter(Collider other)
    {
        Doll1.SetActive(false);
        Doll2.SetActive(false);
        SFX.SetActive(false);
        SceneManager.LoadScene("Death");
    }
}
