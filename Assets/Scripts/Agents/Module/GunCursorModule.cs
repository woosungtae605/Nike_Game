using Agents.Players;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class GunCursorModule : MonoBehaviour, IModule
    {
        private Player _player;

        private GunCursorImage _gunCursor;
        [SerializeField] private GameObject activeFalseObject;
        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
            Debug.Assert(_player != null, "owner is not player");

            _gunCursor = GetComponentInChildren<GunCursorImage>(true);
            Debug.Assert(_gunCursor != null, "not found GunCursorImage in children");
            _gunCursor.Init();
            _gunCursor.SetActiveFalse();
        }

        public void UnActive()
        {
            _gunCursor.SetActiveFalse();
            if (activeFalseObject != null)
            {
                activeFalseObject.SetActive(false);
            }
        }

        public void Active()
        {
            _gunCursor.gameObject.SetActive(true);
            if(activeFalseObject != null)
            {
                activeFalseObject.SetActive(true);
            }
            _gunCursor.ActiveTrue();
        }
        
        public void PlayScaleMotion()
        {
            _gunCursor.PlayScaleMotion();
        }
    }
}