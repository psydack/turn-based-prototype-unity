using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
	public TMP_Text NameText;

	public TMP_Text DamageText;
	public TMP_Text ApCostText;
	public TMP_Text StunChanceText;
	public TMP_Text PoisonChanceText;
	public TMP_Text CriticalHitChanceText;
	public Button SelectModifierButton;

	public Toggle Use;

	public SkillData SkillData { get; set; }
	//public SkillModifierData SkillModifier { get; set; }

	public void Initialize(SkillData skill, System.Action<SkillItem, SkillModifierData> onClickButton)
	{
		SkillData = skill;
		NameText.text = skill.Name;
		DamageText.text = $"Damage: {skill.Attributes.Damage}";
		ApCostText.text = $"APCost: {skill.Attributes.APCost}";
		StunChanceText.text = $"StunChance: {skill.Attributes.StunChance}";
		PoisonChanceText.text = $"PoisonChance: {skill.Attributes.PoisonChance}";
		CriticalHitChanceText.text = $"CriticalHitChance: {skill.Attributes.CriticalHitChance}";

		// TODO: polling.
		//SelectModifierButton.onClick.RemoveAllListeners();
		SelectModifierButton.onClick.AddListener(() => onClickButton?.Invoke(this, SkillData.Modifier));
	}
}
