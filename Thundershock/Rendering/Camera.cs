using Thundershock.Graphics;

namespace Thundershock.Rendering;

public class Camera : Component
{
	private static readonly List<Camera> enabledCameras = new List<Camera>();

	/// <inheritdoc />
	protected override void OnEnable()
	{
		enabledCameras.Add(this);
	}

	/// <inheritdoc />
	protected override void OnDisable()
	{
		enabledCameras.Remove(this);
	}

	public void Render()
	{
		if (GraphicsCard.Active == null)
			return;
		
		
	}

	public static int EnabledCameraCount => enabledCameras.Count;

	public static Camera GetEnabledCamera(int index)
	{
		return enabledCameras[index];
	}
}