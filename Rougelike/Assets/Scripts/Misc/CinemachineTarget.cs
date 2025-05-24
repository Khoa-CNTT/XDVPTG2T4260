using Cinemachine;
using System.Collections;
using tuleeeeee.Managers;
using tuleeeeee.Utilities;
using UnityEngine;

namespace tuleeeeee.Misc
{
    public class CinemachineTarget : MonoBehaviour
    {
        private CinemachineTargetGroup cinemachineTargetGroup;

        [SerializeField] private Transform cursorTarget;

        private void Awake()
        {
            cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        }

        void Start()
        {
            if (GlobalState.isCoop)
            {
                SetCinemachineTargetGroupCoop();
            }
            else
            {
                SetCinemachineTargetGroupSingle();
            }
        }
        private void SetCinemachineTargetGroupSingle()
        {
            CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target
            {
                weight = 1f,
                radius = 2.5f,
                target = GameManager.Instance.GetPlayer().transform
            };

            CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target
            {
                // weight = 0.43f,
                weight = 0.55f,
                radius = 2f,
                target = cursorTarget
            };

            CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[]{
            cinemachineGroupTarget_player,
            cinemachineGroupTarget_cursor
        };

            cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
        }
        private void SetCinemachineTargetGroupCoop()
        {
            CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target
            {
                weight = 1f,
                radius = 2.5f,
                target = GameManager.Instance.GetPlayer().transform
            };

            CinemachineTargetGroup.Target cinemachineGroupTarget_secondPlayer = new CinemachineTargetGroup.Target
            {
                weight = 1f,
                radius = 2.5f,
                target = GameManager.Instance.GetSecondPlayer().transform
            };
            CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target
            {
                // weight = 0.43f,
                weight = 0.55f,
                radius = 2f,
                target = cursorTarget
            };
            CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[]{
            cinemachineGroupTarget_player,
            cinemachineGroupTarget_secondPlayer,
            cinemachineGroupTarget_cursor
        };

            cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
        }

        private void Update()
        {
            cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
        }
    }
}