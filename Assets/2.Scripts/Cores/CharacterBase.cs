using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineUtilitys;
public abstract class CharacterBase : MonoBehaviour
{
    protected string _name;
    protected StatInformation _baseStat;
    protected int _nowHp;

    public int Attack
    {
        get { return _baseStat._charAtt; }
    }

    public bool _isDead { get; set; }
    protected void InitSetStat(AvatarInformatin info)
    {
        _isDead = false;
        _name = info._charName;
        _baseStat._charAtt = info._stat._charAtt;
        _baseStat._charDef = info._stat._charDef;
        _baseStat._charHP = info._stat._charHP;
        _baseStat._charAvd = info._stat._charAvd;
        _baseStat._charHit = info._stat._charHit;
        _nowHp = _baseStat._charHP = info._stat._charHP;
    }
    protected void InitSetStat(string name, StatInformation stat)
    {
        _isDead = false;
        _name = name;
        _baseStat._charAtt = stat._charAtt;
        _baseStat._charDef = stat._charDef;
        _baseStat._charHP = stat._charHP;
        _baseStat._charAvd = stat._charAvd;
        _baseStat._charHit = stat._charHit;
        _nowHp = _baseStat._charHP = stat._charHP;
    }

    public abstract void OnHitting(CharacterBase attacker);
}
