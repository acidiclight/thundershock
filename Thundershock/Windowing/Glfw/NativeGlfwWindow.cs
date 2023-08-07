using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;

namespace Thundershock.Windowing.Glfw;

public sealed unsafe class NativeGlfwWindow : INativeWindow
{
	private readonly Silk.NET.GLFW.Glfw glfw;
	private readonly WindowHandle* handle;
	private readonly GlfwNativeWindow nativeHandle;

	public NativeGlfwWindow(Silk.NET.GLFW.Glfw glfw, WindowHandle* handle)
	{
		this.glfw = glfw;
		this.handle = handle;
		this.nativeHandle = new GlfwNativeWindow(glfw, handle);
	}

	public NativeWindowInfo WindowInfo => GetNativeWindowInfo();

	private NativeWindowInfo GetNativeWindowInfo()
	{
		var result = new NativeWindowInfo();

		// Windows
		if (this.nativeHandle.Win32 != null)
		{
			result.WindowSystem = NativeWindowSystem.Win32;

			result.Win32.HINSTANCE = nativeHandle.Win32.Value.HInstance;
			result.Win32.HWND = nativeHandle.Win32.Value.Hwnd;
		}
		
		// X11/Xorg
		else if (nativeHandle.X11 != null)
		{
			result.WindowSystem = NativeWindowSystem.X11;

			result.X11.Display = nativeHandle.X11.Value.Display;
			result.X11.Window = (uint) nativeHandle.X11.Value.Window;
		}
		
		// Wayland
		else if (nativeHandle.Wayland != null)
		{
			result.WindowSystem = NativeWindowSystem.Wayland;

			result.Wayland.Display = nativeHandle.Wayland.Value.Display;
			result.Wayland.Surface = nativeHandle.Wayland.Value.Surface;
		}

		// Not supported.
		else
		{
			result.WindowSystem = NativeWindowSystem.Other;
		}
		
		return result;
	}
}