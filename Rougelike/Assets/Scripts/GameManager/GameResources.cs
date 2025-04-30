using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
using tuleeeeee.Misc;
using tuleeeeee.NodeGraph;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }
    #region DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region ToolTip
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    #endregion
    public RoomNodeTypeListSO roomNodeTypeList;
    public GameObject playerSelectionPrefab;

    #region Header PLAYER
    [Space(10)]
    [Header("PLAYER")]
    #endregion
    #region Tooltip
    [Tooltip("The current player SO, used to reference the current player between scenes")]
    #endregion
    public CurrentPlayerSO currentPlayerSO;
    public List<PlayerDetailsSO> playerDetailsList;

    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("Sprite-Lit_Default Material")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("Populate with the Variable Lit Shader")]
    #endregion
    public Shader variableLitShader;
    public Shader materializeShader;
}

