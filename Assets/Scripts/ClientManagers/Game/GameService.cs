using System.Threading.Tasks;

using UnityEngine;

using MonoMonarchNetworkFramework.Authentication.Register;
using MonoMonarchNetworkFramework.Authentication.Login;
using MonoMonarchNetworkFramework.Authentication.Logout;
using MonoMonarchNetworkFramework.Authentication.RefreshToken;
using MonoMonarchNetworkFramework.Game.Treasury;
using MonoMonarchNetworkFramework.Game.Character;
using MonoMonarchNetworkFramework.Game.Soupkitchen;
using MonoMonarchNetworkFramework.Game.Kingdom;
using MonoMonarchNetworkFramework.Game.Armoury;
using MonoMonarchNetworkFramework.Game.Kingdom.Map;

namespace Assets.Scripts.ClientManagers.Game
{
    //SYSTEM.NET LIBRARY IS NOT SUPPORTED BY WEBGL DUE TO JAVA CONVERSION NOT SUPPORTING NETWORK SOCKETS... OR SOMETHING LIKE THAT - DONT USE SYSTEM.NET NAMESPACE!!!!!!!! - UnityWebRequest ONLY!!!!
    #region Interface Definitions
    //F2P WITH ADVERTISEMENTS OR MANNEQUIN SUBSCRIPTIONS (minimum 1 additional mannequin to remove ads)
    // 5/3/25 - read somewhere games relying on ads as business model are banned from steam - go mannequin
    public interface IGameService
    {
       // int ResolveScene(string sceneName);  -deprecated
    }
    public interface IUserService
    {
        Task<IRegistrationResponse> RegisterAsync(RegistrationPayload registrationPayload);
        Task<ILoginResponse> LoginAsync(LoginPayload loginPayload);
        Task<ILogoutResponse> LogoutAsync(LogoutPayload logoutPayload);
        Task<IRefreshTokenResponse> RefreshTokenAsync(RefreshTokenPayload refreshTokenPayload);

    }
   
    public interface IKingdomService
    {
        Task<IKingdomLoadResponse> KingdomLoadAsync();
        Task<IKingdomMapUpdateResponse> KingdomMapUpdateAsync(KingdomMapUpdatePayload payload);

        //UPDATE KINGDOM NAME
    }
    public interface IArmouryService
    {
        Task<IArmouryLoadResponse> ArmouryLoadAsync();
        //LOAD ARMOURY
        //ADD MANNEQUIN (subscription service +$1 cost for each additional 
        //~CRAFTING??
    }
    public interface ITreasuryService
    {
        Task<ITreasuryLoadResponse> TreasuryLoadAsync();
        
        //ADD COINBAG (cost 1 political point)
    }
    public interface ICharacterService
    {
        Task<ICharacterLoadResponse> CharacterLoadAsync();
        //UPDATE INVENTORY
        //UPDATE CHARACTER NAME
        //ADD POLITICALPOINT
    }
    public interface ISoupkitchenService
    {
        Task<ISoupkitchenLoadResponse> SoupkitchenLoadAsync();
        //CLAIM SOUP
    }
    public interface IBattleboardService
    {
        //Task<IBattleboardLoadResponse> BattleboardLoadAsync();

        //-YET TO IMPLEMENT SERVER-SIDE~

        //~LEADERBOARD (seasonal/all-time > ranked by prestige points)

        //~BATTLE (cost == 1 political point)
        //(win == prestige point granted 
        //&& character is on cooldown for x amount of time, during cooldown kingdom is advertised on battleboard to be attacked by all character.level < +10 (kingdom_map is frozen))
        //|| lose == character death event
        // ALSO seeded kingdoms are available to battle


    }
    #endregion

    public class GameService : IGameService
    {
        //public int ResolveScene(string sceneName) - deprecated
        //{
        //    switch (sceneName)
        //    {
        //        //bottom canvas
        //        case "btn_MainMenu_Scene":
        //            return 0;
        //        //left canvas
        //        case "btn_Kingdom_Scene":
        //            return 1;

        //        case "btn_Character_Scene":
        //            return 2;

        //        case "btn_Soupkitchen_Scene":
        //            return 3;

        //        case "btn_Bazaar_Scene":
        //            return 4;

        //        case "btn_Battleboard_Scene":
        //            return 5;


        //        //login button on main menu
        //        case "BTN_LOGIN_SUBMIT_L"://BTN_LOGIN_SUBMIT_L
        //            return 1; //register directs to tutorial scene? - login directs to map scene?
        //    }
        //    //default
        //    return 0;
        //}
    }
}
