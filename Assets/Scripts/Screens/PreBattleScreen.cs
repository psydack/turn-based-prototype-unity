using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreBattleScreen : MonoBehaviour
{
	private class ModifierDataUsed
	{
		public SkillModifierData Modifier;
		public bool Using = false;
		//public string InstanceID;

		public static explicit operator ModifierDataUsed(SkillModifierData data)
		{
			return new ModifierDataUsed
			{
				Modifier = data
			};
		}

		public static explicit operator SkillModifierData(ModifierDataUsed data)
		{
			return data.Modifier;
		}
	}

	[SerializeField]
	private SkillData[] _avaiableSkills;

	[SerializeField]
	private List<SkillModifierData> _allModifiers;

	[Header("Skill List")]
	[SerializeField]
	private Transform _skillsContainer;
	[SerializeField]
	private GameObject _skillItemPrefab;

	[Header("Inventory List")]
	[SerializeField]
	private GameObject _inventoryPanel;
	[SerializeField]
	private Transform _inventoryContainer;
	[SerializeField]
	private GameObject _inventoryItemPrefab;

	[SerializeField]
	private TMPro.TMP_Text LogText;

	private readonly List<ModifierDataUsed> _avaiableModifiers = new List<ModifierDataUsed>();

	public void BackToShop()
	{
		SceneManager.LoadScene(Settings.ShopScene);
	}

	public void GoToGame()
	{
		List<SkillItem> skills = new List<SkillItem>();
		foreach (Transform skillTransform in _skillsContainer.GetComponentInChildren<Transform>())
		{
			SkillItem skillItem = skillTransform.GetComponent<SkillItem>();
			if (skillItem.Use.isOn)
			{
				skills.Add(skillItem);
			}
		}

		if (skills.Count == 0 || skills.Count > Settings.MaxSkills)
		{
			LogText.text = $"Select at least 1 skill and less than {Settings.MaxSkills + 1}";
			return;
		}

		Player.Instance.Skills = skills.Select(x => x.SkillData).ToArray();
		Debug.Log(Player.Instance.Skills);
		SceneManager.LoadScene(Settings.MainMenuScene);
	}

	private void OnEnable()
	{
		Utility.IsEmptyObject(_skillsContainer);
		Utility.IsEmptyObject(_skillItemPrefab);

		// Test guide: 
		// In every Skill, player needs to choose modifier to fill the modifier slot. The available
		// modifier will be based on what the player owns in their inventory,
		//
		// The player NEEDS to choose a modifier? So, what happens if he didn't buy any modifier? 
		// I let it as optional: he choose either to use or not a modifier.
		Utility.MockLogin((result) =>
		{
			StartCoroutine(Utility.IsUserLoggedIn());
			LoadSkills();
			LoadInventory();
		});
	}

	private void LoadSkills()
	{
		for (int skillIndex = 0; skillIndex < _avaiableSkills.Length; skillIndex++)
		{
			GameObject item = Instantiate(_skillItemPrefab);
			item.transform.SetParent(_skillsContainer);

			SkillData skill = _avaiableSkills[skillIndex];
			item.GetComponent<SkillItem>().Initialize(skill, OpenInventory);
		}
	}

	private void OpenInventory(SkillItem skill, SkillModifierData skillModifierData)
	{
		_inventoryPanel.SetActive(true);

		SkillModifierData lastModifier =
			(skillModifierData && !string.IsNullOrEmpty(skillModifierData.ID))
			? skillModifierData
			: null;

		if (lastModifier)
		{
			_avaiableModifiers.First(x => x.Modifier.ID == lastModifier.ID).Using = false;
		}

		Utility.DestroyAllChildren(_inventoryContainer.transform);

		// TODO: Change to populate and don't clear.
		Transform item = Instantiate(_inventoryItemPrefab).transform;
		item.name = "None";
		item.GetComponent<ModifierItem>().Initialize(skill, new SkillModifierData
		{
			Name = "None"
		}, CloseInventory);
		item.SetParent(_inventoryContainer);

		foreach (ModifierDataUsed modifier in _avaiableModifiers.Where(x => !x.Using).ToList())
		{
			item = Instantiate(_inventoryItemPrefab).transform;

			item.name = modifier.Modifier.Name;
			item.GetComponent<ModifierItem>().Initialize(skill, modifier.Modifier, CloseInventory);
			item.SetParent(_inventoryContainer);
		}
	}

	private void CloseInventory(SkillItem skill, SkillModifierData modifierData)
	{
		skill.SkillData.Modifier = modifierData;
		skill.GetComponent<ModifierItem>().Initialize(skill, modifierData, null);

		// TODO: Remove this after change to populate and don't clear.
		if (!string.IsNullOrEmpty(modifierData.ID))
		{
			_avaiableModifiers.Find(x => x.Modifier.ID == modifierData.ID).Using = true;
		}

		_inventoryPanel.SetActive(false);
	}

	private void LoadInventory()
	{
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
		{
			for (int index = 0; index < result.Inventory.Count; index++)
			{
				ItemInstance item = result.Inventory[index];
				SkillModifierData modifier = _allModifiers.First(x => x.ID == item.ItemId);
				if (modifier)
				{
					_avaiableModifiers.Add((ModifierDataUsed)modifier);
				}
				else
				{
					Debug.LogError($"Modifier {modifier.ID} not registered.");
				}
			}
		}, null);
	}
}
