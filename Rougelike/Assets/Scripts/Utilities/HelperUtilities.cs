using System.Collections;
using UnityEditor;
using UnityEngine;
using tuleeeeee.Enums;
using UnityEngine.InputSystem;
using tuleeeeee.Dungeon;
using tuleeeeee.Managers;

namespace tuleeeeee.Utilities
{
    /// <summary>
    /// empty string check
    /// </summary>
    public static class HelperUtilities
    {
        public static Camera mainCamera;

        public static Vector3 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }

        /// <summary>
        /// get mouse position
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            if (mainCamera == null) mainCamera = Camera.main;

            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

            // Clamp mouse position to screen size
            mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
            mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            worldPosition.z = 0f;

            return worldPosition;
        }

        /// <summary>
        /// Get Angle From Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float GetAngleFromVector(Vector3 vector)
        {
            float radians = Mathf.Atan2(vector.y, vector.x);

            float degrees = radians * Mathf.Rad2Deg;

            return degrees;
        }

        /// <summary>
        /// Get direction vector from angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 GetDirectionVectorFromAngle(float angle)
        {
            Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
            return directionVector;
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static Direction GetDirection(float angleDegrees)
        {
            Direction direction;

            if (angleDegrees > 22f && angleDegrees <= 67f)
            {
                direction = Direction.upright;
            }
            else if (angleDegrees > 67f && angleDegrees <= 112f)
            {
                direction = Direction.up;
            }
            else if (angleDegrees > 112f && angleDegrees <= 158f)
            {
                direction = Direction.upleft;
            }
            else if ((angleDegrees > 158f && angleDegrees <= 180f) || (angleDegrees > -180f && angleDegrees <= -135f))
            {
                direction = Direction.left;
            }
            else if (angleDegrees > -135f && angleDegrees <= -45f)
            {
                direction = Direction.down;
            }
            else if ((angleDegrees > -45f && angleDegrees <= 0f) && (angleDegrees > 0f && angleDegrees <= 22f))
            {
                direction = Direction.right;
            }
            else
            {
                direction = Direction.right;
            }

            return direction;
        }
        /// <summary>
        /// Empty string debug check
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
        {
            if (stringToCheck == "")
            {
                Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
                return true;
            }
            return false;
        }
        /// <summary>
        /// Null value debug check
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="objectToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
        {
            if (objectToCheck == null)
            {
                Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// List empty or contains null value check - returns true if there is an error
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="enumerableObjectToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
        {
            bool error = false;
            int count = 0;

            if (enumerableObjectToCheck == null)
            {
                Debug.Log(fieldName + "is null in object" + thisObject.name.ToString());
                return true;
            }

            foreach (var item in enumerableObjectToCheck)
            {
                if (item == null)
                {
                    Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                    error = true;
                }
                else
                {
                    count++;
                }
            }

            if (count == 0)
            {
                Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
                error = true;
            }

            return error;
        }
        /// <summary>
        /// positive value debug check
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="valueToCheck"></param>
        /// <param name="isZeroAllowed"></param>
        /// <returns></returns>
        public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
        {
            bool error = false;
            if (isZeroAllowed)
            {
                if (valueToCheck < 0)
                {
                    Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                    error = true;
                }
            }
            else
            {
                if (valueToCheck <= 0)
                {
                    Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                    error = true;
                }
            }
            return error;
        }
        public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
        {
            Room currentRoom = GameManager.Instance.GetCurrentRoom();

            Grid grid = currentRoom.instantiatedRoom.grid;

            Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

            foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
            {
                Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

                if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
                {
                    nearestSpawnPosition = spawnPositionWorld;
                }
            }

            return nearestSpawnPosition;
        }
    }
}