namespace Thundershock;

public class ModuleManager : IEngineModule
{
	private bool isInitialized;
	private readonly List<IEngineModule> activeModules = new List<IEngineModule>();

	public T AddModule<T>() where T : IEngineModule, new()
	{
		ThrowIfInitialized();

		var module = new T();
		this.activeModules.Add(module);

		return module;
	}

	public void AddModule(IEngineModule module)
	{
		ThrowIfInitialized();
		
		Log.PushCategory(nameof(ModuleManager));
		activeModules.Add(module);

		Log.Message($"Registered engine module: {module.GetType().FullName}");
		
		Log.PopCategory();
	}
	
	/// <inheritdoc />
	public void Initialize()
	{
		ThrowIfInitialized();
		Log.PushCategory(nameof(ModuleManager));
		Log.Message("Initializing engine modules...");
		
		isInitialized = true;

		foreach (IEngineModule module in activeModules)
		{
			Log.PushCategory(module.GetType().Name);
			Log.Message("Initializing...");
			module.Initialize();
			Log.Message("...done");
			Log.PopCategory();
		}

		Log.Message("All modules initialized!");
		Log.PopCategory();
	}

	/// <inheritdoc />
	public void RunOneUpdate()
	{
		foreach (IEngineModule module in activeModules)
			module.RunOneUpdate();
	}

	/// <inheritdoc />
	public void Shutdown()
	{
		Log.PushCategory(nameof(ModuleManager));
		Log.Message("Shutting down engine modules...");

		foreach (IEngineModule module in activeModules)
		{
			Log.PushCategory(module.GetType().Name);
			Log.Message("Shutting down...");
			module.Shutdown();
			Log.Message("..done");
			Log.PopCategory();
		}

		activeModules.Clear();
		Log.Message("Done shutting modules down!");
		Log.PopCategory();
	}

	private void ThrowIfInitialized()
	{
		if (isInitialized)
			throw new InvalidOperationException("Cannot perform this operation when the ModuleManager has already been initialized.");
	}
}