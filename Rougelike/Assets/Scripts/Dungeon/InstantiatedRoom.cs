using tuleeeeee.Enums;
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
using tuleeeeee.StaticEvent;


namespace tuleeeeee.Dungeon
{
    [RequireComponent(typeof(BoxCollider2D))]
    [DisallowMultipleComponent]
    public class InstantiatedRoom : MonoBehaviour
    {
        [HideInInspector] public Room room;
        [HideInInspector] public Grid grid;
        [HideInInspector] public List<Tilemap> groundTilemaps = new List<Tilemap>();
        [HideInInspector] public Tilemap decoration1Tilemap;
        [HideInInspector] public Tilemap decoration2Tilemap;
        [HideInInspector] public Tilemap frontTilemap;
        [HideInInspector] public Tilemap collisionTilemap;
        [HideInInspector] public Tilemap minimapTilemap;
        [HideInInspector] public Bounds roomColliderBounds;
        [HideInInspector] public int[,] aStarMovementPenalty;
        [HideInInspector] public int[,] aStarItemObstacles;

        #region Header OBJECT REFERENCES
        [Space(10)]
        [Header("OBJECT REFERENCES")]
        #endregion
        private GameObject environmentGameObject;

        private BoxCollider2D boxCollider2D;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            environmentGameObject = transform.Find(Settings.enviroment).gameObject;
            roomColliderBounds = boxCollider2D.bounds;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Settings.playerTag && room != GameManager.Instance.GetCurrentRoom())
            {

                this.room.isPreviouslyVisisted = true;

                StaticEventHandler.CallRoomChangedEvent(room);
            }
        }
        public void Initialise(GameObject roomGameobject)
        {
            PopulateTilemapMemberVariables(roomGameobject);

            BlockOffUnusedDoorway();

            AddObstaclesAndPreferredPaths();

            CreateItemObstaclesArray();

            AddDoorToRooms();

            DisableCollisionTilemapRenderer();
        }

        private void PopulateTilemapMemberVariables(GameObject roomGameobject)
        {
            grid = roomGameobject.GetComponentInChildren<Grid>();
            Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                switch (tilemap.gameObject.tag)
                {
                    case "groundTilemap":
                        groundTilemaps.Add(tilemap);
                        break;

                    case "decoration1Tilemap":
                        decoration1Tilemap = tilemap;
                        break;

                    case "decoration2Tilemap":
                        decoration2Tilemap = tilemap;
                        break;

                    case "frontTilemap":
                        frontTilemap = tilemap;
                        break;

                    case "collisionTilemap":
                        collisionTilemap = tilemap;
                        break;

                    case "minimapTilemap":
                        minimapTilemap = tilemap;
                        break;
                }

            }
        }

        private void BlockOffUnusedDoorway()
        {
            foreach (Doorway doorway in room.doorWayList)
            {
                if (doorway.isConnected)
                {
                    continue;
                }
                foreach (Tilemap groundTilemap in groundTilemaps)// more than 1 groundTilemap
                {
                    if (groundTilemap != null)
                        BlockADorrwayOnTilemapLayer(groundTilemap, doorway);
                }
                if (decoration1Tilemap != null)
                {
                    BlockADorrwayOnTilemapLayer(decoration1Tilemap, doorway);
                }
                if (decoration2Tilemap != null)
                {
                    BlockADorrwayOnTilemapLayer(decoration2Tilemap, doorway);
                }
                if (frontTilemap != null)
                {
                    BlockADorrwayOnTilemapLayer(frontTilemap, doorway);
                }
                if (collisionTilemap != null)
                {
                    BlockADorrwayOnTilemapLayer(collisionTilemap, doorway);
                }
                if (minimapTilemap != null)
                {
                    BlockADorrwayOnTilemapLayer(minimapTilemap, doorway);
                }

            }
        }

        private void BlockADorrwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
        {
            switch (doorway.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    BlockDoorwayHorizontally(tilemap, doorway);
                    break;
                case Orientation.east:
                case Orientation.west:
                    BlockDoorwayVertically(tilemap, doorway);
                    break;
                case Orientation.none:
                    break;
                default:
                    break;
            }
        }

        private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
        {
            Vector2Int startPosition = doorway.doorwayStartCopyPosition;

            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
                {
                    Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                    tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0),
                        tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                    tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
                }
            }
        }

        private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
        {
            Vector2Int startPosition = doorway.doorwayStartCopyPosition;

            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
                {
                    Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                    tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0),
                        tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                    tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
                }
            }
        }

        private void AddObstaclesAndPreferredPaths()
        {
            aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1,
                room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

            for (int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
            {
                for (int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
                {
                    aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;

                    TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                    foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                    {
                        if (tile == collisionTile)
                        {
                            aStarMovementPenalty[x, y] = 0;
                            break;
                        }
                    }

                    if (tile == GameResources.Instance.preferredEnemyPathTile)
                    {
                        aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
                    }
                }
            }
        }

        private void AddDoorToRooms()
        {
            if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS) return;

            foreach (Doorway doorway in room.doorWayList)
            {
                if (doorway.doorPrefab != null && doorway.isConnected)
                {
                    float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;

                    GameObject door = null;
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);

                    if (doorway.orientation == Orientation.north)
                    {
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                    }
                    else if (doorway.orientation == Orientation.south)
                    {
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y, 0f);
                    }
                    else if (doorway.orientation == Orientation.east)
                    {
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.25f, 0f);
                    }
                    else if (doorway.orientation == Orientation.west)
                    {
                        door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance * 1.25f, 0f);
                    }

                    Door doorComponent = door.GetComponent<Door>();
                    if (room.roomNodeType.isBossRoom)
                    {
                        doorComponent.isBossRoomDoor = true;

                        doorComponent.LockDoor();

                        GameObject skullIcon = Instantiate(GameResources.Instance.minimapSkullPrefab, gameObject.transform);
                        skullIcon.transform.localPosition = door.transform.localPosition;
                    }
                }
            }
        }

        private void DisableRoomCollider()
        {
            boxCollider2D.enabled = false;
        }

        public void EnableRoomCollider()
        {
            boxCollider2D.enabled = true;
        }

        public void ActivateEnvironmentGameObjects()
        {
            if (environmentGameObject != null)
            {
                environmentGameObject.SetActive(true);
            }
        }

        public void DeactivateEnvironmentGameObject()
        {
            if (environmentGameObject != null)
            {
                environmentGameObject.SetActive(false);
            }
        }

        private void DisableCollisionTilemapRenderer()
        {
            collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }

        public void LockDoors()
        {
            Door[] doorArray = GetComponentsInChildren<Door>();

            foreach (Door door in doorArray)
            {
                door.LockDoor();
            }

            DisableRoomCollider();
        }

        public void UnlockDoors(float doorUnlockDelay)
        {
            StartCoroutine(UnlockDoorsRoutine(doorUnlockDelay));
        }

        private IEnumerator UnlockDoorsRoutine(float doorUnlockDelay)
        {
            if (doorUnlockDelay > 0f)
            {
                yield return new WaitForSeconds(doorUnlockDelay);
            }

            Door[] doorArray = GetComponentsInChildren<Door>();

            foreach (Door door in doorArray)
            {
                door.UnlockDoor();
            }

            EnableRoomCollider();
        }

        private void CreateItemObstaclesArray()
        {
            aStarItemObstacles = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1,
                room.templateUpperBounds.y - room.templateLowerBounds.y + 1];
        }

        private void InitializeItemObstaclesArray()
        {
            for (int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
            {
                for (int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
                {
                    aStarItemObstacles[x, y] = Settings.defaultAStarMovementPenalty;
                }
            }
        }

       /* public void UpdateMoveableObstacles()
        {
            InitializeItemObstaclesArray();

            foreach (MoveItem moveItem in moveableItemsList)
            {
                Vector3Int colliderBoundsMin = grid.WorldToCell(moveItem.boxCollider2D.bounds.min);
                Vector3Int colliderBoundsMax = grid.WorldToCell(moveItem.boxCollider2D.bounds.max);

                for (int i = colliderBoundsMin.x; i <= colliderBoundsMax.x; i++)
                {
                    for (int j = colliderBoundsMin.y; j <= colliderBoundsMax.y; j++)
                    {
                        aStarItemObstacles[i - room.templateLowerBounds.x, j - room.templateLowerBounds.y] = 0;
                    }
                }
            }
        }*/
    }
}