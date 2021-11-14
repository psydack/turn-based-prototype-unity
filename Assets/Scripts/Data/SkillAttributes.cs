using System;
using UnityEngine;

[Serializable]
public struct SkillAttributes
{
	public int Damage;
	public int APCost;

	[Range(-100f, 100f)]
	public float StunChance;
	[Range(-100f, 100f)]
	public float PoisonChance;
	[Range(-100f, 100f)]
	public float CriticalHitChance;
}