using System;
using DG.Tweening;
using Score;
using UnityEngine;
using Zenject;


namespace PlayerCamera
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        private ScoreService scoreService;

        
        [Inject]
        private void Construct(ScoreService scoreService)
        {
            this.scoreService = scoreService;    
        }
        
        
        private void Awake()
        {
            cam = Camera.main;
        }

        
        private void Start()
        {
            scoreService.OnTargetScoreReached += RotateToShop;
        }


        private void OnDisable()
        {
            scoreService.OnTargetScoreReached -= RotateToShop;
        }



        public void RotateToGameplay()
        {
            if (cam.transform.rotation.eulerAngles != Vector3.zero)
            {
                cam.transform.DORotate(Vector3.zero, 1f);
                Debug.Log("works");
            }
        }


        public void RotateToShop()
        {
            Vector3 rotationTarget = new Vector3(0, 180, 0);

            if (cam.transform.rotation.eulerAngles != rotationTarget)
            {
                cam.transform.DORotate(rotationTarget, 1f);
            }
        }
    }
}
