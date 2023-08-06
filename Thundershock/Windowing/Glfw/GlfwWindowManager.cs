#nullable enable

using System.Data;
using System.Diagnostics;
using Silk.NET.Core.Native;
using Silk.NET.GLFW;

namespace Thundershock.Windowing.Glfw;

public class GlfwWindowManager : WindowManager<GlfwWindow>
{
	private Silk.NET.GLFW.Glfw glfw;

	internal ref Silk.NET.GLFW.Glfw Glfw => ref glfw;
	
	public GlfwWindowManager()
	{
		this.glfw = Silk.NET.GLFW.Glfw.GetApi();
	}

	/// <inheritdoc />
	protected override GlfwWindow GetNewWindow()
	{
		return new GlfwWindow(this);
	}

	/// <inheritdoc />
	protected override void OnInitialize()
	{
		if (!this.glfw.Init())
			throw new ApplicationException("GLFW failed to initialize");
		
		ThrowOnError();
	}

	/// <inheritdoc />
	protected override void OnShutdown()
	{
		this.glfw.Dispose();
	}

	/// <inheritdoc />
	protected override void OnUpdate()
	{
		glfw.PollEvents();
	}

	private void ThrowOnError()
	{
		ErrorCode err;
		string? description;

		unsafe
		{
			err = glfw.GetError(out byte* descriptionPtr);
			description = SilkMarshal.PtrToString((IntPtr) descriptionPtr);
		}

		if (err == ErrorCode.NoError)
			return;

		throw new ApplicationException($"GLFW error {err}: {description}");
	}
}