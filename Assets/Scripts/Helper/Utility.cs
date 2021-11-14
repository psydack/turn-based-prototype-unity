using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
	public static void IsEmptyCanvasElement<T>(T field)
		where T : ICanvasElement
	{
		if (field == null)
		{
			Debug.LogError($"Please assign: {typeof(T)}.");
		}
	}

	public static void IsEmptyObject<T>(T field)
		where T : Object
	{
		if (field == null)
		{
			Debug.LogError($"Please assign: {typeof(T)}.");
		}
	}

	public static IEnumerator IsUserLoggedIn()
	{
		while (PlayFabClientAPI.IsClientLoggedIn())
		{
			yield return new WaitForSeconds(3f);
		}

		Debug.LogWarning($"You're not logged in. If you're trying to test a particular scene, use the MockLogin method.");

		// TODO: Show UI with information;
		UnityEngine.SceneManagement.SceneManager.LoadScene(Settings.LoginScene);
	}

	public static void DestroyAllChildren<T>(T container) where T : Component
	{
		foreach (Transform child in container.GetComponentInChildren<Transform>())
		{
			Object.Destroy(child.gameObject);
		}
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

#if UNITY_EDITOR
	public static void MockLogin(System.Action<PlayFab.ClientModels.LoginResult> callback)
	{
		PlayFab.ClientModels.LoginWithCustomIDRequest request = new PlayFab.ClientModels.LoginWithCustomIDRequest
		{
			CustomId = "rofli2-mock",
			CreateAccount = true
		};
		PlayFab.PlayFabClientAPI.LoginWithCustomID(
			request,
			callback,
			(PlayFab.PlayFabError error) => Debug.LogError(error.GenerateErrorReport()));
	}
#endif
}
