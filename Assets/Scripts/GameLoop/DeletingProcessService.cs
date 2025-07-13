using System;
using Levels;
using TMPro;
using UnityEngine;
using Words;
using Zenject;


namespace GameLoop
{
    public class DeletingProcessService : MonoBehaviour
    {
        private float timeToDelete;
        private float elapsedTime;
        private bool isDeleting;
        public float NormalizedProgress => elapsedTime / timeToDelete;
        private TypedWordValidator typedWordValidator;
        public event Action OnPlayerLost;


        [Inject]
        private void Construct(TypedWordValidator typedWordValidator)
        {
            this.typedWordValidator = typedWordValidator;
        }


        private void OnEnable()
        {
            typedWordValidator.OnFirstCharTyped += StartDeleting;
        }


        private void OnDisable()
        {
            typedWordValidator.OnFirstCharTyped -= StartDeleting;
        }


        public void SetTimeToDelete(float timeToDelete)
        {
            this.timeToDelete = timeToDelete;
        }
       

        private void StartDeleting()
        {
            elapsedTime = 0;
            isDeleting = true;
        }


        private void Update()
        {
            if (isDeleting)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= timeToDelete)
                {
                    elapsedTime = timeToDelete;
                    isDeleting = false;
                    OnPlayerLost?.Invoke();
                }
            }
        }


        public void AddTime(float timeToAdd)
        {
            elapsedTime += timeToAdd;
        }


        public void ResetTimer()
        {
            isDeleting = false;
            elapsedTime = 0;
        }
    }
}
