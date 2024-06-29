using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineUtilitys;
public class CharacterBase : MonoBehaviour
{
    protected string _name;
    protected StatInformation _baseStat;
    protected int _nowHp;


    public bool _isDead { get; set; }
    protected void InitSetStat(AvatarInformatin info)
    {
        _isDead = false;
        name = info._charName;
        _baseStat._charAtt = info._stat._charAtt;
        _baseStat._charDef = info._stat._charDef;
        _baseStat._charHP = info._stat._charHP;
        _baseStat._charAvd = info._stat._charAvd;
        _baseStat._charHit = info._stat._charHit;
    }
    public void OnHitting(CharacterBase attacker )
    {

    }
}
