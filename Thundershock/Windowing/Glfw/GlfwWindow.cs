using Silk.NET.Core.Native;
using Silk.NET.GLFW;
using Silk.NET.WebGPU;
using Thundershock.Graphics.WebGpuGraphics;

namespace Thundershock.Windowing.Glfw;

public class GlfwWindow : 
	IWindow
{
	private readonly GlfwWindowManager wm;
	private unsafe WindowHandle* handle;
	private string? title;
	private int width = 300;
	private int height = 300;
	private INativeWindow? nativeWindow;
	
	internal unsafe GlfwWindow(in GlfwWindowManager wm)
	{
		this.wm = wm;
	}

	/// <inheritdoc />
	public INativeWindow? NativeWindow => nativeWindow;
	
	/// <inheritdoc />
	public bool IsOpen
	{
		get
		{
			unsafe
			{
				return handle != null;
			}
		}
	}
	
	/// <inheritdoc />
	public string? Title
	{
		get => title;
		set
		{
			title = value;

			unsafe
			{
				if (handle != null)
				{
					wm.Glfw.SetWindowTitle(handle, this.title ?? "GLFW Window");
				}
			}
		}
	}

	/// <inheritdoc />
	public int Width
	{
		get
		{
			unsafe
			{
				if (handle != null)
				{
					wm.Glfw.GetWindowSize(handle, out width, out height);
				}
			}

			return width;
		}
		set
		{
			if (value == width)
				return;

			width = value;

			unsafe
			{
				if (handle != null)
				{
					wm.Glfw.SetWindowSize(handle, width, height);
				}
			}
		}
	}

	/// <inheritdoc />
	public int Height
	{
		get
		{
			unsafe
			{
				if (handle != null)
				{
					wm.Glfw.GetWindowSize(handle, out width, out height);
				}
			}

			return height;
		}
		set
		{
			if (value == height)
				return;

			height = value;

			unsafe
			{
				if (handle != null)
				{
					wm.Glfw.SetWindowSize(handle, width, height);
				}
			}
		}
	}
	
	/// <inheritdoc />
	public void Open()
	{
		unsafe
		{
			handle = wm.Glfw.CreateWindow(Width, Height, "GLFW Window", null, null);
			ThrowOnError();
			wm.Glfw.ShowWindow(handle);

			wm.Glfw.SetWindowCloseCallback(handle, CloseCallback);

			this.nativeWindow = new NativeGlfwWindow(wm.Glfw, this.handle);
		}
	}

	/// <inheritdoc />
	public void Close()
	{
		unsafe
		{
			nativeWindow = null;
			wm.Glfw.DestroyWindow(handle);
			ThrowOnError();
			handle = null;
		}
	}
	
	private void ThrowOnError()
	{
		ErrorCode err;
		string? description;

		unsafe
		{
			err = this.wm.Glfw.GetError(out byte* descriptionPtr);
			description = SilkMarshal.PtrToString((IntPtr) descriptionPtr);
		}

		if (err == ErrorCode.NoError)
			return;

		throw new ApplicationException($"GLFW error {err}: {description}");
	}

	private unsafe void CloseCallback(WindowHandle* win)
	{
		if (win != handle)
			return;

		Close();
	}
}