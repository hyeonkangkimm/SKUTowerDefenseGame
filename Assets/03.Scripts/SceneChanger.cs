using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public Image fadePanel; // 검정 이미지
    public float fadeDuration = 1f;

    public void StartFadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad("KIMHYEONKANG"));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // 페이드 아웃
        float time = 0f;
        Color color = fadePanel.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        // 씬 비동기 로드
        AsyncOperation op = SceneManager.LoadSceneAsync("KIMHYEONKANG");
        yield return op;
    }


}