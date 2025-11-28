using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.Data;
using System;
public class LoginUI : MonoBehaviour
{
    public Button Login;
    public Button Guest;

    // login username and password field
    public LoginBL loginBL;

    public bool isLoading = false;
    void Start()
    {

        Login = GameObject.Find("Login").GetComponent<Button>();
        Login.onClick.AddListener(Login_OnClick);
    
        loginBL = new();

    }

    public async void Login_OnClick()
    {
        if (isLoading) return;
        isLoading = true;

        try
        {
            UserDTO u = new UserDTO()
            {
                username = "default_user",
                password = "12345678",
                email = "default_mail"
            };

            User user = await loginBL.loginUser(u);
            Debug.Log("User: " + user);
            
            if (user != null)
            {
                Debug.Log("Logging in...");
                GlobalContext.Instance.SetUser(user);
                GlobalContext.Instance.SetState(GameState.Lobby);
                await SceneManager.LoadSceneAsync(1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
        }
        finally
        {
            isLoading = false;
        }
    }



    void Guest_OnClick()
    {
        if (isLoading) return; // ignore second click
        isLoading = true;
        GlobalContext.Instance.SetUser(new User(1, "Guest"));
        GlobalContext.Instance.SetState(GameState.Lobby);
        SceneManager.LoadScene("LobbyScene");

    }

    // Update is called once per frame
    // void Update()
    // {

    // }
}
