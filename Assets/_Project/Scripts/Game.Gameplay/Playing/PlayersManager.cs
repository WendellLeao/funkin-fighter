using System.Collections.Generic;
using Game.Services;
using UnityEngine;
using Game.Events;
using Game.Input;

namespace Game.Gameplay.Playing
{
    public sealed class PlayersManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Player _automaticPlayerPrefab;

        [Header("(DEBUG)")] 
        [SerializeField] private bool _spawnAutomaticPlayer;
        
        private IEventService _eventService;
        private IInputService _inputService;
        private List<Player> _activePlayers;
        private int _playersAmount = 2;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
            
            _inputService = ServiceLocator.GetService<IInputService>();

            _activePlayers = new List<Player>();
            
            SpawnPlayers();
        }

        public void Dispose()
        {
            foreach (Player activePlayer in _activePlayers)
            {
                activePlayer.Stop();
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (Player activePlayer in _activePlayers)
            {
                activePlayer.Tick(deltaTime);
            }
        }

        private void SpawnPlayers()
        {
            for (int index = 0; index < _playersAmount; index++)
            {
                Player playerBasePrefab = GetPlayerPrefab(index);
                
                Player player = Instantiate(playerBasePrefab, transform);

                player.transform.position = _spawnPoints[index].position;

                FixPlayerScale(index, player);

                player.Begin(_eventService, index);
                
                _activePlayers.Add(player);
                
                _eventService.DispatchEvent(new PlayerCreatedEvent(player));
            }
        }

        private Player GetPlayerPrefab(int index)
        {
            if (_spawnAutomaticPlayer && index > 0)
            {
                return _automaticPlayerPrefab;
            }

            return _playerPrefab;
        }

        private void FixPlayerScale(int i, Player newPlayer)
        {
            bool isSecondPlayer = i == 1;

            if (!isSecondPlayer)
            {
                return;
            }
            
            newPlayer.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
