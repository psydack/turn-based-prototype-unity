public class Damage
{
	public readonly int Value;
	public readonly StatusEffect DamageType;

	public Damage(int value, StatusEffect damageType = StatusEffect.None)
	{
		Value = value;
		DamageType = damageType;
	}
}
