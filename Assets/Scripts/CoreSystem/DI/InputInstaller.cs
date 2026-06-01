using Reflex.Core;
using Systems;
using UnityEngine;

namespace CoreSystem.DI
{
    public class InputInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private PlayerInputSO playerInput;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterValue(playerInput);
        }
    }
}