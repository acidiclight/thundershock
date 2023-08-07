using System.Reflection;
using System.Text;
using Thundershock.Rendering;
using Thundershock.Windowing;

namespace Thundershock;

public abstract class Application :
	IDisposable 
{
	private static Application? currentInstance;

	private readonly StringBuilder logStringBuilder = new StringBuilder();
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
		Log.OnMessageLogged += HandleApplicationLogs;
		Log.Message("Logging has been initialized.");
		
		Assembly appAssembly = this.GetType().Assembly;

		ApplicationInfo.CompanyName = appAssembly.GetCustomAttributes(false)
			.OfType<AssemblyCompanyAttribute>()
			.First()
			.Company;

		ApplicationInfo.ProductName = appAssembly.GetCustomAttributes(false)
			.OfType<AssemblyProductAttribute>()
			.First()
			.Product;
		
		Log.Message($"Application name: {ApplicationInfo.ProductName}");
		Log.Message($"Developer: {ApplicationInfo.CompanyName}");
		Log.Message($"Persistent data path: {ApplicationInfo.PersistentDataPath}");
		Log.Message($"Base directory: {ApplicationInfo.BaseDirectory}");

		Clock.Init();
		
		Initialize();

		RunLoop();

		Shutdown();
		
		Log.OnMessageLogged -= HandleApplicationLogs;
	}

	private void HandleApplicationLogs(in Log.LogMessage message)
	{
		// TODO: We really need a better logging system than this. 
		logStringBuilder.Length = 0;

		logStringBuilder.Append("[");
		logStringBuilder.Append(message.TimeStamp);
		logStringBuilder.Append("] <");
		logStringBuilder.Append(message.Category);
		logStringBuilder.Append("> ");
		logStringBuilder.Append(message.Text);
		
		Console.WriteLine(logStringBuilder);
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
		moduleManager.AddModule<RenderModule>();

		RegisterModules(moduleManager);
		
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

	protected virtual void RegisterModules(ModuleManager moduleManager)
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