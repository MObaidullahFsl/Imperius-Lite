using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using SocketIOClient;
using Imperius.Logic;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace Imperius.Data
{
    public class LobbyDL : MonoBehaviour
    {
        private SocketIOUnity socket;
        private bool connected = false;
        public LobbyDL()
        {
            Debug.Log("Greeting from Login BL");
        }
        /// <summary>
        /// Connects to the Socket.IO server if not already connected
        /// </summary>
        private async Task EnsureConnection()
        {
            if (connected && socket != null) return;

            var uri = new Uri("http://localhost:3000"); // Replace with your server URL
            socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string> { { "token", "UNITY" } },
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            });

            socket.unityThreadScope = SocketIOUnity.UnityThreadScope.Update;

            // Optional: log connection events
            socket.OnUnityThread("connect", e => Debug.Log("Connected to server!"));
            socket.OnUnityThread("disconnect", e => Debug.Log("Disconnected from server"));

            socket.Connect();
            await socket.ConnectAsync();
            connected = true;
        }

        /// <summary>
        /// Requests a match from the server and waits for server response
        /// </summary>
        public async Task<MatchDTO> RequestMatchFromServer(MatchDTO myMatchData)
        {
            await EnsureConnection();

            var tcs = new TaskCompletionSource<MatchDTO>();

            // Clear any previous listeners to avoid duplicates
            socket.Off("matchFound");

            socket.OnUnityThread("matchFound", (response) =>
            {
                Debug.Log("MATCH FOUND: " + response.ToString());

                // Use JsonObject instead of string since server sends JSON object
                var jsonObject = response.GetValue<JsonObject>();

                if (jsonObject == null)
                {
                    Debug.LogWarning("Received empty match data from server.");
                    return;
                }

                // Deserialize safely into MatchDTO
                MatchDTO match = JsonConvert.DeserializeObject<MatchDTO>(jsonObject.ToString());

                if (match == null)
                {
                    Debug.LogWarning("Failed to deserialize match data.");
                    return;
                }

                // Ensure lists are not null
                match.EnemyTeam ??= new List<Character>();
                match.PlayerTeam ??= new List<Character>();

                // Ensure Decks are not null
                match.EnemyDeck ??= new Deck();
                match.PlayerDeck ??= new Deck();

                // Debug logs
                Debug.Log($"Match ID: {match.MatchId}");
                Debug.Log($"Room ID: {match.RoomId}");
                Debug.Log($"Enemy ID: {match.EnemyName}");
                Debug.Log($"Active Player: {match.ActivePlayer}");
                Debug.Log($"Enemy Team Count: {match.EnemyTeam.Count}");

                // Set the result for awaiting tasks
                tcs.TrySetResult(match);
            });

            // Send player data to server

            var matchJson = JsonConvert.SerializeObject(new
            {
                name = myMatchData.PlayerName,
                team = myMatchData.PlayerTeam,
                deck = myMatchData.PlayerDeck
            });

            socket.Emit("findMatch", matchJson);

            // Await server response
            return await tcs.Task;
        }
        public async Task StopMatch()
        {
            if (socket == null) return;

            // Emit a "leaveMatch" event to server
            socket.Emit("leaveMatch");

            // Optional: disconnect socket if you want to fully leave
            await socket.DisconnectAsync();
            connected = false;
        }

        public async Task<PlayerDTO> getPlayerFromDB(User user)
        {
            // fetch player data from persistent storage or server using user info
            // do await Task.Yield(); on the web request
            PlayerDTO p = new();

            return p;
        }

    }
}

