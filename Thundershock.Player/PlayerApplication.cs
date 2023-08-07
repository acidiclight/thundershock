using Thundershock.Graphics;
using Thundershock.Graphics.WebGpuGraphics;
using Thundershock.Windowing;
using Thundershock.Windowing.Glfw;

namespace Thundershock.Player;

internal class PlayerApplication    : Application
{
	private GlfwWindowManager wm = new GlfwWindowManager();
	private GlfwWindow? mainWindow;
	private GraphicsCard? graphicsCard;
	
	/// <inheritdoc />
	protected override void OnInitialize()
	{
		// Find the game data image. A properly packaged game will have it in a well-known location.
		string productName = ApplicationInfo.ProductName;
		string imageLocation = Path.Combine(ApplicationInfo.BaseDirectory, productName + ".tsimage");
		
		// Check that it exists. It must exist for the player to start.
		if (!File.Exists(imageLocation))
		{
			Log.Message($"Missing game data in {imageLocation}. Player will shut down.");
			DialogBox.Message(
				$"{productName} - Missing game data!",
				$@"Could not start {productName} because the game data expected at {imageLocation} is missing.

Please ensure that {productName} was installed correctly. {productName} will now exit."
			);
			
			return;
		}

		// Registers the GLFW window manager with the engine.
		this.wm = this.WindowingModule.CreateWindowManager<GlfwWindowManager, GlfwWindow>();
		
		// Create the main game window.
		mainWindow = wm.CreateWindow("Thundershock Player");
		
		// We can now create a graphics card from the window.
		graphicsCard = new WebGpuGraphicsCard(mainWindow);
		
		// Activate the graphics card for rendering.
		graphicsCard.Activate();
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