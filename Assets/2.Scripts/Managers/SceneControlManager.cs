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

    IEnumerator LoadingScene(SceneName Scene, int stageIndex = 1)
    {
        AsyncOperation aOper;
        //æ¿∑ŒµÂ
        aOper = SceneManager.LoadSceneAsync(Scene.ToString());
        while (!aOper.isDone)
        {
            yield return null;
        }
        //æ¿√ ±‚»≠

        //æ¿ √º≈© »ƒ Map Load
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


            //∏  µ•¿Ã≈Õ º≥¡§
            GameObject mapRoot = GameObject.FindGameObjectWithTag("MapRoot");

            string stageName = TableManager._instance.Get(TableName.StageInfoTable).ToString(stageIndex, mapScene);
            GameObject go = Resources.Load("Prefabs/Maps/" + stageName) as GameObject;
            Transform pSpawnPoint = go.transform.GetChild(0);
            Instantiate(go, mapRoot.transform);

            //Player, MonsterSpawn
            go = Resources.Load("Prefabs/Objects/PlayerObj") as GameObject;
            Instantiate(go, pSpawnPoint.position, Quaternion.identity);
            go = Resources.Load("Prefabs/Opts/OptsStage1") as GameObject;
            Instantiate(go, mapRoot.transform);


            //progress √º≈©
        }

    }
}
