using UnityEngine;

namespace Descent.Gameplay.Effects.Camera
{
    public class CameraShake : MonoBehaviour
    {
        private float _elapsed = 0.0f;
        private Vector3 _originalLocalPosition = Vector3.zero;
        private float _magnitude = 0.0f;
        private float _duration = 0.0f;
        private bool _isPlaying = false;

        [SerializeField]
        private float _maxFrequency = 0.25f;
        [SerializeField]
        private float _maxDuration = 0.0f;
        [SerializeField]
        private float _maxMagnitude = 0.0f;
        [SerializeField]
        private AnimationCurve _dampingCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        private void Awake()
        {
            _originalLocalPosition = transform.localPosition;
        }

        private void LateUpdate()
        {
            if (!_isPlaying)
            {
                return;
            }

            _elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(_elapsed / _duration);
            float damping = _dampingCurve.Evaluate(progress);

            float shakeX = Mathf.PerlinNoise(0, Time.time * _maxFrequency) * 2.0f - 1.0f;
            float shakeY = Mathf.PerlinNoise(1, Time.time * _maxFrequency) * 2.0f - 1.0f;
            float shakeZ = Mathf.PerlinNoise(2, Time.time * _maxFrequency) * 2.0f - 1.0f;

            Vector3 offset = new Vector3(shakeX, shakeY, shakeZ) * _magnitude * damping;
            transform.localPosition = _originalLocalPosition + offset;

            if (_elapsed >= _duration)
            {
                _isPlaying = false;
                transform.localPosition = _originalLocalPosition;
            }
        }

        public void Shake(float effectStrength)
        {
            _elapsed = 0.0f;
            _magnitude = Mathf.Clamp01(effectStrength / 10.0f) * _maxMagnitude;
            _duration = Mathf.Lerp(0.01f, _maxDuration, Mathf.Clamp01(effectStrength / 10.0f));
            _isPlaying = true;
        }
    }
}