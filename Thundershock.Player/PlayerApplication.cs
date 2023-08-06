using Thundershock.Windowing;
using Thundershock.Windowing.Glfw;

namespace Thundershock.Player;

internal class PlayerApplication    : Application
{
	private GlfwWindowManager wm = new GlfwWindowManager();
	private IWindow? mainWindow;
	
	/// <inheritdoc />
	protected override void OnInitialize()
	{
		// Registers the GLFW window manager with the engine.
		this.wm = this.WindowingModule.CreateWindowManager<GlfwWindowManager, GlfwWindow>();
		
		// Create the main game window.
		mainWindow = wm.CreateWindow("Thundershock Player");
	}

	/// <inheritdoc />
	protected override void OnUpdate()
	{
		if (mainWindow?.IsOpen != true)
			Exit();
	}

	/// <inheritdoc />
	protected override void OnShutdown()
	{
		
	}
}