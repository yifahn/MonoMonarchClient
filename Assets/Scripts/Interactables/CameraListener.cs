using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.ClientManagers.Game.Loading;
using Assets.Scripts.ClientManagers.Kingdom;
using MonoMonarchGameFramework.Game.Kingdom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public class CameraListener : MonoBehaviour
    {
      //  private readonly string[] nodeArray = new string[9] { "Grassland", "TownCentre", "House", "Library", "Factory", "Road", "Blockade", "Tower", "Wonder" };

      ////  private int nodeIdSelected { get; set; } = -1;
      //  private List<int> nodeIdSelectedList { get; set; } = new List<int>();
      //  private Vector3 previousMousePosition { get; set; } = Vector3.zero;
      //  void FixedUpdate()
      //  {
      //      if (SceneTracker.CurrentScene == 1) //building
      //      {
      //          if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)) //drag build
      //          {
      //              if (previousMousePosition != Input.mousePosition)
      //              {
      //                  previousMousePosition = Input.mousePosition;

      //                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      //                  RaycastHit hit;
      //                  if (Physics.Raycast(ray, out hit, 700))
      //                  {
      //                      if (nodeArray.Contains(hit.transform.gameObject.name))
      //                      {
      //                          int nodeId = KingdomState.CalculateNodeId((int)hit.transform.position.x, (int)hit.transform.position.z);
      //                          if (!nodeIdSelectedList.Contains(nodeId))
      //                          {
      //                              nodeIdSelectedList.Add(nodeId);
      //                          }
      //                          // int[] nodeIdArray = new int[1] { nodeId };
      //                          //GameManager.Instance.BuildEvent.Invoke(nodeIdArray);
      //                      }
      //                  }
      //              }
      //          }
      //          if (nodeIdSelectedList.Count > 0)
      //          {
      //              GameManager.Instance.BuildEvent.Invoke(nodeIdSelectedList.ToArray());
      //          }
      //      }
      //  }
    }
}
