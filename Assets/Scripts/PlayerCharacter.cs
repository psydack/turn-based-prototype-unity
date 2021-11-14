using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	[SerializeField]
	private GameObject _skillsPanel;

	[SerializeField]
	private Transform _skillsContainer;

	[SerializeField]
	private GameObject _skillPrefab;

	private List<SkillItem> _avaiableSkills = new List<SkillItem>();

	public override void Initialize(SkillData[] skills)
	{
		base.Initialize(skills);
		PopulateSkillsPanel();
	}

	public override void ChooseSkill()
	{
		// TODO: Animation, etc.
		int countAvaiableSkills = 0;
		_avaiableSkills.ForEach(item =>
		{
			item.SelectModifierButton.gameObject.SetActive(AP >= item.SkillData.AttributesModified.APCost);

			if (item.SelectModifierButton.gameObject.activeSelf)
			{
				countAvaiableSkills++;
			}
		});

		if (countAvaiableSkills == 0)
		{
			EndTurn();
			return;
		}

		_skillsPanel.SetActive(true);
	}

	private void OnSelectedSkill(SkillItem skillItem, SkillModifierData modifier)
	{
		// TODO: Animation, etc.
		_skillsPanel.SetActive(false);
		UseSkill(_target, skillItem.SkillData);
	}

	private void PopulateSkillsPanel()
	{
		for (int skillIndex = 0; skillIndex < Skills.Length; skillIndex++)
		{
			GameObject skillGo = Instantiate(_skillPrefab);
			SkillItem skillItem = skillGo.GetComponent<SkillItem>();

			_avaiableSkills.Add(skillItem);
			skillItem.Initialize(Skills[skillIndex], OnSelectedSkill);
			skillGo.transform.SetParent(_skillsContainer);
		}
	}
}
