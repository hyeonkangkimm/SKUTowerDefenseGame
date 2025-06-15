using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyAudioOnSceneChange : MonoBehaviour
{
    private string initialSceneName;

    void Awake()
    {
        // 현재 씬 이름 저장
        initialSceneName = SceneManager.GetActiveScene().name;

        // 씬 변경 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 씬과 처음 씬이 다르면 삭제
        if (scene.name != initialSceneName)
        {
            Destroy(gameObject);
        }
    }
}
