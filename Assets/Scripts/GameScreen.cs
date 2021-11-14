using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
	private enum Winner
	{
		Player,
		Enemy
	};

	[SerializeField]
	private GameObject _endGamePanel;

	// We could have a SeiralizebleObject to create enemies 
	// That way we could balance easier
	[Header("Skills and Modifiers for Enemy")]
	[SerializeField]
	private List<SkillData> _avaiableSkills;
	[SerializeField]
	private List<SkillModifierData> _avaiableModifiers;

	private int _turn;
	private Character PlayerCharacter = new PlayerCharacter();
	private Character Enemy = new SimpleEnemyCharacter();

	private void OnEnable()
	{
		Utility.IsEmptyObject(_endGamePanel);
	}

	private void Initialize()
	{
		PlayerCharacter.Initialize(Player.Instance.Skills);
		InitializeEnemy();

		PlayerCharacter.OnEndTurn += ChangeTurn;
		Enemy.OnEndTurn += ChangeTurn;

		PlayerCharacter.OnDead += EndGame;
		Enemy.OnDead += EndGame;

		_turn = Random.Range(0, 1);
		ChangeTurn();
	}

	private void EndGame()
	{
		Debug.Log("<color=green>End game</color>");

		Winner winner = PlayerCharacter.Health > 0 ? Winner.Player : Winner.Enemy;
		ShowWinnerPanel(winner);
	}

	private void ChangeTurn()
	{
		if (_turn++ % 2 == 0)
		{
			PlayerCharacter.Turn(Enemy);
		}
		else
		{
			Enemy.Turn(PlayerCharacter);
		}
	}

	private void ShowWinnerPanel(Winner winner)
	{
		_endGamePanel.SetActive(true);
	}

	private void InitializeEnemy()
	{
		List<SkillData> shuffledSkills = _avaiableSkills;
		shuffledSkills.Shuffle();

		List<SkillModifierData> shuffledModifiers = _avaiableModifiers;
		shuffledModifiers.Shuffle();

		int index = 0;
		IEnumerable<SkillData> selectedSkills = shuffledSkills.Take(4);
		foreach (SkillData skill in selectedSkills)
		{
			skill.Modifier = shuffledModifiers[index];
			index++;
		}
	}
}