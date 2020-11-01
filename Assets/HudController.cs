﻿using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private GameObject _dmgPanel;
    [SerializeField] private float _blinkDuration;
    private void Start()
    {
        AppModel.Player().OnDamageTake += ScreenBlink;
    }

    private void ScreenBlink()
    {
        StartCoroutine(DoBlink());
    }

    private IEnumerator DoBlink()
    {
        _dmgPanel.SetActive(true);
        yield return new WaitForSeconds(_blinkDuration);
        _dmgPanel.SetActive(false);
    }
}