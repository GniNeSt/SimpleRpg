using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager._instance.LoadAll();
        UserInfoManager._instance.SettingPlayerAvatar(null);

        SceneControlManager._instance.LoadIngameScene();
    }
}
