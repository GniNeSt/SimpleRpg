using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MiniMonInfoWnd : MonoBehaviour
{
    [SerializeField] Slider _hpBar;
    [SerializeField] TextMeshProUGUI _textName;

    const float _visibleTime = 2f;

    float _checkTime;

    private void Update()
    {
        _checkTime += Time.deltaTime;
        if(_checkTime >= _visibleTime)
        {
            CloseWnd();
        }
        

    }
    public void InitOpen(string name)
    {
        _textName.text = name;
        _hpBar.value = 1;
        CloseWnd();
    }
    public void OpenWnd()
    {
        _checkTime = 0;
        gameObject.SetActive(true);
    }
    public void CloseWnd()
    {
        gameObject.SetActive(false);
    }
    public void SetHpRate(float rate)
    {
        _checkTime = 0;
        _hpBar.value = rate;
        OpenWnd();
    }
}
