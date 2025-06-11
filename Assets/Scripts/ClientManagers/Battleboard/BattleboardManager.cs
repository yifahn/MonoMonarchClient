using UnityEngine;

namespace Assets.Scripts.ClientManagers.Battleboard
{
    public class BattleboardManager : MonoBehaviour
    {

        // Singleton instance
        private static BattleboardManager _instance;

        // Public accessor for the instance
        public static BattleboardManager Instance
        {
            get
            {
                // If the instance is null, find the existing instance in the scene
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<BattleboardManager>();

                    // If no instance is found, create a new GameObject and attach the script
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(BattleboardManager).Name);
                        _instance = singletonObject.AddComponent<BattleboardManager>();
                    }

                    // Make sure the object persists across scenes
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
        public static void ResetInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
        // Your manager's methods and variables go here
        // For example:
        // public void YourMethod() { /* Your implementation here */ }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Ensure there's only one instance of the manager
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        // Add other necessary methods and variables for your manager here
    }
}
