using UnityEngine;

[CreateAssetMenu(fileName = "TMP_Skill.asset", menuName = "Skill/Create skill", order = 1)]
public class SkillData : ScriptableObject
{
	public string Name;

	public SkillAttributes Attributes;

	public SkillModifierData Modifier;

	public SkillAttributes AttributesModified =>
		new SkillAttributes
		{
			APCost = Attributes.APCost + (Modifier?.AttributesModifier.APCost ?? 0),
			Damage = Attributes.Damage + (Modifier?.AttributesModifier.Damage ?? 0),
			CriticalHitChance = Attributes.CriticalHitChance + (Modifier?.AttributesModifier.CriticalHitChance ?? 0),
			PoisonChance = Attributes.PoisonChance + (Modifier?.AttributesModifier.PoisonChance ?? 0),
			StunChance = Attributes.StunChance + (Modifier?.AttributesModifier.StunChance ?? 0)
		};
}
