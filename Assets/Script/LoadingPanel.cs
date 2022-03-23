using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] VideoClip[] videos;
    [SerializeField] RenderTexture renderImage;
    [SerializeField] Slider progressBar;
    [SerializeField] string[] sceneToLoad;// sceneName 
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] TMP_Text progressText;
    [SerializeField] ©Ð¶¡¤¤ inRoom;
    
    
    public string sceneName;
    float progress = 0f;

   
    private void OnEnable()
    {
        RandomLoadingVideo();
       
        StartCoroutine(LoadingScene());

    }

    public IEnumerator LoadingScene()
    {



        while (progress < 0.99f)
        {
            progress = Mathf.Lerp(progress, inRoom.async.progress / 9 * 10, Time.deltaTime);
            //progress += Mathf.Clamp(0, 0.01f, BoltNetwork.CurrentAsyncOperation.progress);
            progressBar.value = progress;
            progressText.SetText(Mathf.Floor(progress * 100f).ToString() + "%");
            yield return null;
        }

        progress = 1f;
        progressBar.value = 1f;

        inRoom.async.allowSceneActivation = true;
        progress = 0f;
        
        
        // Debug.LogError(sceneToLoad);




    }
    void RandomLoadingVideo()
    {
        videoPlayer.clip = videos[Random.Range(0, videos.Length)];
    }
}
