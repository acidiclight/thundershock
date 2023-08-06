using System.Diagnostics;

namespace Thundershock;

/// <summary>
///		Provides and tracks the engine's uptime and frame time statistics.
/// </summary>
public static class Clock
{
	/// <summary>
	///		Gets the amount of time, in seconds, spent running the last frame.
	/// </summary>
	public static float DeltaTime { get; private set; }

	/// <summary>
	///		Gets the total amount of time spent running the game.
	/// </summary>
	public static TimeSpan TotalTime { get; private set; }

	private static readonly Stopwatch stopwatch = new Stopwatch();
	
	/// <summary>
	///		Resets all clock values to zero.
	/// </summary>
	internal static void Init()
	{
		TotalTime = TimeSpan.Zero;
		DeltaTime = 0;
	}

	/// <summary>
	///		Notifies that a frame has ended, updating the clock.
	/// </summary>
	internal static void Elapse()
	{
		// If the stopwatch is running, we stop it, track the time elapsed, and reset it.
		if (stopwatch.IsRunning)
		{
			stopwatch.Stop();
			
			TimeSpan elapsed = stopwatch.Elapsed;

			stopwatch.Reset();
			
			DeltaTime = (float) elapsed.TotalSeconds;

			TotalTime = TotalTime + elapsed;
		}

		// Starts counting time for the current frame.
		stopwatch.Start();
	}
}