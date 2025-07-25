using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.ClientManagers.Game;

namespace Assets.Scripts.Interactables
{
    public class SceneInteractable : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
           // GameManager.Instance.NavToScene(gameObject.name); -deprecated
        }
    }
}
