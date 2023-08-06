namespace Thundershock;

public abstract class Application : IDisposable
{
	private static Application? currentInstance;

	private bool exitRequested;
	
	protected Application()
	{
		// Prevents client applications from instantiating the engine more than once at a time. Doing so
		// causes headaches I don't want to deal with.
		//
		// This probably makes this the only singleton in the entire engine.
		if (currentInstance is not null)
			throw new ApplicationException($"Instantiating two instances of the {nameof(Application)} class is not supported. You must dispose of the previous application instance before creating a new one.");

		currentInstance = this;
	}

	/// <summary>
	///		Runs the application.
	/// </summary>
	public void Run()
	{
		Clock.Init();
		
		Initialize();

		RunLoop();

		Shutdown();
	}

	private void RunLoop()
	{
		while (!exitRequested)
		{
			RunOneUpdate();
		}
	}

	private void RunOneUpdate()
	{
		Clock.Elapse();
		Update();
	}
	
	private void ExitInternal()
	{
		exitRequested = true;
	}

	protected abstract void Update();
	protected abstract void Initialize();
	protected abstract void Shutdown();

	/// <inheritdoc />
	public virtual void Dispose()
	{
		// Everything's been torn down, we can let other apps start now.
		currentInstance = null;
	}

	/// <summary>
	///		Exits the current application.
	/// </summary>
	public static void Exit()
	{
		if (currentInstance is null)
			throw new ApplicationException("Application is not running.");

		currentInstance?.ExitInternal();
	}
}