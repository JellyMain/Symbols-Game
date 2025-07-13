using System;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;


namespace UI
{
    public class TransitionScreen : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 1;
        [SerializeField] private float fadeOutDuration = 1;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private CanvasGroup canvasGroup;


        private void OnEnable()
        {
            videoPlayer.loopPointReached += StartFadingOut;
        }


        private void OnDisable()
        {
            videoPlayer.loopPointReached -= StartFadingOut;
        }


        private void StartFadingOut(VideoPlayer sources)
        {
            FadeOut().Forget();
        }


        public async UniTask FadeIn()
        {
            videoPlayer.Play();
            float elapsedTime = 0;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;

            while (elapsedTime < fadeInDuration)
            {
                float t = elapsedTime / fadeInDuration;

                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();
            }

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }




        private async UniTaskVoid FadeOut()
        {
            float elapsedTime = 0;

            while (elapsedTime < fadeOutDuration)
            {
                float t = elapsedTime / fadeOutDuration;

                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();
            }

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;

            Destroy(gameObject);
        }
    }
}
