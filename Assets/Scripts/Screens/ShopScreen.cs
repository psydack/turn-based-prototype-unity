using PlayFab;
using PlayFab.ClientModels;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopScreen : MonoBehaviour
{
	[SerializeField]
	private Transform _itemContainer;

	[SerializeField]
	private GameObject _itemPrefab;

	[SerializeField]
	private TMP_Text _shopLogText;

	public void DoClose()
	{
		SceneManager.LoadScene(Settings.MainMenuScene);
	}

	private void OnEnable()
	{
		StartCoroutine(Utility.IsUserLoggedIn());
		LoadStore(Settings.StoreId);
	}

	private void LoadStore(string storeID)
	{
		// TODO: Prevent to buy twice or more.
		PlayFabClientAPI.GetStoreItems(
			new GetStoreItemsRequest
			{
				StoreId = storeID
			},
			(result) =>
			{
				for (int index = 0; index < result.Store.Count; index++)
				{
					GameObject go = Instantiate(_itemPrefab);
					PlayFab.ClientModels.StoreItem storeItem = result.Store[index];
					StoreItem item = go.GetComponent<StoreItem>();

					item.IDText.text = storeItem.ItemId;
					item.PriceText.text = storeItem.VirtualCurrencyPrices[Settings.VirtualCurrencyCode].ToString();
					item.TitleText.text = storeItem.CustomData?.ToString();

					item.BuyButton.onClick.AddListener(() =>
						BuyItem(storeID, storeItem.ItemId, storeItem.VirtualCurrencyPrices[Settings.VirtualCurrencyCode]));

					go.transform.SetParent(_itemContainer);
					go.name = index + ": " + storeItem.ItemId;
				}
			},
			null);
	}

	private void BuyItem(string storeID, string itemID, uint price)
	{
		PlayFabClientAPI.PurchaseItem(
			new PurchaseItemRequest
			{
				ItemId = itemID,
				StoreId = storeID,
				VirtualCurrency = Settings.VirtualCurrencyCode,
				Price = (int)price
			},
			(result) =>
			{
				StringBuilder sb = new StringBuilder();
				foreach (ItemInstance item in result.Items)
				{
					sb
						.Append("ItemID: ")
						.Append(item.ItemId)
						.Append("DisplayName: ")
						.Append(item.DisplayName)
						.Append("ItemClass: ")
						.Append(item.ItemClass)
						.Append("PurchaseDate: ")
						.Append(item.PurchaseDate)
						.AppendLine();
				}
				_shopLogText.text = sb.ToString();
			},
			(error) => _shopLogText.text = error.GenerateErrorReport());
	}
}
