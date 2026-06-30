using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{

    public GameObject GameOver;
    
    public void Start()
    {
        
    }

    public void Reset()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
