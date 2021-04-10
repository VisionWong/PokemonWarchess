﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    private Transform _hpBarBg;
    private Image _hpBar1;//血条上层
    private Image _hpBar2;//血条下层慢放
    private float _lastBar1FillAmount;

    private void Awake()
    {
        _hpBarBg = transform.Find("Canvas").Find("hpbg");
        _hpBar1 = _hpBarBg.Find("hpBar1").GetComponent<Image>();
        _hpBar2 = _hpBarBg.Find("hpBar2").GetComponent<Image>();
        _lastBar1FillAmount = _hpBar1.fillAmount;
    }

    public void ChangeHPValue(float hp, float maxHp, float duration = 1f)
    {
        HidePreview();
        StartCoroutine(WaitForHpLose(hp, maxHp, duration));
    }

    private IEnumerator WaitForHpLose(float hp, float maxHp, float duration)
    {
        _hpBar1.DOFillAmount(hp / maxHp, duration);
        var tw = _hpBar2.DOFillAmount(hp / maxHp, duration * 2f);
        yield return tw.WaitForCompletion();
    }

    public void ShowPreview(float hp, float maxHp)
    {
        //TODO 根据克制效果显示不同标志
        _lastBar1FillAmount = _hpBar1.fillAmount;
        _hpBar1.fillAmount = hp / maxHp;
        //TODO 若死亡，则出现死亡标志
    }
    public void HidePreview()
    {
        _hpBar1.fillAmount = _lastBar1FillAmount;
    }
}