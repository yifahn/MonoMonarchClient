using Assets.Scripts.ClientManagers.Game;
using MonoMonarchGameFramework.Game.Character;
using MonoMonarchGameFramework.Game.Treasury;
using MonoMonarchNetworkFramework;
using MonoMonarchNetworkFramework.Game.Character;
using MonoMonarchNetworkFramework.Game.Kingdom;
using MonoMonarchNetworkFramework.Game.Treasury;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.ClientManagers.Character
{
    public class CharacterManager : MonoBehaviour
    {

        #region Character Singleton
        private static ICharacterService _characterService { get; set; }
        private static CharacterManager _instance;
        public static CharacterManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CharacterManager>();
                    if (_instance == null)
                    {
                        GameObject singletonInstance = new GameObject(typeof(CharacterManager).Name);
                        _instance = singletonInstance.AddComponent<CharacterManager>();
                        _characterService = new CharacterService();
                    }
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
        private void Awake()
        {
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
        #endregion

        #region Character Properties
        public CharacterLoadResponse CharacterLoadResponse { get; set; }
        public ErrorResponse CharacterErrorResponse { get; set; }
        public CharacterState CharacterState { get => _characterState; set => _characterState = value; }
        private CharacterState DeserialiseCharacterState(string serialisedCharacterState)
        {
            using (StringReader sr = new StringReader(serialisedCharacterState))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                    return new JsonSerializer().Deserialize<CharacterState>(reader);
            }
        }
        [SerializeField] private CharacterState _characterState;

        #endregion

        public async Task<bool> CharacterLoadAsync()
        {
            try
            {
                var response = await _characterService.CharacterLoadAsync();
                if (response is CharacterLoadResponse characterLoadResponse)
                {
                    CharacterLoadResponse = characterLoadResponse;

                    CharacterState = DeserialiseCharacterState(CharacterLoadResponse.CharacterState);
                }
                else if (response is ErrorResponse errorResponse)
                {
                    CharacterErrorResponse = errorResponse;
                    await LoadingScreenExtensions.LoadLoadingSceneAsync();
                    await LoadingScreenExtensions.UnloadGameSceneAsync();
                    await LoadingScreenExtensions.LoadMainMenuSceneAsync();
                    await LoadingScreenExtensions.UnloadLoadingSceneAsync();
                    GameManager.Instance.ClearGameCache();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR-RESPONSE FAILURE");
                Debug.Log(ex);
                await LoadingScreenExtensions.LoadLoadingSceneAsync();
                await LoadingScreenExtensions.UnloadGameSceneAsync();
                await LoadingScreenExtensions.LoadMainMenuSceneAsync();
                await LoadingScreenExtensions.UnloadLoadingSceneAsync();
                GameManager.Instance.ClearGameCache();
                return false;
            }
        }


        public void ClearCharacterCache()
        {
            CharacterLoadResponse = null;
            CharacterState = null;
        }
    }
}
