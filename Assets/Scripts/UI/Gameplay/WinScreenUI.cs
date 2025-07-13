using System;
using Currency;
using Infrastructure.GameStates;
using Infrastructure.Services;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace UI
{
    public class WinScreenUI : MonoBehaviour
    {
        [SerializeField] private Button collectButton;
        [SerializeField] private TMP_Text memoryAllocatedNumberText;
        [SerializeField] private TMP_Text wordsTypedNumberText;
        [SerializeField] private TMP_Text correctWordsNumberText;
        [SerializeField] private TMP_Text maxCorrectWordsStreakNumberText;
        [SerializeField] private TMP_Text maxWpmNumberText;
        [SerializeField] private TMP_Text memoryAllocatedRewardNumberText;
        [SerializeField] private TMP_Text wordsTypedRewardNumberText;
        [SerializeField] private TMP_Text correctWordsRewardNumberText;
        [SerializeField] private TMP_Text maxCorrectWordsStreakRewardNumberText;
        [SerializeField] private TMP_Text maxWpmRewardNumberText;
        private CurrencyService currencyService;
        private GameStateMachine gameStateMachine;
        private int totalReward;

        
        [Inject]
        private void Construct(CurrencyService currencyService, GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
            this.currencyService = currencyService;
        }
        

        private void OnEnable()
        {
            collectButton.onClick.AddListener(CollectTokens);
        }

        
        private void OnDisable()
        {
            collectButton.onClick.RemoveListener(CollectTokens);
        }


        public void SetLevelStats(LevelStats levelStats)
        {
            memoryAllocatedNumberText.text = levelStats.memoryAllocated.ToString();
            wordsTypedNumberText.text = levelStats.wordsTyped.ToString();
            correctWordsNumberText.text = levelStats.correctWords.ToString();
            maxCorrectWordsStreakNumberText.text = levelStats.maxCorrectWordsStreak.ToString();
            maxWpmNumberText.text = levelStats.maxWpm.ToString();
        }
        
        
        public void SetLevelRewards(LevelRewards levelRewards)
        {
            memoryAllocatedRewardNumberText.text = levelRewards.memoryAllocatedReward.ToString();
            wordsTypedRewardNumberText.text = levelRewards.wordsTypedReward.ToString();
            correctWordsRewardNumberText.text = levelRewards.correctWordsReward.ToString();
            maxCorrectWordsStreakRewardNumberText.text = levelRewards.maxCorrectWordsStreakReward.ToString();
            maxWpmRewardNumberText.text = levelRewards.maxWpmReward.ToString();
            totalReward = levelRewards.totalReward;
        }


        private void CollectTokens()
        {
            currencyService.AddTokens(totalReward);
            gameStateMachine.Enter<ShopState>();
            Destroy(gameObject);
        }
    }
}



