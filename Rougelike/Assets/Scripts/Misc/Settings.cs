﻿using System.Collections;
using UnityEngine;

namespace tuleeeeee.Misc
{
    public static class Settings
    {
        #region UNITS
        public const float pixelsPerUnit = 16f;
        public const float tileSizePixels = 16f;
        #endregion

        #region DUNGEON BUILD SETTINGS
        public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
        public const int maxDungeonBuildAttempts = 10;
        #endregion

        #region ROOM SETTINGS
        public const float fadeInTime = 0.5f;
        public const int maxChildCorridors = 3;
        public const int maxChildConnection = 2;
        public const float doorUnlockDelay = 1f;
        #endregion

        #region ANIMATOR PARAMETERS

        public static int aimUp = Animator.StringToHash("aimUp");
        public static int aimDown = Animator.StringToHash("aimDown");
        public static int aimUpRight = Animator.StringToHash("aimUpRight");
        public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
        public static int aimRight = Animator.StringToHash("aimRight");
        public static int aimLeft = Animator.StringToHash("aimLeft");

        public static int isIdle = Animator.StringToHash("isIdle");
        public static int isMoving = Animator.StringToHash("isMoving");

        public static int rollUp = Animator.StringToHash("rollUp");
        public static int rollUpRight = Animator.StringToHash("rollUpRight");
        public static int rollRight = Animator.StringToHash("rollRight");
        public static int rollLeft = Animator.StringToHash("rollLeft");
        public static int rollDown = Animator.StringToHash("rollDown");

        public static int flipUp = Animator.StringToHash("flipUp");

        public static int flipRight = Animator.StringToHash("flipRight");
        public static int flipLeft = Animator.StringToHash("flipLeft");
        public static int flipDown = Animator.StringToHash("flipDown");

        public static int use = Animator.StringToHash("use");
        public static float baseSpeedForPlayerAnimation = 8f;
        public static float baseSpeedForEnemyAnimation = 3f;
        // Door
        public static int open = Animator.StringToHash("open");
        public static int destroy = Animator.StringToHash("destroy");
        public static string stateDestroyed = "Destroyed";
        #endregion

        #region GAMEOBJECT TAGS
        public const string playerTag = "Player";
        public const string playerWeapon = "playerWeapon";
        #endregion
    }
}