using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineUtilitys;
using DefineEnums;
using System.Xml;
using System.IO;

public class UserInfoManager : TSingleTon<UserInfoManager>
{
    //Avatar ����
    AvatarInformatin _avatarInfo;
    int _nextXP;
    //==

    public AvatarInformatin _nowAvatarInfo
    {
        get { return _avatarInfo; }
    }
    public int _targetXP
    {
        get { return _nextXP; }
    }
    public long _holdingMoney
    {
        get;set;
    }
    protected override void Init()
    {
        base.Init();

        //�ε� �� ������ �⺻ �� ����
        try
        {
            //���� �ε�
            string fullname = Application.dataPath + "UserInfo.txt";
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(fullname);


        }
        catch(FileNotFoundException ex)
        {
            //�⺻�� ����
            InitSetStandard("ȫ�浿");
        }
    }
    public void UserInfoSaveFile()
    {
        string fullname = Application.dataPath + "UserInfo.txt";
        XmlDocument xmlFile = new XmlDocument();
    }
    public void SetXP(int currentXp)
    {
        _avatarInfo._nowXp = currentXp;
        if(_nextXP <= _avatarInfo._nowXp)
        {
            //������
        }
        //����
        UserInfoSaveFile();
    }
    void InitSetStandard(string name)
    {
        //Hp 100
        //Att 5
        //Def 1
        //Avd 60
        //Hit 140
        _avatarInfo = new AvatarInformatin(name, 1, 5, 1, 100, 60, 140, 0);
        _nextXP = GetNextXp(1);
    }
    int GetNextXp(int nowLevel)
    {
        int next = 0;
        for(int n = 1; n <= nowLevel; n++)
            next += TableManager._instance.Get(TableName.LevelUpTable).ToInteger(1, "ReqXp");

        return next;
    }
    public void SettingPlayerAvatar(PlayerCtrlObj obj)
    {

    }
}
