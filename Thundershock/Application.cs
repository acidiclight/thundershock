using Thundershock.Windowing;

namespace Thundershock;

public abstract class Application :
	IDisposable 
{
	private static Application? currentInstance;

	private ModuleManager moduleManager;
	private WindowingModule windowingModule;
	private bool exitRequested;

	protected ModuleManager ModuleManager => moduleManager;
	protected WindowingModule WindowingModule => windowingModule;
	
	protected Application()
	{
		// Prevents client applications from instantiating the engine more than once at a time. Doing so
		// causes headaches I don't want to deal with.
		//
		// This probably makes this the only singleton in the entire engine.
		if (currentInstance is not null)
			throw new ApplicationException($"Instantiating two instances of the {nameof(Application)} class is not supported. You must dispose of the previous application instance before creating a new one.");

		currentInstance = this;

		this.moduleManager = new ModuleManager();
		this.windowingModule = new WindowingModule();
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

	public void RunOneUpdate()
	{
		Clock.Elapse();
		OnUpdate();

		this.moduleManager.RunOneUpdate();
	}
	
	private void ExitInternal()
	{
		exitRequested = true;
	}

	private void Initialize()
	{
		moduleManager.AddModule(windowingModule);

		moduleManager.Initialize();
		OnInitialize();
	}

	private void Shutdown()
	{
		moduleManager.Shutdown();
		OnShutdown();
	}

	protected virtual void OnUpdate()
	{
		
	}

	protected virtual void OnInitialize()
	{
		
	}

	protected virtual void OnShutdown()
	{
		
	}

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