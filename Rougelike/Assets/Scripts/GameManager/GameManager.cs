using System.Collections.Generic;
using UnityEngine;
using tuleeeeee.Enums;
using tuleeeeee.Dungeon;
using tuleeeeee.Misc;
using tuleeeeee.Utilities;
using tuleeeeee.Data;
using tuleeeeee.StaticEvent;

namespace tuleeeeee.Managers
{
    [DisallowMultipleComponent]
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        #region Header DUNGEON LEVELS

        [Space(10)]
        [Header("DUNGEON LEVELS")]

        #endregion
        #region  Tooltip
        [Tooltip("Populate with the dungeon level sciptable objects")]
        #endregion Tooltip
        [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

        #region Tooltip
        [Tooltip("Populate with starting the dungeon level for testing, first level =0")]
        #endregion Tooltip
        [SerializeField] private int currentDungeonLevelListIndex;

        private Room currentRoom;
        private Room previousRoom;

        private PlayerDetailsSO playerDetails;
        private Player player;


        [HideInInspector] public GameState gameState;
        protected override void Awake()
        {
            base.Awake();

            playerDetails = GameResources.Instance.currentPlayerSO.playerDetails;

            InstantiatePlayer();
        }

        private void InstantiatePlayer()
        {
            GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

            player = playerGameObject.GetComponent<Player>();

            player.Initialize(playerDetails);
        }

        private void Start()
        {
            gameState = GameState.gameStarted;
        }

        private void Update()
        {
            HandleGameState();

            if (Input.GetKeyDown(KeyCode.R))
            {
                gameState = GameState.gameStarted;
            }
        }

        private void HandleGameState()
        {
            switch (gameState)
            {
                case GameState.gameStarted:
                    PlayDungeonLevel(currentDungeonLevelListIndex);
                    gameState = GameState.playingLevel;
                    break;
            }
        }

        private void PlayDungeonLevel(int dungeonLeveListIndex)
        {
            bool dungeonBuiltSuccessful = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLeveListIndex]);

            if (!dungeonBuiltSuccessful)
            {
                Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
            }

            StaticEventHandler.CallRoomChangedEvent(currentRoom);

            Vector3 playerPosition = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f,
                (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

            player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(playerPosition);
        }

        public void SetCurrentRoom(Room room)
        {
            previousRoom = currentRoom;
            currentRoom = room;
        }

        public Room GetCurrentRoom()
        {
            return currentRoom;
        }

        public Player GetPlayer()
        {
            return player;
        }
        public Sprite GetPlayerMiniMapIcon()
        {
            return playerDetails.playerMiniMapIcon;
        }

        public DungeonLevelSO GetCurrentDungeonLevel()
        {
            return dungeonLevelList[currentDungeonLevelListIndex];
        }

        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
        }
#endif
        #endregion
    }
}