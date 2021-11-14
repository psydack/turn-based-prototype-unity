using UnityEngine;

public class Player : MonoBehaviour
{
	// TODO: Turn private with read only for editr
	[SerializeField]
	public SkillData[] Skills;

	private static Player _instance;

	public static Player Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = new GameObject();
				go.AddComponent<DontDestroy>();
				go.name = "Player";
				_instance = go.AddComponent<Player>();
			}
			return _instance;
		}
	}
}
