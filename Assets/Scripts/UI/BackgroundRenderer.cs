using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace UI
{
    public class BackgroundRenderer : MonoBehaviour
    {
        [SerializeField] float updateInterval = 0.5f;
        [SerializeField] TMP_Text backgroundText;
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        private readonly char[] randomChars = new char[] { '#', '@', '0', '1' };
        private StringBuilder stringBuilder;
        private float elapsedTime;
        private int charsCount;


        private void Start()
        {
            stringBuilder = new StringBuilder();
            charsCount = randomChars.Length;
            UpdateText();
        }


        private void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= updateInterval)
            {
                elapsedTime = 0;
                UpdateText();
            }
        }
        
        
        private void UpdateText()
        {
            stringBuilder.Clear();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char randomChar = randomChars[Random.Range(0, charsCount)];
                    stringBuilder.Append(randomChar);
                }

                stringBuilder.AppendLine();
            }
            
            backgroundText.text = stringBuilder.ToString();
        }
    }
}
