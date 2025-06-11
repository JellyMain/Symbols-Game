using System.Collections.Generic;
using System.Net.Http;
using Assets;
using Const;
using Cysharp.Threading.Tasks;
using StaticData.Data;
using StaticData.Models;
using UnityEngine;
using Utils;


namespace StaticData.Services
{
    public class StaticDataService
    {
        private readonly AssetProvider assetProvider;
        public WordResponseArray WordResponseArray { get; private set; }
        public ScoreMultipliersConfig ScoreMultipliersConfig { get; private set; }


        public StaticDataService(AssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
        }


        public async UniTask LoadStaticData()
        {
            UniTask fetchRandomWordsTask = FetchRandomWords();
            UniTask loadScoreMultiplierConfigTask = LoadScoreMultiplierConfig();

            UniTask[] tasks =
            {
                fetchRandomWordsTask,
                loadScoreMultiplierConfigTask
            };

            await UniTask.WhenAll(tasks);
        }


        private async UniTask FetchRandomWords()
        {
            using HttpClient client = new HttpClient();

            string url = "https://random-words-api.kushcreates.com/api?words=50&language=en";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                string wrappedJson = $"{{ \"words\": {json} }}";

                WordResponseArray = wrappedJson.ToDeserialized<WordResponseArray>();
            }
            catch (HttpRequestException e)
            {
                Debug.Log($"Request error: {e.Message}");
            }
        }


        private async UniTask LoadScoreMultiplierConfig()
        {
            ScoreMultipliersConfig =
                await assetProvider.LoadAsset<ScoreMultipliersConfig>(RuntimeConstants.StaticDataAddresses
                    .SCORE_MULTIPLIERS_CONFIG);
        }
    }
}
