using System;
using Agents.Module;
using LitMotion;
using Module;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerBarController : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private GameObjectBar healthBar;
        [SerializeField] private GameObjectBar coverBar;
        [SerializeField] private GameObject healthBackGround;
        [SerializeField] private GameObject coverBackGround;

        private Player _player;
        
        private HealthModule _health;
        private CoverModule _cover;
        
        [Header("Motion")]
        [SerializeField] private float moveDuration = 0.25f;
        [SerializeField] private float backScale = 0.85f;
        [SerializeField] private Ease moveEase = Ease.OutCubic;

        private Vector3 _healthOriginPos;
        private Vector3 _coverOriginPos;
        
        private Vector3 _healthOriginScale;
        private Vector3 _coverOriginScale;

        private MotionHandle _healthMoveHandle;
        private MotionHandle _coverMoveHandle;
        
        private MotionHandle _healthScaleHandle;
        private MotionHandle _coverScaleHandle;

        
        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
        }

        public void AfterInit()
        {
            _health = _player.GetModule<HealthModule>();
            _cover = _player.GetModule<CoverModule>();
            
            _healthOriginPos = healthBar.transform.localPosition;
            _coverOriginPos = coverBar.transform.localPosition;
            _healthOriginScale = healthBar.transform.localScale;
            _coverOriginScale = coverBar.transform.localScale;
            
            _health.OnChanged += healthBar.SetValue;
            _cover.OnChanged += coverBar.SetValue;

            healthBar.SetValue(_health.CurrentValue, _health.MaxValue);
            coverBar.SetValue(_cover.CurrentValue, _cover.MaxValue);
            coverBar.gameObject.SetActive(false);
            _cover.OnCoverValueChange += HandleCoverValueChange;
        }

        private void OnDestroy()
        {
            _healthMoveHandle.TryCancel();
            _coverMoveHandle.TryCancel();
            _healthScaleHandle.TryCancel();
            _coverScaleHandle.TryCancel();
            
            if (_health != null && healthBar != null)
                _health.OnChanged -= healthBar.SetValue;
            
            if (_cover != null)
            {
                if (coverBar != null)
                    _cover.OnChanged -= coverBar.SetValue;

                _cover.OnCoverValueChange -= HandleCoverValueChange;
            }
        }

        private void HandleCoverValueChange(bool value)
        {
            if (value)
            {
                SwapToCover();
            }
            else
            {
                SwapToHealth();
            }
        }
        
        private void SwapToHealth()
        {
            SwapBars(false);
        }

        private void SwapToCover()
        {
            SwapBars(true);
        }
        private void SwapBars(bool coverFront)
        {
            _healthMoveHandle.TryCancel();
            _coverMoveHandle.TryCancel();
            _healthScaleHandle.TryCancel();
            _coverScaleHandle.TryCancel();

            healthBar.gameObject.SetActive(true);
            coverBar.gameObject.SetActive(true);

            healthBackGround.gameObject.SetActive(coverFront);
            coverBackGround.gameObject.SetActive(!coverFront);

            Vector3 healthTargetPos = coverFront ? _coverOriginPos : _healthOriginPos;
            Vector3 coverTargetPos = coverFront ? _healthOriginPos : _coverOriginPos;

            Vector3 healthTargetScale = coverFront ? _healthOriginScale * backScale : _healthOriginScale;
            Vector3 coverTargetScale = coverFront ? _coverOriginScale : _coverOriginScale * backScale;

            _healthMoveHandle = LMotion.Create(
                    healthBar.transform.localPosition,
                    healthTargetPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => healthBar.transform.localPosition = pos);

            _healthScaleHandle = LMotion.Create(
                    healthBar.transform.localScale,
                    healthTargetScale,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(scale => healthBar.transform.localScale = scale);

            _coverMoveHandle = LMotion.Create(
                    coverBar.transform.localPosition,
                    coverTargetPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => coverBar.transform.localPosition = pos);

            _coverScaleHandle = LMotion.Create(
                    coverBar.transform.localScale,
                    coverTargetScale,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(scale => coverBar.transform.localScale = scale);
        }
    }
}