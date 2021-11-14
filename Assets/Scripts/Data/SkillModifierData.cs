using UnityEngine;

[CreateAssetMenu(fileName = "TMP_Modifier.asset", menuName = "Skill/Create modifier", order = 2)]
public class SkillModifierData : ScriptableObject
{
	public string ID = string.Empty;
	public string Name = string.Empty;

	public SkillAttributes AttributesModifier;

	[TextArea(3, 5)]
	public string ShortDescription = string.Empty;

	[TextArea(5, 10)]
	public string FullDescription = string.Empty;
}