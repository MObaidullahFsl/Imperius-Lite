using UnityEngine;
using UnityEngine.UI;
using Imperius.Logic;
using Imperius.Data;
using TMPro;

public class SignInModal : MonoBehaviour
{
    public Button SubmitBtn;
    public Button GoToSignUpBtn;
    public TMP_InputField Username;
    public TMP_InputField Password;

    void Start()
    {
        if (Password != null) Password.contentType = TMP_InputField.ContentType.Password;

        SubmitBtn.onClick.AddListener(Submit);
        GoToSignUpBtn.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/SignUpModal"), transform.parent);
            Destroy(gameObject);
        });

        Show();
    }

     void Submit()
    {
        string user = Username?.text.Trim();
        string pass = Password?.text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            Debug.LogError("Username or password cannot be empty!");
            ShowAlert("Username or password cannot be empty!");
            return;
        }

        var login = GameObject.FindFirstObjectByType<LoginUI>();
        if (login == null)
        {
            Debug.LogError("LoginUI not found in scene!");
            return;
        }

        try
        {
             login.signinUser(new UserDTO
            {
                username = user,
                password = pass,
                email = "" // optional
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
            ShowAlert("Login failed: " + e.Message);
        }
    }

    private void ShowAlert(string message)
    {
        var alertGO = Instantiate(Resources.Load<GameObject>("Prefabs/Alert"), transform.parent);
        var alert = alertGO.GetComponent<Alert>();
        if (alert != null)
        {
            alert.SetText(message);
            alert.Show();
        }
    }

    public void Show()
    {
        GetComponent<ModalAnimator>()?.Show();
    }

    public void Hide()
    {
        GetComponent<ModalAnimator>()?.Hide();
    }
}
