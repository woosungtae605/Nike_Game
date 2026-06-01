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

        private Player _player;
        
        private HealthModule _health;
        private CoverModule _cover;
        
        [Header("Motion")]
        [SerializeField] private float moveDuration = 0.25f;
        [SerializeField] private Ease moveEase = Ease.OutCubic;

        private Vector3 _healthOriginPos;
        private Vector3 _coverOriginPos;

        private MotionHandle _healthMoveHandle;
        private MotionHandle _coverMoveHandle;

        
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

            coverBar.gameObject.SetActive(false);
            _cover.OnCoverValueChange += HandleCoverValueChange;
        }

        private void OnDestroy()
        {
            _cover.OnCoverValueChange -= HandleCoverValueChange;
        }

        private void HandleCoverValueChange(bool value)
        {
            if (value)
            {
                ShowCoverAndSwap();
            }
            else
            {
                ShowHealthOnly();
            }
        }
        
        private void ShowHealthOnly()
        {
            _healthMoveHandle.TryCancel();
            _coverMoveHandle.TryCancel();

            _healthMoveHandle = LMotion.Create(
                    healthBar.transform.localPosition,
                    _healthOriginPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => healthBar.transform.localPosition = pos);

            _coverMoveHandle = LMotion.Create(
                    coverBar.transform.localPosition,
                    _coverOriginPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => coverBar.transform.localPosition = pos);

            coverBar.gameObject.SetActive(false);
        }

        private void ShowCoverAndSwap()
        {
            _healthMoveHandle.TryCancel();
            _coverMoveHandle.TryCancel();

            coverBar.gameObject.SetActive(true);

            _healthMoveHandle = LMotion.Create(
                    healthBar.transform.localPosition,
                    _coverOriginPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => healthBar.transform.localPosition = pos);

            _coverMoveHandle = LMotion.Create(
                    coverBar.transform.localPosition,
                    _healthOriginPos,
                    moveDuration)
                .WithEase(moveEase)
                .Bind(pos => coverBar.transform.localPosition = pos);
        }
    }
}