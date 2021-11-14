using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField _customIDText;

	[SerializeField]
	private Button _loginButton;

	public void DoLogin()
	{
		if (!string.IsNullOrWhiteSpace(_customIDText.text))
		{
			_loginButton.interactable = false;
			LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
			{
				CustomId = _customIDText.text.Trim(),
				CreateAccount = true
			};
			PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
		}
		else
		{
			// TODO: Show UI Error;
		}
	}

	private void OnLoginSuccess(LoginResult result)
	{
		// TODO: Add loading
		SceneManager.LoadScene(Settings.MainMenuScene);
	}

	private void OnLoginFailure(PlayFabError error)
	{
		_loginButton.interactable = true;
		Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
		// TODO: Show UI Error;
	}

	private void OnEnable()
	{
		Utility.IsEmptyCanvasElement(_customIDText);
		Utility.IsEmptyObject(_loginButton);

		_loginButton.onClick.AddListener(DoLogin);
	}

	private void OnDisable()
	{
		_loginButton.onClick.RemoveAllListeners();
	}
}