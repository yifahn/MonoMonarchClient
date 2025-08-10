using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scripts.ClientManagers.Game.Loading
{
    public static class SceneTracker
    {
        public static readonly string[] sceneOrder = new string[6] {"menu","map", "monarch", "soup","bazaar","battle"};
        private static int currentScene;
        /// <summary>
        /// 0:MainMenu - 1:Kingdom - 2:Character - 3:Soupkitchen - 4:Bazaar -5:Battleboard
        /// </summary>
        public static int CurrentScene { get { return currentScene; } set { currentScene = value; } }
    }
}
