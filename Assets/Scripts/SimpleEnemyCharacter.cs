using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleEnemyCharacter : Character
{
	private IEnumerable<SkillData> _avaiableSkills => Skills
		.ToList()
		.Where(x => AP >= x.AttributesModified.APCost);

	public override void ChooseSkill()
	{
		if (AP <= 0 ||
			AP <= Skills.Min(x => x.AttributesModified.APCost))
		{
			EndTurn();
			return;
		}

		int randomIndex = Random.Range(0, _avaiableSkills.Count() - 1);
		SkillData skillData = _avaiableSkills.ElementAt(randomIndex);
		UseSkill(_target, skillData);
	}
}