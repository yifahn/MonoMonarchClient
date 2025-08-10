using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.ClientManagers.Game.Loading;
using Assets.Scripts.ClientManagers.Kingdom;
using MonoMonarchGameFramework.Game.Kingdom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class NodeInteractables : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
        }


        private void OnMouseDown()
        {
            if (!Input.GetKey("left shift"))
            {
                int nodeId = GetNodeId();
                if (GameManager.Instance.NodeIdSelected != nodeId)
                {
                    RaiseBuildEvent(nodeId);
                    GameManager.Instance.NodeIdSelected = nodeId;
                }
                if (Input.GetMouseButton(1))
                {
                    KingdomManager.Instance.UpdateNodeStatistics(nodeId);
                }
            }
        }

        private void OnMouseOver()
        {
            if (!Input.GetKey("left shift"))
            {
                int nodeId = GetNodeId();
                if (GameManager.Instance.NodeIdSelected != nodeId)
                {
                    if (Input.GetMouseButton(0))
                    {
                        RaiseBuildEvent(nodeId);
                        GameManager.Instance.NodeIdSelected = nodeId;
                    }
                }
                if (Input.GetMouseButton(1))
                {
                    KingdomManager.Instance.UpdateNodeStatistics(nodeId);
                }
            }
        }
        private void RaiseBuildEvent(int nodeId)
        {
            if (SceneTracker.CurrentScene == 1)
            {

                // GameManager.Instance.BuildEvent.Invoke(new int[] { nodeId });
                if (nodeId >= 0 && nodeId < KingdomManager.Instance.Map.Length)
                {
                    GameManager.Instance.BuildEvent.Invoke(new int[] { nodeId });
                }
                else
                {
                    Debug.LogWarning($"Invalid nodeId: {nodeId} for position {transform.position}");
                }
            }

        }
        private int GetNodeId()
        {
            int x = ((int)transform.position.x) / 3;
            int y = -((int)transform.position.z) / 3;
            int nodeId = KingdomState.CalculateNodeId(x, y);
            return nodeId;
        }
    }
}


