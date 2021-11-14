
public interface IDestroyable
{
	int Health { get; set; }
	int MaxHealth { get; }
	bool IsAlive { get; }

	int HPRegenPerTurn { get; }

	event DeadEvent OnDead;

	void ReceiveDamage(Damage damage);
}
