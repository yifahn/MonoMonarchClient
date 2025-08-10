using Assets.Scripts.ClientManagers.Game;
using Assets.Scripts.ClientManagers.Game.Loading;
using Assets.Scripts.ClientManagers.Kingdom;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interactables
{
    public class SceneInteractable : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            switch(gameObject.name)
            {
                case "btn_Kingdom_Scene":

                    GameManager.Instance.NavigateToUICanvas(1);
                    break;
                case "btn_Character_Scene":
                    GameManager.Instance.NavigateToUICanvas(2);

                    break;
                case "btn_Soupkitchen_Scene":
                    GameManager.Instance.NavigateToUICanvas(3);

                    break;
                case "btn_Bazaar_Scene":
                    GameManager.Instance.NavigateToUICanvas(4);

                    break;
                case "btn_Battleboard_Scene":
                    GameManager.Instance.NavigateToUICanvas(5);

                    break;
                case "btn_MainMenu_Scene":
                    GameManager.Instance.NavigateToUICanvas(6);

                    break;

            }
        }
    }
}
