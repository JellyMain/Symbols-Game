using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UI.Overview;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class OverviewWindowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text contentTitleText;
        [SerializeField] private OverviewStageContent stageContent;
        [SerializeField] private OverviewInstructionsContent instructionsContent;
        [SerializeField] private OverviewSettingsContent settingsContent;
        [SerializeField] private OverviewStatsContent statsContent;
        [SerializeField] private Button stageButton;
        [SerializeField] private Button instructionsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button statsButton;


        private void OnEnable()
        {
            stageButton.onClick.AddListener(ShowStageContent);
            instructionsButton.onClick.AddListener(ShowInstructionsContent);
            settingsButton.onClick.AddListener(ShowSettingsContent);
            statsButton.onClick.AddListener(ShowStatsContent);
        }


        private void OnDisable()
        {
            stageButton.onClick.RemoveListener(ShowStageContent);
            instructionsButton.onClick.RemoveListener(ShowInstructionsContent);
            settingsButton.onClick.RemoveListener(ShowSettingsContent);
            statsButton.onClick.RemoveListener(ShowStatsContent);
        }
        

        private void Start()
        {
            ShowInstructionsContent();
        }


        private void ShowInstructionsContent()
        {
            contentTitleText.text = "INSTRUCTIONS";
            instructionsContent.gameObject.SetActive(true);
            instructionsContent.Init();
        }


        private void ShowStageContent()
        {
            contentTitleText.text = "STAGE";
            stageContent.gameObject.SetActive(true);
        }


        private void ShowSettingsContent()
        {
            contentTitleText.text = "SETTINGS";
            settingsContent.gameObject.SetActive(true);
        }


        private void ShowStatsContent()
        {
            contentTitleText.text = "STATS";
            statsContent.gameObject.SetActive(true);
        }
    }
}
