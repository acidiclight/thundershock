namespace Thundershock.Windowing;

public struct NativeWindowInfo
{
	public NativeWindowSystem WindowSystem;

	public Win32WindowHandle Win32;
	public X11WindowHandle X11;
	public WaylandWindowHandle Wayland;
	public CocoaWindowHandle Cocoa;
}

public struct CocoaWindowHandle
{
	public IntPtr Layer;
}

public struct WaylandWindowHandle
{
	public IntPtr Display;
	public IntPtr Surface;
}

public struct X11WindowHandle
{
	public IntPtr Display;
	public uint Window;
}



public struct Win32WindowHandle
{
	public IntPtr HINSTANCE;
	public IntPtr HWND;
}