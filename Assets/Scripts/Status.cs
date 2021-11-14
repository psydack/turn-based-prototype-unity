public struct Status
{
	public StatusEffect StatusEffect;
	public int TurnCount;

	public int ConsumeTurn()
	{
		return TurnCount--;
	}

	public StatusEffect ChangeStatus(StatusEffect status)
	{
		return StatusEffect = status;
	}
}
