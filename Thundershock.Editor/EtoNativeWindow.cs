using System.Runtime.InteropServices;
using Eto.Forms;
using Eto.GtkSharp.Forms;
using Gtk;
using Thundershock.Windowing;

namespace Thundershock.Editor;

public class EtoNativeWindow : INativeWindow
{
	private readonly Control control;

	public EtoNativeWindow(Control control)
	{
		this.control = control;
	}

	/// <inheritdoc />
	public NativeWindowInfo WindowInfo => DetermineSystemWindowInfo();

	private NativeWindowInfo DetermineSystemWindowInfo()
	{
		var result = new NativeWindowInfo();

		if (control.Platform.IsGtk)
		{
			// Note: Eto.Forms doesn't support Wayland.
			// This is because, on Linux, it uses GTK3. Which doesn't support Wayland.
			// So we must assume X11.
			// For fuck sake.
			result.WindowSystem = NativeWindowSystem.X11;
			
			// Get the x11 display handle from GTK.
			// First we need the native gtk widget.
			Widget? widget = control.GetGtkControlHandler().ContainerControl;
			
			// Now for the display handle.
			result.X11.Display = gdk_x11_display_get_xdisplay(widget.Display.Handle);
			
			// Get the window handle.
			result.X11.Window = (uint) gdk_x11_window_get_xid(widget.Window.Handle);
		}
		
		return result;
	}

	#region X11/GDK bindings

	// These bindings allow us to get X11 display and window handles to use in the rest of the engine.
	// Taken from https://github.com/picoe/Eto.Veldrid/blob/master/src/Eto.Veldrid.Gtk/X11Interop.cs

	private const string LinuxLibgdkX11Name = "libgdk-3.so.0";
	private const string LinuxLibGlName = "libGL.so.1";

	[DllImport(LinuxLibgdkX11Name)]
	private static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

	[DllImport(LinuxLibgdkX11Name)]
	private static extern int gdk_x11_screen_get_screen_number(IntPtr gdkScreen);

	[DllImport(LinuxLibgdkX11Name)]
	private static extern IntPtr gdk_x11_window_get_xid(IntPtr gdkDisplay);

	[DllImport(LinuxLibGlName)]
	private static extern IntPtr glXGetProcAddress(string name);

	#endregion

}