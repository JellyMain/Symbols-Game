using System.Collections;
using System.Collections.Generic;
using Currency;
using Cysharp.Threading.Tasks;
using Factories;
using Shop;
using StaticData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Transform commandOutputContainer;
        [SerializeField] private Transform instructionsContainer;
        [SerializeField] private TMP_Text terminalInputText;
        [SerializeField] private RectTransform cursor;
        [SerializeField] private TMP_Text tokensBalanceText;
        [SerializeField] private Button continueButton;
        private ShopService shopService;
        private GameplayUIFactory gameplayUIFactory;
        private ShopTerminalService shopTerminalService;
        private CurrencyService currencyService;
        private readonly Queue<CommandOutputUI> createdCommandOutputs = new Queue<CommandOutputUI>();


        [Inject]
        private void Construct(ShopService shopService, GameplayUIFactory gameplayUIFactory,
            ShopTerminalService shopTerminalService, CurrencyService currencyService)
        {
            this.gameplayUIFactory = gameplayUIFactory;
            this.shopService = shopService;
            this.shopTerminalService = shopTerminalService;
            this.currencyService = currencyService;
        }


        private void OnEnable()
        {
            continueButton.onClick.AddListener(shopService.StartNextLevel);
            shopTerminalService.OnStringUpdated += UpdateTerminalInputText;
            shopTerminalService.OnCursorPositionUpdated += UpdateCursorPosition;
            shopTerminalService.OnCursorDefaultPositionSet += SetCursorDefaultPosition;
            shopTerminalService.OnCommandExecuted += ShowCommandOutput;
            currencyService.OnTokensAmountChanged += UpdateTokensBalanceText;
            shopTerminalService.OnCommandOutputContainerOverfilled += RemoveFirstCommandOutput;
            shopTerminalService.OnCommandOutputCleared += ClearCommandOutput;
        }


        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(shopService.StartNextLevel);
            shopTerminalService.OnStringUpdated -= UpdateTerminalInputText;
            shopTerminalService.OnCursorPositionUpdated -= UpdateCursorPosition;
            shopTerminalService.OnCursorDefaultPositionSet -= SetCursorDefaultPosition;
            shopTerminalService.OnCommandExecuted -= ShowCommandOutput;
            currencyService.OnTokensAmountChanged -= UpdateTokensBalanceText;
            shopTerminalService.OnCommandOutputContainerOverfilled -= RemoveFirstCommandOutput;
            shopTerminalService.OnCommandOutputCleared -= ClearCommandOutput;
        }


        private void Start()
        {
            CreateInstructionsUI().Forget();
            UpdateTokensBalanceText();
            ShowCommandOutput("> Type \"help\" to get list of the commands");
        }


        private void UpdateTerminalInputText(string currentString)
        {
            terminalInputText.text = currentString;
        }


        private void UpdateCursorPosition(int cursorPositionIndex)
        {
            terminalInputText.ForceMeshUpdate();

            TMP_CharacterInfo charInfo = terminalInputText.textInfo.characterInfo[cursorPositionIndex];

            float charXPosition = charInfo.xAdvance;
            cursor.localPosition = new Vector3(charXPosition, 0, 0);
        }


        private async void ShowCommandOutput(string commandOutput)
        {
            CommandOutputUI commandOutputUI = await gameplayUIFactory.CreateCommandOutputUI(commandOutputContainer);
            commandOutputUI.InitText(commandOutput);
            createdCommandOutputs.Enqueue(commandOutputUI);
        }


        private void ClearCommandOutput()
        {
            while (createdCommandOutputs.Count > 0)
            {
                RemoveFirstCommandOutput();
            }
        }


        private void RemoveFirstCommandOutput()
        {
            CommandOutputUI commandOutputUI = createdCommandOutputs.Dequeue();
            Destroy(commandOutputUI.gameObject);
        }


        private void SetCursorDefaultPosition()
        {
            cursor.anchoredPosition = Vector3.zero;
        }


        private void UpdateTokensBalanceText()
        {
            tokensBalanceText.text = currencyService.Tokens.ToString();
        }


        private async UniTaskVoid CreateInstructionsUI()
        {
            foreach (InstructionData instructionData in shopService.SelectedInstructions)
            {
                ShopInstructionUI shopInstructionUI = await gameplayUIFactory.CreateInstructionUI(instructionsContainer);
                shopInstructionUI.Init(instructionData);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);;
        }
    }
}
