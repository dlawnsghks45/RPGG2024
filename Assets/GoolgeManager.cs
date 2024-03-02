using UnityEngine;
using Google.Play.Review;
using Cysharp.Threading.Tasks;
using Google.Play.Common;
using Google.Play.AppUpdate;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;


public class GoolgeManager : MonoBehaviour
{
      public static GoolgeManager Instance { get; private set; }

    ReviewManager _reviewManager = null;
    AppUpdateManager appUpdateManager = null;

    private const string REVIEW_URL = "https://play.google.com/store/apps/details?id=com.LKGames.com.unity.RPGG2";

    public void Awake()
    {
        Instance = this;

    }
    public void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// ?¥î? ???? ??? ???.
    /// </summary>
    public void LaunchReview()
    {
        try
        {
            _reviewManager = new ReviewManager();
            var playReviewInfoAsyncOperation = _reviewManager.RequestReviewFlow();

            playReviewInfoAsyncOperation.Completed += playReviewInfoAsync =>
            {
                if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
                {
                    var playReviewInfo = playReviewInfoAsync.GetResult();
                    if (playReviewInfo == null)
                    {
                        //null?? ??? ???????? ???????????? ???? ???.
                        OpenUrl();
                    }
                    else
                    {
                        //?¥î???? ????.
                        _reviewManager.LaunchReviewFlow(playReviewInfo);
                    }
                }
                else
                {
                    OpenUrl();
                }
            };
        }
        catch(Exception e)
        {
            OpenUrl();
            Debug.Log(e.Message);
        }
    }
    
 

    /// <summary>
    /// ?¥î? ??????? ??? ???.
    /// </summary>
    /// <returns></returns>
    public async UniTask UpdateApp()
    {
        try
        {
            appUpdateManager = new AppUpdateManager();

            PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =  appUpdateManager.GetAppUpdateInfo();
            await appUpdateInfoOperation;

            if (appUpdateInfoOperation.IsSuccessful)
            {
                var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                await startUpdateRequest;
            }
            
            else
            {
                Debug.Log(appUpdateInfoOperation.Error);
                Application.Quit();
            }
        }
        catch(Exception e){Debug.Log(e.Message);}
    }

    public void OpenUrl()
    {
        Application.OpenURL(REVIEW_URL);
    }
}