using UnityEngine;

public delegate void EndTurnEvent();
public delegate void StartTurnEvent(Character target);
public delegate void DeadEvent();

// I do use the c# layout guideline.
// fields => properties => construct => event => methods
// public => protected => internal => private  
public abstract class Character : MonoBehaviour, ICharacter, IStatusVunerable, IFighter, IDestroyable
{
	protected Character _target; // each turn save on this, all avaiable targets

	private int _health;
	private int _ap;

	public virtual int APRegenPerTurn => 5;
	public virtual int HPRegenPerTurn => 5;
	public virtual int MaxAP => 150;
	public virtual int MaxHealth => 500;

	public Status CurrentStatus => new Status();
	public SkillData[] Skills { get; set; }
	public SkillData LastSkill { get; set; }

	public int AP
	{
		get
		{
			return _ap;
		}
		set
		{
			_ap = _ap > MaxAP ? MaxAP : value;
		}
	}
	public int Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = _health > MaxHealth ? MaxHealth : value;
			if (!IsAlive)
			{
				OnDead?.Invoke();
			}
		}
	}

	public bool IsAlive => Health > 0;
	public bool FinishedTurn { get; set; }

	#region Events
	public event StartTurnEvent OnStartTurn;
	public event EndTurnEvent OnEndTurn;
	public event DeadEvent OnDead;
	#endregion

	// Could be Character[] targets.
	public abstract void ChooseSkill();

	protected void EndTurn()
	{
		OnEndTurn?.Invoke();
	}

	public virtual void Initialize(SkillData[] skills)
	{
		OnStartTurn += Turn;
		Skills = skills;
	}

	// Could be Character[] targets for spread damage.
	public virtual void UseSkill(Character target, SkillData skill)
	{
		LastSkill = skill;
		StatusEffect status = CalculateStatusEffect(skill);

		int totalDamage = skill.AttributesModified.Damage;
		if (skill.AttributesModified.CriticalHitChance > 0)
		{
			if (skill.AttributesModified.CriticalHitChance >= 100 ||
				Random.Range(0f, 100f) <= skill.AttributesModified.CriticalHitChance)
			{
				totalDamage *= 2;
			}
		}

		target.ReceiveDamage(new Damage(totalDamage, status));
		OnEndTurn();
	}

	public virtual void ReceiveDamage(Damage damage)
	{
		if (damage.DamageType != StatusEffect.None)
		{
			CurrentStatus.ChangeStatus(damage.DamageType);
		}

		// Could be by type or something else;
		if (LastSkill.Name == "Defense")
		{
			Health -= damage.Value >> 1;
		}
		else
		{
			Health -= damage.Value;
		}
	}

	public virtual void Turn(Character target)
	{
		AP += APRegenPerTurn;
		Health += HPRegenPerTurn;

		ApplyStatus();
		if (FinishedTurn)
		{
			OnEndTurn?.Invoke();
			return;
		}

		ChooseSkill();
	}

	public void ApplyStatus()
	{
		if (CurrentStatus.ConsumeTurn() < 0) return;

		switch (CurrentStatus.StatusEffect)
		{
			case StatusEffect.Stuned:
				FinishedTurn = true;
				break;
			case StatusEffect.Poisoned:
				// It's fixed damage and turns. But we could make object for diferent poison
				// that way we could control damage
				ReceiveDamage(new Damage(15));
				break;
		}
	}

	private StatusEffect CalculateStatusEffect(SkillData skill)
	{
		StatusEffect status = StatusEffect.None;
		float stunChance = skill.AttributesModified.StunChance;
		float poisonChance = skill.AttributesModified.PoisonChance;

		if (stunChance == 0 && poisonChance == 0) return status;

		// No deterministic. We would to store a seed.
		int random = Random.Range(1, 100);

		// Simple way. We could put some fancy logic here.
		if (stunChance >= 100 || random <= stunChance)
		{
			status = StatusEffect.Stuned;
		}
		else if (poisonChance >= 100 || random <= poisonChance)
		{
			status = StatusEffect.Poisoned;
		}

		return status;
	}
}
