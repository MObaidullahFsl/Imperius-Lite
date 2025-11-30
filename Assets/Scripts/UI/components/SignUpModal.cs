using UnityEngine;
using UnityEngine.UI;
using Imperius.Logic;
using Imperius.Data;
using TMPro;

public class SignUpModal : MonoBehaviour
{
    public Button SubmitBtn;
    public Button GoToSignInBtn;
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField ConfirmPassword;

    void Start()
    {
        // Ensure password fields are masked
        if (Password != null) Password.contentType = TMP_InputField.ContentType.Password;
        if (ConfirmPassword != null) ConfirmPassword.contentType = TMP_InputField.ContentType.Password;

        SubmitBtn.onClick.AddListener(Submit);
        GoToSignInBtn.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/SignInModal"), transform.parent);
            Destroy(gameObject);
        });

        Show();
    }

    void Submit()
    {
        string user = Username?.text.Trim();
        string pass = Password?.text;
        string confirm = ConfirmPassword?.text;

        // Validate input
        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            Debug.LogError("Username or password cannot be empty!");
            ShowAlert("Username or password cannot be empty!");
            return;
        }

        if (pass != confirm)
        {
            Debug.LogError("Passwords do not match!");
            ShowAlert("Passwords do not match!");
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
            login.signupUser(new UserDTO
            {
                username = user,
                password = pass,
                email = "" // optional
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("Signup failed: " + e.Message);
            ShowAlert("Signup failed: " + e.Message);
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
