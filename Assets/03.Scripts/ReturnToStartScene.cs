using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToStartScene : MonoBehaviour
{
    public void OnClickReturnButton()
    {
        SceneManager.LoadScene("START");
    }
}
