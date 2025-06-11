using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Tools
{
    public class CustomLayoutGroup : MonoBehaviour
    {
        public enum Type
        {
            Horizontal,
            Vertical
        }

        public enum SpacingType
        {
            BetweenCenters,
            BetweenEdges
        }

        [SerializeField] private bool isAutoUpdate = true;
        [SerializeField] private Type type = Type.Horizontal;
        [SerializeField] private SpacingType spacingType = SpacingType.BetweenEdges;

        [Tooltip("Spacing between elements")]
        [field: SerializeField] public float Spacing { get; private set; } = 10f;

        [Tooltip("Align content to center of layout group")]
        [SerializeField] private bool centerAlignment = true;

        [Tooltip("Whether to account for RectTransform size or Renderer bounds")]
        [SerializeField] private bool hasContentSizeFitter = false;

        private void OnValidate()
        {
            if (isAutoUpdate)
            {
                UpdateLayout();
            }
        }

        
        [Button]
        public void UpdateLayout()
        {
            Transform[] childrenTransforms = GetChildrenTransforms();
            if (childrenTransforms.Length == 0) return;
            
            float[] sizes = new float[childrenTransforms.Length];
            for (int i = 0; i < childrenTransforms.Length; i++)
            {
                sizes[i] = GetElementSize(childrenTransforms[i]);
            }
            
            float totalLength = 0f;

            for (int i = 0; i < sizes.Length; i++)
            {
                totalLength += sizes[i];
            }

            if (childrenTransforms.Length > 1)
            {
                totalLength += Spacing * (childrenTransforms.Length - 1);
            }

            float startPos = centerAlignment ? -totalLength / 2f : 0f;

            float currentPos = startPos;

            for (int i = 0; i < childrenTransforms.Length; i++)
            {
                if (spacingType == SpacingType.BetweenEdges)
                {
                    currentPos += sizes[i] / 2f;
                }

                if (type == Type.Horizontal)
                {
                    childrenTransforms[i].localPosition = new Vector3(currentPos, 0f, 0f);
                }
                else 
                {
                    childrenTransforms[i].localPosition = new Vector3(0f, currentPos, 0f);
                }

                if (spacingType == SpacingType.BetweenEdges)
                {
                    currentPos += sizes[i] / 2f + Spacing;
                }
                else 
                {
                    currentPos += Spacing;
                }
            }
        }

        private float GetElementSize(Transform element)
        {
            if (hasContentSizeFitter)
            {
                TMP_Text text = element.GetComponent<TMP_Text>();
                if (text != null)
                {
                    return type == Type.Horizontal ? 
                        text.preferredWidth : 
                        text.preferredHeight;
                }
            }

            RectTransform rectTransform = element.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                return type == Type.Horizontal ? 
                    rectTransform.sizeDelta.x : 
                    rectTransform.sizeDelta.y;
            }

            return 1f;
        }

        private Transform[] GetChildrenTransforms()
        {
            Transform[] childrenTransforms = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                childrenTransforms[i] = transform.GetChild(i);
            }

            return childrenTransforms;
        }
    }
}
