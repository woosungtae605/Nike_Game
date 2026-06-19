using System.Collections.Generic;
using Agents.Enemies;
using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using Systems.UpgradeSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Agents.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerSquadSO playerSquad;
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform playerParent;

        private readonly List<Player> playerList = new();
        private readonly List<Player> _spawnedPlayers = new();
        public IReadOnlyList<Player> Players => playerList;
        public Player CurrentPlayer { get; private set; }

        private EnemyRegisterSo _enemyRegisterSo;
        private bool _battleFailed;
        private static readonly Key[] ChangePlayerKeys =
        {
            Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5
        };

        private void Awake()
        {
            SpawnSquadPlayers();
            Debug.Assert(playerList != null && playerList.Count > 0, "Player list is empty");
            Bus<PlayerDeathEvent>.OnEvent += HandlePlayerDeath;
        }

        private void OnDestroy()
        {
            Bus<PlayerDeathEvent>.OnEvent -= HandlePlayerDeath;
        }

        private void Start()
        {
            AllPlayerDummy();
        }

        public void AllPlayerDummy()
        {
            foreach (Player player in playerList)
            {
                if (player == null)
                    continue;

                player.ChangeState(PlayerStates.Dummy);
            }
        }

        public void Init(EnemyRegisterSo enemyRegisterSo)
        {
            _enemyRegisterSo = enemyRegisterSo;
            _battleFailed = false;

            if (playerList.Count <= 0)
                SpawnSquadPlayers();

            foreach (Player player in playerList)
            {
                if (player == null)
                    continue;

                player.ReviveForBattle();
                player.SetEnemyRegister(_enemyRegisterSo);
                player.PlayerNotControl();
            }

            ChangeFirstPlayer();
        }

        private void SpawnSquadPlayers()
        {
            ClearSpawnedPlayers();

            if (playerSquad == null)
            {
                Debug.LogError("PlayerSquadSO is null", this);
                return;
            }

            PlayerDataSO[] playerDatas = playerSquad.PlayerDataSos;
            if (playerDatas == null)
                return;

            for (int i = 0; i < playerDatas.Length; i++)
            {
                PlayerDataSO playerData = playerDatas[i];
                if (playerData == null || playerData.player == null)
                    continue;

                Player player = SpawnPlayer(playerData.player, i);
                if (player == null)
                    continue;

                ApplyUpgradeStats(player, playerData);
                playerList.Add(player);
                _spawnedPlayers.Add(player);
            }
        }

        private void ApplyUpgradeStats(Player player, PlayerDataSO playerData)
        {
            if (player == null || playerData == null || upgradeManager == null)
                return;

            player.SetBattleStats(upgradeManager.GetAttack(playerData), upgradeManager.GetMaxHp(playerData));
            Debug.Log($"[Player] Apply upgrade stats {playerData.NikkeName} LV:{upgradeManager.GetLevel(playerData) + 1} ATK:{player.AttackDamage} HP:{player.MaxHp}", player);
        }

        private Player SpawnPlayer(Player playerPrefab, int index)
        {
            Transform spawnPoint = GetSpawnPoint(index);
            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
            Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;
            Transform parent = playerParent != null ? playerParent : transform;

            return Instantiate(playerPrefab, position, rotation, parent);
        }

        private Transform GetSpawnPoint(int index)
        {
            if (spawnPoints == null || index < 0 || index >= spawnPoints.Length)
                return null;

            return spawnPoints[index];
        }

        private void ClearSpawnedPlayers()
        {
            CurrentPlayer = null;
            playerList.Clear();

            foreach (Player player in _spawnedPlayers)
            {
                if (player == null)
                    continue;

                Destroy(player.gameObject);
            }

            _spawnedPlayers.Clear();
        }

        public Player GetClosestPlayer(Vector3 origin)
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in playerList)
            {
                if (!IsAlivePlayer(player))
                    continue;

                float distance = (player.transform.position - origin).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }

        public Player GetRandomPlayer()
        {
            List<Player> activePlayers = new List<Player>();

            foreach (Player player in playerList)
            {
                if (!IsAlivePlayer(player))
                    continue;

                activePlayers.Add(player);
            }

            if (activePlayers.Count == 0)
                return null;

            int randomIndex = Random.Range(0, activePlayers.Count);
            return activePlayers[randomIndex];
        }

        private void Update()
        {
            if (_battleFailed || Keyboard.current == null)
                return;

            for (int i = 0; i < ChangePlayerKeys.Length && i < playerList.Count; i++)
            {
                if (Keyboard.current[ChangePlayerKeys[i]].wasPressedThisFrame)
                {
                    ChangePlayer(i);
                    return;
                }
            }
        }

        private void ChangeFirstPlayer()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (IsAlivePlayer(playerList[i]))
                {
                    ChangePlayer(i);
                    return;
                }
            }

            CurrentPlayer = null;
        }

        private void ChangePlayer(int index)
        {
            if (index < 0 || index >= playerList.Count)
            {
                Debug.LogError($"Player index {index} is out of range");
                return;
            }
            if (!IsAlivePlayer(playerList[index]))
                return;

            if (CurrentPlayer != null)
                CurrentPlayer.PlayerNotControl();
            
            CurrentPlayer = playerList[index];
            CurrentPlayer.PlayerControl();
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(playerList[index].CameraTransform, 0.2f));
        }

        private void HandlePlayerDeath(PlayerDeathEvent deathEvent)
        {
            if (deathEvent.Player == CurrentPlayer)
            {
                CurrentPlayer = null;
                ChangeFirstPlayer();
            }

            if (_battleFailed || HasAlivePlayer())
                return;

            _battleFailed = true;
            CurrentPlayer = null;
            AllPlayerDummy();
            Bus<BattleFailEvent>.Raise(new BattleFailEvent());
        }

        private bool HasAlivePlayer()
        {
            foreach (Player player in playerList)
            {
                if (IsAlivePlayer(player))
                    return true;
            }

            return false;
        }

        private bool IsAlivePlayer(Player player)
        {
            return player != null && player.gameObject.activeInHierarchy && !player.IsDead;
        }
    }
}