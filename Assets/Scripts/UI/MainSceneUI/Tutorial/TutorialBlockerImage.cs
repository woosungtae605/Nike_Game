using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Tutorial
{
    public class TutorialBlockerImage : Image
    {
        [SerializeField] private RectTransform passThroughTarget;
        [SerializeField] private float padding = 16f;

        private readonly Vector3[] _targetWorldCorners = new Vector3[4];
        private readonly Vector3[] _selfWorldCorners = new Vector3[4];
        private Rect _lastHoleRect;

        public void SetPassThroughTarget(RectTransform target)
        {
            passThroughTarget = target;
            SetVerticesDirty();
        }

        private void LateUpdate()
        {
            if (passThroughTarget == null)
                return;

            Rect holeRect = GetHoleLocalRect();
            if (Approximately(_lastHoleRect, holeRect))
                return;

            _lastHoleRect = holeRect;
            SetVerticesDirty();
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (!base.IsRaycastLocationValid(screenPoint, eventCamera))
                return false;

            if (passThroughTarget == null)
                return true;

            passThroughTarget.GetWorldCorners(_targetWorldCorners);
            Rect rect = new Rect(
                _targetWorldCorners[0].x - padding,
                _targetWorldCorners[0].y - padding,
                _targetWorldCorners[2].x - _targetWorldCorners[0].x + padding * 2f,
                _targetWorldCorners[2].y - _targetWorldCorners[0].y + padding * 2f);

            return !rect.Contains(screenPoint);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Rect fullRect = rectTransform.rect;
            if (passThroughTarget == null)
            {
                AddRect(vh, fullRect);
                return;
            }

            Rect holeRect = GetHoleLocalRect();
            holeRect.xMin = Mathf.Clamp(holeRect.xMin, fullRect.xMin, fullRect.xMax);
            holeRect.xMax = Mathf.Clamp(holeRect.xMax, fullRect.xMin, fullRect.xMax);
            holeRect.yMin = Mathf.Clamp(holeRect.yMin, fullRect.yMin, fullRect.yMax);
            holeRect.yMax = Mathf.Clamp(holeRect.yMax, fullRect.yMin, fullRect.yMax);

            AddRect(vh, new Rect(fullRect.xMin, holeRect.yMax, fullRect.width, fullRect.yMax - holeRect.yMax));
            AddRect(vh, new Rect(fullRect.xMin, fullRect.yMin, fullRect.width, holeRect.yMin - fullRect.yMin));
            AddRect(vh, new Rect(fullRect.xMin, holeRect.yMin, holeRect.xMin - fullRect.xMin, holeRect.height));
            AddRect(vh, new Rect(holeRect.xMax, holeRect.yMin, fullRect.xMax - holeRect.xMax, holeRect.height));
        }

        private Rect GetHoleLocalRect()
        {
            passThroughTarget.GetWorldCorners(_targetWorldCorners);
            rectTransform.GetWorldCorners(_selfWorldCorners);

            Vector2 min = rectTransform.InverseTransformPoint(_targetWorldCorners[0]);
            Vector2 max = rectTransform.InverseTransformPoint(_targetWorldCorners[2]);

            min -= Vector2.one * padding;
            max += Vector2.one * padding;

            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private void AddRect(VertexHelper vh, Rect rect)
        {
            if (rect.width <= 0f || rect.height <= 0f)
                return;

            int startIndex = vh.currentVertCount;
            Color32 vertexColor = color;

            vh.AddVert(new Vector3(rect.xMin, rect.yMin), vertexColor, Vector2.zero);
            vh.AddVert(new Vector3(rect.xMin, rect.yMax), vertexColor, Vector2.up);
            vh.AddVert(new Vector3(rect.xMax, rect.yMax), vertexColor, Vector2.one);
            vh.AddVert(new Vector3(rect.xMax, rect.yMin), vertexColor, Vector2.right);

            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private bool Approximately(Rect a, Rect b)
        {
            return Mathf.Approximately(a.xMin, b.xMin)
                   && Mathf.Approximately(a.xMax, b.xMax)
                   && Mathf.Approximately(a.yMin, b.yMin)
                   && Mathf.Approximately(a.yMax, b.yMax);
        }
    }
}
