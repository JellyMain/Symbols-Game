using StaticData.Models;
using StaticData.Services;
using UnityEngine;


namespace Words
{
    public class WordsProvider
    {
        private readonly WordResponseArray wordResponseArray;
        
        
        public WordsProvider(StaticDataService staticDataService)
        {
            wordResponseArray = staticDataService.WordResponseArray;
        }
        

        public string GetRandomWord()
        {
            int randomIndex = Random.Range(0, wordResponseArray.words.Length);
            return wordResponseArray.words[randomIndex].word;
        }
    }
}
