using System.Numerics;
using Silk.NET.WebGPU;
using Thundershock.Windowing;

namespace Thundershock.Graphics.WebGpuGraphics;

public sealed unsafe class WebGpuGraphicsCard : GraphicsCard
{
	private readonly WebGPU wgpu;
	private readonly IWindow window;
	private readonly Instance* instance;
	private Surface* nativeSurface;
	private Adapter* adapter;
	private Device* device;
	private SwapChain* swapChain;

	public WebGpuGraphicsCard(IWindow window)
	{
		if (!window.IsOpen)
			throw new InvalidOperationException("Cannot create a WebGPU graphics card instance on a window that isn't open.");
		
		this.window = window;
		
		wgpu = WebGPU.GetApi();

		var descriptor = new InstanceDescriptor();
		this.instance = wgpu.CreateInstance(in descriptor);
	}

	/// <inheritdoc />
	protected override void OnActivate()
	{
		this.nativeSurface = CreateNativeWindowSurface();

		var options = new RequestAdapterOptions()
		{
			CompatibleSurface = this.nativeSurface
		};

		wgpu.InstanceRequestAdapter(
			instance,
			in options,
			new PfnRequestAdapterCallback(( _,  adapter,  _,  _) =>
			{
				this.adapter = adapter;
			}),
			null
		);

		var deviceDescriptor = new DeviceDescriptor();
		wgpu.AdapterRequestDevice(
			adapter,
			in deviceDescriptor,
			new PfnRequestDeviceCallback(( _,  device,  _,  _) =>
			{
				this.device = device;
			}),
			null
		);

		TextureFormat surfaceFormat = wgpu.SurfaceGetPreferredFormat(nativeSurface, adapter);

		var swapChainDescriptor = new SwapChainDescriptor()
		{
			Width = 1280,
			Height = 720,
			Format = surfaceFormat
		};
		
		this.swapChain = wgpu.DeviceCreateSwapChain(this.device, this.nativeSurface, in swapChainDescriptor);
	}

	/// <inheritdoc />
	protected override void OnDeactivate()
	{
		adapter = null;
		nativeSurface = null;
	}

	/// <inheritdoc />
	public override void Clear(Vector3 clearColor)
	{
		
	}

	/// <inheritdoc />
	public override void Present()
	{
		wgpu.SwapChainPresent(swapChain);
	}

	private Surface* CreateNativeWindowSurface()
	{
		if (window.NativeWindow is null)
			throw new InvalidOperationException("Cannot create a WebGPU graphics card instance on a window that doesn't report native window information.");

		var surfaceDescriptor = new SurfaceDescriptor();

		NativeWindowInfo windowInfo = this.window.NativeWindow.WindowInfo;

		switch (windowInfo.WindowSystem)
		{
			case NativeWindowSystem.Win32:
			{
				var win32Descriptor = new SurfaceDescriptorFromWindowsHWND()
				{
					Chain = new ChainedStruct
					{
						Next = null,
						SType = SType.SurfaceDescriptorFromWindowsHwnd
					},
					Hwnd = (void*) windowInfo.Win32.HWND,
					Hinstance = (void*) windowInfo.Win32.HINSTANCE
				};

				surfaceDescriptor.NextInChain = (ChainedStruct*) (&win32Descriptor);
				
				break;
			}

			case NativeWindowSystem.X11:
			{
				var xlibDescriptor = new SurfaceDescriptorFromXlibWindow()
				{
					Chain = new ChainedStruct
					{
						Next = null,
						SType = SType.SurfaceDescriptorFromXlibWindow
					},
					Display = (void*) windowInfo.X11.Display,
					Window = windowInfo.X11.Window
				};

				surfaceDescriptor.NextInChain = (ChainedStruct*) (&xlibDescriptor);
				
				break;
			}
			
			case NativeWindowSystem.Wayland:
			{
				var waylandDescriptor = new SurfaceDescriptorFromWaylandSurface()
				{
					Chain = new ChainedStruct()
					{
						Next = null,
						SType = SType.SurfaceDescriptorFromWaylandSurface
					},
					Display = (void*) windowInfo.Wayland.Display,
					Surface = (void*) windowInfo.Wayland.Surface
				};

				surfaceDescriptor.NextInChain = (ChainedStruct*) (&waylandDescriptor);
				
				break;
			}
			
			default:
				throw new InvalidOperationException("Unsupported window system!");
		}
		
		return wgpu.InstanceCreateSurface(this.instance, in surfaceDescriptor);
	}
}