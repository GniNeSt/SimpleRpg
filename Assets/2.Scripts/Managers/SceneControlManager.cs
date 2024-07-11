using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;
using UnityEngine.SceneManagement;
public class SceneControlManager : TSingleTon<SceneControlManager>
{
    protected override void Init()
    {
        base.Init();
    }

    SceneName _CurrentScene;

    public void LoadIngameScene()
    {
        StartCoroutine(LoadingScene(SceneName.IngameScene, 1));
        _CurrentScene = SceneName.IngameScene;
    }

    IEnumerator LoadingScene(SceneName Scene, int stageIndex = 0)
    {
        AsyncOperation aOper;
        //���ε�
        aOper = SceneManager.LoadSceneAsync(Scene.ToString());
        while (!aOper.isDone)
        {
            yield return null;
        }
        //���ʱ�ȭ

        //�� üũ �� Map Load
        if(SceneName.IngameScene == Scene)
        {
            string mapScene = "MapScene";
            aOper = SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);
            while (!aOper.isDone)
            {
                yield return null;
            }
            Scene active = SceneManager.GetSceneByName(mapScene);
            SceneManager.SetActiveScene(active);
        }

        //�� ������ ����
        if (SceneName.IngameScene == Scene)
        {
            aOper = SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);
            while (!aOper.isDone)
            {
                yield return null;
            }
            Scene active = SceneManager.GetSceneByName(mapScene);
            SceneManager.SetActiveScene(active);
        }
    }
}
