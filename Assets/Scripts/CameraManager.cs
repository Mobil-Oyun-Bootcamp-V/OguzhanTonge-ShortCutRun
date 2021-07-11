using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // We set our camera as cinemachine.
    [SerializeField] private CinemachineVirtualCamera introCamera,gamePlayCamera, finishCamera;
    public static CameraManager instance;
    private void Awake()
    {
        instance = this;
    }
    // We adjusted our camera transitions according to the game states.
    #region CameraTransitions
    public void IntroToMain()
    {
        introCamera.enabled = false;
        gamePlayCamera.enabled = true;
        finishCamera.enabled = false;
    }
    public void MainToFinish()
    {
       introCamera.enabled = false;
        gamePlayCamera.enabled = false;
        finishCamera.enabled = true;
    }
    public void FinishToIntro()
    {
        introCamera.enabled = true;
        gamePlayCamera.enabled = false;
        finishCamera.enabled = false;
    }
    #endregion

    public IEnumerator IntroToMain_()
    {
        yield return new WaitForSeconds(1f);
        IntroToMain();
    }
   
}
