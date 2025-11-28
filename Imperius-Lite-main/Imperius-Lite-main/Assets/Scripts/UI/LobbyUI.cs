using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.UI;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Imperius.Data;

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

            // <-- SHOW MODAL
            var modal = GlobalContext.Instance.CreateModal("Choose Team!");

            foreach (var c in player.PlayerCharactersList)
            {
                // get img where 
                Debug.Log("Element of default char: " + c.element);
                modal.AddImageItem(GlobalContext.Instance.ImageItemPrefab, c.element.ToString(), c.Portrait, () =>
                {

                })


            }



            try
            {
                bool matchFound = await lobbyBL.StartMatch();

                if (matchFound)
                {
                    Debug.Log("Match found!");

                    // <-- HIDE MODAL

                    GlobalContext.Instance.SetState(GameState.Battle);
                    SceneManager.LoadScene("BattleScene");

                }
                else
                {
                    Debug.LogWarning("Match not found.");
                    // <-- HIDE MODAL ON FAILURE
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Matchmaking failed: " + e.Message);
                // <-- HIDE MODAL ON ERROR
            }
            finally
            {
                isLoading = false;
            }
        }


        // Update is called once per frame
        void Update() { }


    }

}
