using TMPro;
using UnityEngine;


namespace UI
{
    public class CommandOutputUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text commandOutputText;


        public void InitText(string text)
        {
            commandOutputText.text = text;
        }
    }
}
