
public interface IFighter
{
	int AP { get; set; }
	int MaxAP { get; }
	int APRegenPerTurn { get; }

	// Could be an array to select a target, if we have more enemies.
	void ChooseSkill();
	void UseSkill(Character target, SkillData skill);
}
