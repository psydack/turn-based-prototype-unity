

public interface ICharacter
{
	SkillData[] Skills { get; set; }

	bool FinishedTurn { get; set; }

	SkillData LastSkill { get; set; }

	event StartTurnEvent OnStartTurn;
	event EndTurnEvent OnEndTurn;

	void Initialize(SkillData[] skills);

	void Turn(Character target);
}
