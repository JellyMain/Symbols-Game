using System;
using UnityEngine;
using Zenject;


namespace Words
{
    public class WpmService : MonoBehaviour
    {
        private int currentWpm;
        private int totalCharsTyped;
        private float startTime;
        private bool isTracking;
        public event Action<int> OnWpmUpdated;
        private TypedWordValidator typedWordValidator;
        public int MaxWpm { get; private set; }


        [Inject]
        private void Construct(TypedWordValidator typedWordValidator)
        {
            this.typedWordValidator = typedWordValidator;
        }


        private void Start()
        {
            typedWordValidator.OnFirstCharTyped += StartTracking;
            typedWordValidator.OnCharTyped += RegisterCharTyped;
        }


        private void OnDisable()
        {
            typedWordValidator.OnFirstCharTyped -= StartTracking;
            typedWordValidator.OnCharTyped -= RegisterCharTyped;
        }


        private void StartTracking()
        {
            startTime = Time.time;
            totalCharsTyped = 0;
            isTracking = true;
        }


        private void Update()
        {
            if (isTracking)
            {
                UpdateWpm();
            }
        }


        public void StopTracking()
        {
            isTracking = false;
        }


        private void RegisterCharTyped()
        {
            if (!isTracking) return;

            totalCharsTyped++;
        }

        
        private void UpdateWpm()
        {
            float elapsedTimeInMinutes = (Time.time - startTime) / 60f;

            const float MIN_TIME_THRESHOLD = 0.016f;
            const int MAX_REALISTIC_WPM = 350;

            if (elapsedTimeInMinutes >= MIN_TIME_THRESHOLD)
            {
                currentWpm = Mathf.RoundToInt(totalCharsTyped / 5f / elapsedTimeInMinutes);

                currentWpm = Mathf.Min(currentWpm, MAX_REALISTIC_WPM);

                if (currentWpm > MaxWpm)
                {
                    MaxWpm = currentWpm;
                }
                
                OnWpmUpdated?.Invoke(currentWpm);
            }
            else
            {
                currentWpm = 0;
                OnWpmUpdated?.Invoke(currentWpm);
            }
        }
    }
}
