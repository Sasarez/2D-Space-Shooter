using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private float _shakeDuration;
    [SerializeField]
    private float _shakeAmount = .2f;
    [SerializeField]
    private float _shakeFactor = .7f;

    private Vector3 _startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (_camera == null)
        {
            _camera = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable()
    {
        _startingPosition = _camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeDuration > 0)
        {
            _camera.localPosition = _startingPosition + Random.insideUnitSphere * _shakeAmount;

            _shakeDuration -= Time.deltaTime * _shakeFactor;
        }
        else
        {
            _shakeDuration = 0f;
            _camera.localPosition = _startingPosition;
        }
    }

    public void ShakeScreen(float duration, float amount, float factor)
    {
        _shakeDuration = duration;
        _shakeAmount = amount;
        _shakeFactor = factor;
    }
}
