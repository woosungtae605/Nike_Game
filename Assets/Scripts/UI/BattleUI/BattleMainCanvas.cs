using System.Collections.Generic;
using UnityEngine;

namespace UI.BattleUI
{
    public class BattleMainCanvas : MonoBehaviour
    {
        private readonly List<IUIElement> _uiElements = new();

        private void Awake()
        {
            CacheUIElements();
        }

        [ContextMenu("Hide")]
        public void HideAll()
        {
            foreach (IUIElement uiElement in _uiElements)
            {
                uiElement?.Hide();
            }
        }

        private void CacheUIElements()
        {
            _uiElements.Clear();
            
            MonoBehaviour[] children = GetComponentsInChildren<MonoBehaviour>(true);
            AddUIElements(children);
        }

        private void AddUIElements(IEnumerable<MonoBehaviour> behaviours)
        {
            if (behaviours == null)
                return;

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null || behaviour == this)
                    continue;

                if (behaviour is not IUIElement uiElement || _uiElements.Contains(uiElement))
                    continue;

                _uiElements.Add(uiElement);
            }
        }
    }
}