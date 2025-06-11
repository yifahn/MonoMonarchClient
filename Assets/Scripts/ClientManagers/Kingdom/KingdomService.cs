using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;

using Assets.Scripts.ClientManagers.User;

using MonoMonarchNetworkFramework;
using MonoMonarchNetworkFramework.Game.Kingdom;
using MonoMonarchNetworkFramework.Game.Soupkitchen;

using Assets.Scripts.ClientManagers.Game;
using MonoMonarchNetworkFramework.Game.Kingdom.Map;

namespace Assets.Scripts.ClientManagers.Kingdom
{
    public class KingdomService : IKingdomService
    {
        public async Task<IKingdomLoadResponse> KingdomLoadAsync()
        {

            string url = $"{NetworkConstants.GetServerURLPrefix()}/kingdom/loadkingdom";  //http://localhost:5223/kingdom/loadkingdom
            UnityWebRequest webRequest = new UnityWebRequest(url, "GET");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {UserManager.Instance.UserLoginResponse.AuthToken}");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // await webRequest.SendWebRequest();

            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            { await Task.Yield(); }



            IKingdomLoadResponse response = null;
            JsonSerializer serialiser = new JsonSerializer();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (webRequest.responseCode == 200)
                {
                    Debug.Log("Web request result: " + webRequest.downloadHandler.text);

                    using (StringReader sr = new StringReader(webRequest.downloadHandler.text))
                    {
                        using (JsonReader reader = new JsonTextReader(sr)) response = serialiser.Deserialize<KingdomLoadResponse>(reader);
                    }
                }
                else response = new ErrorResponse("Web request is Ok but is not StatusCode 200");
            }
            else if (webRequest.responseCode == 400)
            {
                Debug.LogWarning("Web request error (400): " + webRequest.downloadHandler.text);

                using (StringReader sr = new StringReader(webRequest.downloadHandler.text))
                {
                    using (JsonReader reader = new JsonTextReader(sr)) response = serialiser.Deserialize<ErrorResponse>(reader);
                }
            }
            else if (webRequest.responseCode == 500)
            {
                Debug.LogError("Server error (500).");
                response = new ErrorResponse("Internal server error.");
            }
            else if (webRequest.responseCode == 401)
            {//potential endless loop if untested and turns out to always produce unauthorized error
                await UserManager.Instance.RefreshTokenAsync();
                await KingdomManager.Instance.KingdomLoadAsync();
            }
            else //Unknown error code 
            {
                Debug.LogError("Unexpected response code: " + webRequest.responseCode);
                response = new ErrorResponse($"Unexpected response: {webRequest.responseCode}");
            }
            return response;
        }

        public async Task<IKingdomMapUpdateResponse> KingdomMapUpdateAsync(KingdomMapUpdatePayload payload)
        {

            string url = $"{NetworkConstants.GetServerURLPrefix()}/kingdom/updatemap";  //http://localhost:5223/kingdom/updatemap
            UnityWebRequest webRequest = new UnityWebRequest(url, "GET");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {UserManager.Instance.UserLoginResponse.AuthToken}");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // await webRequest.SendWebRequest();

            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            { await Task.Yield(); }



            IKingdomMapUpdateResponse response = null;
            JsonSerializer serialiser = new JsonSerializer();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (webRequest.responseCode == 200)
                {
                    Debug.Log("Web request result: " + webRequest.downloadHandler.text);

                    using (StringReader sr = new StringReader(webRequest.downloadHandler.text))
                    {
                        using (JsonReader reader = new JsonTextReader(sr)) response = serialiser.Deserialize<KingdomMapUpdateResponse>(reader);
                    }
                }
                else response = new ErrorResponse("Web request is Ok but is not StatusCode 200");
            }
            else if (webRequest.responseCode == 400)
            {
                Debug.LogWarning("Web request error (400): " + webRequest.downloadHandler.text);

                using (StringReader sr = new StringReader(webRequest.downloadHandler.text))
                {
                    using (JsonReader reader = new JsonTextReader(sr)) response = serialiser.Deserialize<ErrorResponse>(reader);
                }
            }
            else if (webRequest.responseCode == 500)
            {
                Debug.LogError("Server error (500).");
                response = new ErrorResponse("Internal server error.");
            }
            else if (webRequest.responseCode == 401)
            {//potential endless loop if untested and turns out to always produce unauthorized error
                await UserManager.Instance.RefreshTokenAsync();
                await KingdomManager.Instance.KingdomMapUpdateAsync(payload);
            }
            else //Unknown error code 
            {
                Debug.LogError("Unexpected response code: " + webRequest.responseCode);
                response = new ErrorResponse($"Unexpected response: {webRequest.responseCode}");
            }
            return response;
        }
    }
}
