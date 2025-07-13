using System;
using TMPro;
using UnityEngine;
using Words;
using Zenject;


namespace UI
{
    public class WpmUI : MonoBehaviour
    {
        [SerializeField] TMP_Text wpmText;
        private WpmService wpmService;


        [Inject]
        private void Construct(WpmService wpmService)
        {
            this.wpmService = wpmService;
        }


        private void Start()
        {
            UpdateWpmUI(0);
        }


        private void OnEnable()
        {
            wpmService.OnWpmUpdated += UpdateWpmUI;
        }


        private void OnDisable()
        {
            wpmService.OnWpmUpdated -= UpdateWpmUI;
        }


        private void UpdateWpmUI(int wpm)
        {
            wpmText.text = wpm.ToString();
        }
    }
}