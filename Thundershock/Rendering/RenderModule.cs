#nullable enable

using Thundershock.Graphics;

namespace Thundershock.Rendering;

public class RenderModule : IEngineModule
{
	/// <inheritdoc />
	public void Initialize()
	{
		
	}

	/// <inheritdoc />
	public void RunOneUpdate()
	{
		if (GraphicsCard.Active == null)
			return;
		
		if (Camera.EnabledCameraCount == 0)
			return;

		for (var i = 0; i < Camera.EnabledCameraCount;  i++)
		{
			var cam = Camera.GetEnabledCamera(i);
			cam.Render();
		}

		GraphicsCard.Active.Present();
	}

	/// <inheritdoc />
	public void Shutdown()
	{
	}
}