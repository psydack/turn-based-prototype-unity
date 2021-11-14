
public interface IStatusVunerable
{
	Status CurrentStatus { get; }

	void ApplyStatus();
}
