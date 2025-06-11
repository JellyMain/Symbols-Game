using System;


namespace StaticData.Models
{
    [Serializable]
    public class WordResponse
    {
        public string word;
        public int length;
        public string category;
        public string language;
    }
}