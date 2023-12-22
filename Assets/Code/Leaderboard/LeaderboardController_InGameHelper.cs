using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;

public class LeaderboardController_InGameHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void AddScore(double score)
    {
        if (AuthenticationService.Instance.IsAuthorized == false)
        {
            await SignInAnonymously();
        }

        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(GlobalVariables.LeaderboardId_BestTime, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }
}
