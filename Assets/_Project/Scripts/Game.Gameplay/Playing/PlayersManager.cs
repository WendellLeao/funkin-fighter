using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Gameplay.Playing
{
    public sealed class PlayersManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Player _automaticPlayerPrefab;
        [SerializeField, Range(1, 2)] private int _playersAmount = 2;

        [Header("(DEBUG)")] 
        [SerializeField] private bool _spawnAutomaticPlayer;
        
        private IEventService _eventService;
        private List<Player> _activePlayers;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
            
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

                FlipSecondPlayer(index, player);

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

        private void FlipSecondPlayer(int i, Player newPlayer)
        {
            bool isSecondPlayer = i == 1;

            if (isSecondPlayer)
            {
                Vector3 newScale = new Vector3(-1, 1, 1);
                
                newPlayer.transform.localScale = newScale;
            }
        }
    }
}
