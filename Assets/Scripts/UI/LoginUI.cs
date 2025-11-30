using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.Data;
using System;
using TMPro;
using Unity.VisualScripting;
public class LoginUI : MonoBehaviour
{
    public Button Play;

    // login username and password field
    public LoginBL loginBL;

    public bool isLoading = false;

    public GameObject SignInModal;
    public GameObject SignUpModal;
    public GameObject Canvas;

    public GameObject currentModal;

    void Start()
    {

        Play = GameObject.Find("Play").GetComponent<Button>();
        Play.AddComponent<ButtonEffects>();
        Play.onClick.AddListener(Play_OnClick);

        loginBL = new();

        SignInModal = Resources.Load<GameObject>("Prefabs/SignInModal");
        SignUpModal = Resources.Load<GameObject>("Prefabs/SignUpModal");
        Canvas = GameObject.Find("Canvas");

        isLoading = false;

    }

    public async void Play_OnClick()
    {

        currentModal = Instantiate(SignInModal, Canvas.transform);
        // if (isLoading) return;
        // isLoading = true;
    }

    public async void signinUser(UserDTO u)
    {
        if (isLoading) return;
        isLoading = true;
        try
        {
            // UserDTO u = new UserDTO()
            // {
            //     username = "default_user",
            //     password = "12345678",
            //     email = "default_mail"
            // };

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


    public async void signupUser(UserDTO u)
    {   
        if (isLoading) return;
        isLoading = true;
        try
        {
            // UserDTO u = new UserDTO()
            // {
            //     username = "default_user",
            //     password = "12345678",
            //     email = "default_mail"
            // };

            User user = await loginBL.loginUser(u);
            Debug.Log("User: " + user);

            if (user != null)
            {
                Debug.Log("Signing in...");
                GlobalContext.Instance.SetUser(user);
                GlobalContext.Instance.SetState(GameState.Lobby);
                await SceneManager.LoadSceneAsync(1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Signing in failed: " + e.Message);
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
