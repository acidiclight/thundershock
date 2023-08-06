namespace Thundershock.Player;

internal class PlayerApplication    : Application
{
	/// <inheritdoc />
	protected override void Initialize()
	{
		
	}

	/// <inheritdoc />
	protected override void Update()
	{
		Console.WriteLine($"Time since engine start: {Clock.TotalTime}");
		Console.WriteLine($"Frrame time: {Clock.DeltaTime} seconds");

		Console.Write("Enter some text >>> ");
		string? text = Console.ReadLine();
		Console.WriteLine(text);
		
	}

	/// <inheritdoc />
	protected override void Shutdown()
	{
		
	}
}