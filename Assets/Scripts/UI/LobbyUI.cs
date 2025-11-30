using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.UI;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Imperius.Data;
using System.Collections.Generic;
using System.Linq;

namespace Imperius.UI
{
    public class LobbyUI : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public Button BattleButton;

        private LobbyBL lobbyBL; // drag to inspector if it uses Monobehaviours

        Player player;

        bool isLoading;
        void Start()
        {

            BattleButton = GameObject.Find("Battle").GetComponent<Button>();
            BattleButton.onClick.RemoveListener(Battle_OnClick);
            BattleButton.onClick.AddListener(Battle_OnClick);
            isLoading = false;
            lobbyBL = new();

            _ = InitializePlayerAsync();

            MatchContext.Instance.PlayerName = GlobalContext.Instance.GlobalUser.Username;
            MatchContext.Instance.player = player;
        }

        private async Task InitializePlayerAsync()
        {
            try
            {
                player = await lobbyBL.fetchPlayer();
                Debug.Log("player loaded: " + player);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load player: " + ex.Message);
            }
        }


        public async void Battle_OnClick()
        {
            if (isLoading) return;
            isLoading = true;

            var modal = UI_Util.CreateModal("Choose Team! Upto 3 Characters");

            // WAIT for user selection
            List<Character> selectedList = await modal.CharacterSelectModeAsync(
                "Choose upto 3 Characters!",
                3,
                player.PlayerCharactersList
            );

            foreach (var c in selectedList)
            {
                Debug.Log("Returned : " + c.name);
            }

            if (selectedList == null || selectedList.Count > 3)
            {
                Debug.Log("Matchmaking cancelled.");
                isLoading = false;
                return;
            }

            MatchContext.Instance.PlayerTeam = selectedList;

            foreach (var c in MatchContext.Instance.PlayerTeam)
            {
                Debug.Log("char : " + c.name);
            }


            modal = UI_Util.CreateModal("Choose Your Deck!");


            Deck selectedDeck = await modal.DeckSelectMode(player.DecksList);

            if (selectedDeck == null)
            {
                Debug.Log("Matchmaking cancelled. No deck selected");
                isLoading = false;
                return;
            }

            Debug.Log("Returned : " + selectedDeck.Name);


            modal = UI_Util.CreateModal("MatchMacking");

            modal.AddContentText("Waiting for players ... ");

            modal.AddFooterButton("Cancel", async () =>

            {

                modal.Hide(true);

                //await lobbyBL.CancelMatch(); 

            });

            // temp solution

            MatchContext.Instance.EnemyName = "default enemy";

            Character c1 = new("default1", Element.Fire, 1, "Does literally nothing", 100);
            List<Character> clist = new()
        {
            c1
        };  

            List<Character> templist = new();
            templist.Add(c1);

            MatchContext.Instance.EnemyTeam = templist;

            Deck DefaultDeck = new("Preset_1 (Enemy)");

            foreach (var ch in player.PlayerCharactersList)
            {
                for (int i = 0; i < DefaultDeck.CardsNumber; i++)
                {
                    DefaultDeck.AddCard(player.PlayerCardsList[i]);
                }
            } 

            MatchContext.Instance.EnemyDeck = DefaultDeck;
            MatchContext.Instance.PlayerDeck = DefaultDeck;
            GlobalContext.Instance.SetState(GameState.Battle);
            SceneManager.LoadScene("BattleScene");



            // try
            // {

            // MatchDTO m = new()
            // {
            //     PlayerName = MatchContext.Instance.PlayerName,
            //     PlayerDeck = selectedDeck,
            //     PlayerTeam = selectedList
            // };


            //send match dto to server

            //bool matchFound = await lobbyBL.StartMatch(m);

            //     if (matchFound)
            //     {
            //         Debug.Log("Match found!");

            //         modal.Hide(true);

            //         // <-- HIDE MODAL

            //         GlobalContext.Instance.SetState(GameState.Battle);
            //         SceneManager.LoadScene("BattleScene");

            //     }
            //     else
            //     {
            //         Debug.LogWarning("Match not found.");
            //         // <-- HIDE MODAL ON FAILURE
            //     }
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError("Matchmaking failed: " + e.Message);
            //     // <-- HIDE MODAL ON ERROR
            // }
            // finally
            // {
            //     isLoading = false;
            // }
        }


        // Update is called once per frame
        void Update() { }


    }

}
