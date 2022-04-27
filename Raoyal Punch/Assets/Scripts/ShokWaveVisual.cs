using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShokWaveVisual : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer ShokWave;
    public Material ShokWaveMat;
    private bool _isCircleGrow = false;

    private float _timer = 0;
    private float _duration = 2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isCircleGrow)
        {
            _timer += Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _duration);
            float alpha = _timer / _duration;
            ShokWave.material.SetFloat("_NearPlane", Mathf.Clamp(alpha, 0 , 0.5f));
            ShokWave.material.SetFloat("_FarPlane", alpha);

            if(_timer >= _duration)
            {
                _timer = 0;
                _isCircleGrow = false;
            }
        }
    }

    public void Launch()
    {
        _isCircleGrow = true;
    }

    internal void CircleHide()
    {
        ShokWave.material.SetFloat("_NearPlane", 0);
        ShokWave.material.SetFloat("_FarPlane", 0);
    }
}
