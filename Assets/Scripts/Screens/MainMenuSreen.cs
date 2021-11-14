using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSreen : MonoBehaviour
{
	[Header("Currency virtual properties")]
	[SerializeField]
	private TMP_Text _goTitle;

	[SerializeField]
	private TMP_Text _goValue;

	[SerializeField]
	private TMP_InputField _amountInputField;

	[SerializeField]
	private Button _addVirtualCurrencyButton;

	[SerializeField]
	private Button _subtractVirtualCurrencyButton;

	[Header("Inventory List")]
	[SerializeField]
	private Transform _inventoryListContainer;

	[SerializeField]
	private GameObject _inventoryItemPrefab;

	public void OpenShop()
	{
		SceneManager.LoadScene(Settings.ShopScene);
	}

	public void GoToPreBattle()
	{
		SceneManager.LoadScene(Settings.PreBattleScene);
	}

	public void AddUserVirtualCurrency()
	{
		// TODO: Handle wait response for double request.
		PlayFabClientAPI.AddUserVirtualCurrency(
			new AddUserVirtualCurrencyRequest
			{
				Amount = int.Parse(_amountInputField.text),
				VirtualCurrency = Settings.VirtualCurrencyCode
			},
			(result) => _goValue.text = result.Balance.ToString(),
			null);
	}

	public void SubtractUserVirtualCurrency()
	{
		// TODO: Handle wait response for double request.
		PlayFabClientAPI.SubtractUserVirtualCurrency(
			new SubtractUserVirtualCurrencyRequest
			{
				Amount = int.Parse(_amountInputField.text),
				VirtualCurrency = Settings.VirtualCurrencyCode
			},
			(result) => _goValue.text = result.Balance.ToString(),
			null);
	}

	private void OnEnable()
	{
		Utility.IsEmptyCanvasElement(_goTitle);
		Utility.IsEmptyCanvasElement(_goValue);
		Utility.IsEmptyCanvasElement(_amountInputField);
		Utility.IsEmptyObject(_addVirtualCurrencyButton);
		Utility.IsEmptyObject(_subtractVirtualCurrencyButton);

		Utility.IsEmptyObject(_inventoryListContainer);
		Utility.IsEmptyObject(_inventoryItemPrefab);

		Utility.MockLogin((result) =>
		{
			StartCoroutine(Utility.IsUserLoggedIn());
			GetCurrentVirtualCurrency();
			LoadInventory();
		});
		Debug.Log(Player.Instance.Skills);
		//StartCoroutine(nameof(IsUserLoggedIn));
		//GetCurrentVirtualCurrency();
		//GetItemsList();
	}

	private void GetCurrentVirtualCurrency()
	{
		// TODO: Handle error in a better way.
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
			(result) =>
			{
				int value = result
					.VirtualCurrency?
					.First(x => x.Key == Settings.VirtualCurrencyCode)
					.Value ?? 0;
				_goValue.text = value.ToString();
			},
			(error) => GetCurrentVirtualCurrency());
	}

	private void LoadInventory()
	{
		// TODO: Use polling.
		Utility.DestroyAllChildren(_inventoryListContainer);

		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
		{
			for (int index = 0; index < result.Inventory.Count; index++)
			{
				GameObject item = Instantiate(_inventoryItemPrefab);
				item.transform.SetParent(_inventoryListContainer);
				item.name = index + ": " + result.Inventory[index].DisplayName;
				item.GetComponent<TMP_Text>().text = result.Inventory[index].DisplayName;
			}
		}, null);
	}
}
