using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.ClientManagers.Character;
using Assets.Scripts.ClientManagers.Armoury;
using Assets.Scripts.ClientManagers.Kingdom;
using Assets.Scripts.ClientManagers.Soupkitchen;
using Assets.Scripts.ClientManagers.Treasury;
using System.Threading.Tasks;


public class LoadingScreen : MonoBehaviour
{
    public static Stack<IEnumerator> Tasks = new Stack<IEnumerator>();

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI textProgress;
    [SerializeField] private TextMeshProUGUI textInfo;

    [SerializeField] private bool[] stagesCompleted;
    public bool[] StagesCompleted { get { return stagesCompleted; } set { stagesCompleted = value; } }
    //    set 
    //    {
    //        int count = 0;
    //        stagesCompleted = value; 
    //        foreach (bool stage in stagesCompleted)
    //        {
    //            if (stage)
    //                count++;
    //        }
    //        UpdateProgress(count / StagesCompleted.Length);
    //    } 
    //}
    public void IncrementStagesCompleted()
    {
        
            int count = 0;
            foreach (bool stage in stagesCompleted)
            {
                if (stage)
                    count++;
            }
            UpdateProgress((float)count / (float)StagesCompleted.Length);
        
    }

    public void UpdateProgress(float progress)
    {
        if (progress < 0)
            progress = -10;

        UpdateProgress(Mathf.CeilToInt(progress * 100f));
    }
    public void UpdateProgress(int progress)
    {
        if (textProgress == null)
            return;

        if (progress >= 0)
            textProgress.text = $"({progress:D2}%) Loading...";
        else
            textProgress.text = "Loading...";
    }
    public void UpdateInfo(string info)
    {
        if ((textInfo == null))
            return;

        textInfo.text = info;
    }
    

}
public static class LoadingScreenExtensions
{
    public static async Task LoadLoadingSceneAsync()
    {
        var op = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
    public static async Task UnloadLoadingSceneAsync()
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync("Loading");
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
    public static async Task LoadGameSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MonoMonarch", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
    public static async Task UnloadGameSceneAsync()
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync("MonoMonarch");
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
    public static async Task LoadMainMenuSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
    public static async Task UnloadMainMenuSceneAsync()
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync("MainMenu");
        while (!op.isDone)
        {
            await Task.Yield();
        }
    }
}