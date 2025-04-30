using UnityEditor;
using UnityEngine;

namespace tuleeeeee.Data
{
    [CreateAssetMenu(fileName = "CurrentPlayer", menuName = "Scriptable Objects/Player/Current Player", order = 0)]
    public class CurrentPlayerSO : ScriptableObject
    {
        public PlayerDetailsSO playerDetails;
        public string playerName;
    }
}