using tuleeeeee.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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

        private BoxCollider2D boxCollider2D;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            roomColliderBounds = boxCollider2D.bounds;
        }

        public void Initialise(GameObject roomGameobject)
        {
            PopulateTilemapMemberVariables(roomGameobject);

            BlockOffUnusedDoorway();

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

                        //groundTilemap = tilemap;   // only 1 groundTilemap
                        groundTilemaps.Add(tilemap);// more than 1 groundTilemap
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
                // only 1 groundTilemap
                /* if (groundTilemap != null)
                 {
                     BlockADorrwayOnTilemapLayer(groundTilemap, doorway);
                 }*/

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

        private void DisableCollisionTilemapRenderer()
        {
            collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }
    }
}