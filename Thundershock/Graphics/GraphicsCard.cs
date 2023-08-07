#nullable enable

using System.Numerics;

namespace Thundershock.Graphics;

public abstract class GraphicsCard
{
	private static GraphicsCard? activeGraphicsCard;

	public static GraphicsCard? Active => activeGraphicsCard;

	public bool IsActive => this == activeGraphicsCard;

	/// <summary>
	///		Called when the graphics card is activated.
	/// </summary>
	protected abstract void OnActivate();
	
	/// <summary>
	///		Called when the graphics card is deactivated.
	/// </summary>
	protected abstract void OnDeactivate();
	
	/// <summary>
	///		Clears the screen to the specified color.
	/// </summary>
	/// <param name="clearColor"></param>
	public abstract void Clear(Vector3 clearColor);
	
	/// <summary>
	///		Instructs the graphics card to present the current frame to its display.
	/// </summary>
	public abstract void Present();
	
	/// <summary>
	///		Activates this graphics card, allowing the engine to render to it.
	/// </summary>
	/// <exception cref="InvalidOperationException">Another graphics card is already active.</exception>
	public void Activate()
	{
		if (IsActive)
			return;

		if (activeGraphicsCard != null)
			throw new InvalidOperationException("Cannot activate multiple graphics cards at the same time.");

		activeGraphicsCard = this;

		this.OnActivate();
	}

	/// <summary>
	///		Deactivates this graphics card, stopping the engine from rendering to it.
	/// </summary>
	public void Deactivate()
	{
		if (!IsActive)
			return;

		activeGraphicsCard = null;
		this.OnDeactivate();
	}
}