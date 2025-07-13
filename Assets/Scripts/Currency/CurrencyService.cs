using System;


namespace Currency
{
    public class CurrencyService
    {
        public int Tokens { get; private set; }
        public event Action OnTokensAmountChanged;


        public void AddTokens(int tokensToAdd)
        {
            Tokens += tokensToAdd;
            OnTokensAmountChanged?.Invoke();
        }


        public bool TrySpentTokens(int tokensToSpent)
        {
            if (Tokens >= tokensToSpent)
            {
                Tokens -= tokensToSpent;
                OnTokensAmountChanged?.Invoke();
                return true;
            }

            return false;
        }
    }
}
