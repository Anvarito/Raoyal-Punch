using System;
using UnityEngine;
using UnityEngine.UI;

public class HitPointBar : MonoBehaviour
{
    [SerializeField] private Slider HP_bar;
    [SerializeField] private Text HP_text;

    void Update()
    {
        var pointInScreen = Camera.main.WorldToScreenPoint(transform.position);
        HP_bar.transform.position = pointInScreen;
    }

    public void ResetValue(string maxHP)
    {
        HP_text.text = maxHP;
        HP_bar.value = 1;
    }

    internal void ChangeValue(string hp, float alpha)
    {
        HP_text.text = hp;
        HP_bar.value = alpha;
    }
}