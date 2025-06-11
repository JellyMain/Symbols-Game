using DG.Tweening;
using UnityEngine;


namespace PlayerCamera
{
    public class CameraController : MonoBehaviour
    {
        private void Start()
        {
            Camera camera = Camera.main;

            camera.DOShakeRotation(5, new Vector3(0.2f, 0.2f, 0), randomness: 45,
                randomnessMode: ShakeRandomnessMode.Harmonic);
        }
    }
}
