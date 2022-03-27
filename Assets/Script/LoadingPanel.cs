using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoadingPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] VideoClip[] videos;
    [SerializeField] RenderTexture renderImage;
    [SerializeField] Slider progressBar;
    [SerializeField] string[] sceneToLoad;// sceneName 
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] TMP_Text progressText;

    AsyncOperation async;

    public string sceneName;
    float progress = 0f;

    private void Start()
    {
        progress = 0f;
        RandomLoadingVideo();
        if (PhotonNetwork.IsMasterClient)
        {
            async = SceneManager.LoadSceneAsync(sceneToLoad[Random.Range(0, sceneToLoad.Length)]);
            async.allowSceneActivation = false;
            StartCoroutine(LoadingScene());
        }
        else
        {
            StartCoroutine(ClinetLoading());
        }
        
       
    }

   public IEnumerator ClinetLoading()
    {
        while (progress < 0.99)
        {
            progress += 0.5f * Time.deltaTime;
            progressBar.value = progress;
            progressText.SetText(Mathf.Floor(progress * 100f).ToString("0") + "%");
            yield return null;
        }
        
    }
    
   public IEnumerator LoadingScene()
   {



       while (progress < 0.99f)
       {
           progress = Mathf.Lerp(progress, async.progress / 9 * 10, Time.deltaTime);
           //progress += Mathf.Clamp(0, 0.01f, BoltNetwork.CurrentAsyncOperation.progress);
           progressBar.value = progress;
           progressText.SetText(Mathf.Floor(progress * 100f).ToString() + "%");
           yield return null;
       }

       progress = 1f;
       progressBar.value = 1f;

       async.allowSceneActivation = true;
       progress = 0f;


       // Debug.LogError(sceneToLoad);


     

}
      
    void RandomLoadingVideo()
    {
        videoPlayer.clip = videos[Random.Range(0, videos.Length)];
    }
}
