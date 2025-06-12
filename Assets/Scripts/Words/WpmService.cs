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


        private void StopTracking()
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

            if (elapsedTimeInMinutes > 0)
            {
                currentWpm = Mathf.RoundToInt(totalCharsTyped / 5f / elapsedTimeInMinutes);
                OnWpmUpdated?.Invoke(currentWpm);
            }
        }
    }
}
