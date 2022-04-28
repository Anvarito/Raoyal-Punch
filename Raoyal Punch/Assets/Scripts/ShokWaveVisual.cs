using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShokWaveVisual : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer ShokWave;
    public ParticleSystem SmokeParticles;
    public Material ShokWaveMat;
    private bool _isCircleGrow = false;

    private float _timer = 0;
    private float _duration = 2;
    void Start()
    {
        ShokWave.material.SetFloat("_NearPlane", 0);
        ShokWave.material.SetFloat("_FarPlane", 0);
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

    public void ResetAll()
    {
        ShokWave.material.SetFloat("_NearPlane", 0);
        ShokWave.material.SetFloat("_FarPlane", 0);
        _isCircleGrow = false;
        _timer = 0;
        SmokeParticles.Stop();
    }
    internal void CircleHide()
    {
        ShokWave.material.SetFloat("_NearPlane", 0);
        ShokWave.material.SetFloat("_FarPlane", 0);
        SmokeParticles.Play();
    }
}
