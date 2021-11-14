using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifierItem : MonoBehaviour
{
	public TMP_Text ModfierName;
	public TMP_Text DamageModText;
	public TMP_Text ApCostModText;
	public TMP_Text StunModChanceText;
	public TMP_Text PoisonModChanceText;
	public TMP_Text CriticalHitModChanceText;

	public Button UseButton;

	public SkillModifierData SkillModifier { get; private set; }

	public void Initialize(SkillItem skill, SkillModifierData modifierData, System.Action<SkillItem, SkillModifierData> onSelect)
	{
		SkillModifier = modifierData;
		const string format = "{0:+#;-#;+0}";
		SkillAttributes attr = SkillModifier.AttributesModifier;

		ModfierName.text = SkillModifier.Name;
		DamageModText.text = string.Format(format, attr.Damage);
		ApCostModText.text = string.Format(format, attr.APCost);
		StunModChanceText.text = string.Format(format, attr.StunChance);
		PoisonModChanceText.text = string.Format(format, attr.PoisonChance);
		CriticalHitModChanceText.text = string.Format(format, attr.CriticalHitChance);

		UseButton?.onClick.AddListener(() => onSelect?.Invoke(skill, SkillModifier));
	}
}