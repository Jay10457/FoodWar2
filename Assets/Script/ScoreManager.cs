using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    Animator gameOverPanel;
    int r_Score;
    int g_Score;
    FoodTeam teamVaule;
   
    private void Start()
    {
        gameOverPanel = WinOrLost.instance.gameOverAni;
        teamVaule = RoomManager.instance.myPlayer.myTeam;
        EventManager.instance.AddScore = SendScoreRequest;
        EventManager.instance.GameOver = GameOverRPC;
    }
    public void SendScoreRequest(int _score)
    {
        photonView.RPC("SendAddScoreRequest", RpcTarget.MasterClient, _score);
    }

    public void GameOverRPC()
    {
        photonView.RPC("SendGameResultCal", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void SendGameResultCal()
    {
        photonView.RPC("SendFinalScoreToAll", RpcTarget.All, r_Score, g_Score);
    }
    [PunRPC]
    private void SendFinalScoreToAll(int _r_Score, int _g_Score)
    {
        
        //Debug.LogError(string.Format("R:{0} G:{1}", _r_Score, _g_Score));

        WinOrLost.instance.rScore.text = Mathf.Abs(_r_Score).ToString();
        WinOrLost.instance.gScore.text = _g_Score.ToString();
        WinOrLost.instance.bar.value = (_r_Score + 10f) / ((_g_Score + 10f) + (_r_Score + 10f));
        if (teamVaule == FoodTeam.GOOD && _g_Score > _r_Score || teamVaule == FoodTeam.BAD && _g_Score < _r_Score)
        {
            gameOverPanel.SetTrigger("Win");
            Cursor.lockState = CursorLockMode.None;
           
        }
        else if (teamVaule == FoodTeam.GOOD && _g_Score < _r_Score || teamVaule == FoodTeam.BAD && _g_Score > _r_Score)
        {
            gameOverPanel.SetTrigger("Lost");
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            gameOverPanel.SetTrigger("Tie");
            Cursor.lockState = CursorLockMode.None;

        }
    }
    [PunRPC]
    private void SendAddScoreRequest(int _score)
    {
        if (_score > 0)
        {
            g_Score += _score;
        }
        else
        {
            r_Score += _score;
        }
        photonView.RPC("ConfirmAddScore", RpcTarget.All, r_Score, g_Score);
    }
    int r;
    int g;
    [PunRPC]
    private void ConfirmAddScore(int _r_Score, int _g_Score)
    {
       
        ScoreBarAndTimer.instance.g_Score.text = _g_Score.ToString();
        if (g != _g_Score)
        {
            
            g = _g_Score;
            
            ScoreBarAndTimer.instance.g_Score.gameObject.transform.localScale = Vector3.one;
            if (ScoreBarAndTimer.instance.g_Score.gameObject.transform.localScale == Vector3.one)
            {
                LeanTween.scale(ScoreBarAndTimer.instance.g_Score.gameObject, Vector3.one * 3f, 1f).setEase(LeanTweenType.punch);
               
            }
            
        }

        
        
        ScoreBarAndTimer.instance.r_Score.text = Mathf.Abs(_r_Score).ToString();
        if (r != _r_Score)
        {
            r = _r_Score;
           
            ScoreBarAndTimer.instance.r_Score.gameObject.transform.transform.localScale = Vector3.one;
            if (ScoreBarAndTimer.instance.r_Score.gameObject.transform.transform.localScale == Vector3.one)
            {
                LeanTween.scale(ScoreBarAndTimer.instance.r_Score.gameObject, Vector3.one * 3f, 0.5f).setEase(LeanTweenType.punch);
              
            }
           
        }

        ScoreBarAndTimer.instance.scoreSlider.value = (_r_Score+10f) / ((_g_Score+10f) + (_r_Score+10f));// 10/(10+10)
        
    }
}
