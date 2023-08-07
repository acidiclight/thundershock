using System.ComponentModel;
using Microsoft.VisualBasic.CompilerServices;
using Thundershock.Graphics.WebGpuGraphics;

namespace Thundershock.Editor;

public class EditorApplication : Application
{
	private static EditorApplication editorInstance;
	
	private Eto.Forms.Application? etoApplication;
	private MainWindow? editorWindow;
	private ProjectDatabase projectDatabase;

	public static EditorApplication Instance => editorInstance;

	public EditorApplication() : base()
	{
		editorInstance = this;
	}
	
	/// <inheritdoc />
	protected override void RegisterModules(ModuleManager moduleManager)
	{
		projectDatabase = moduleManager.AddModule<ProjectDatabase>();
	}

	/// <inheritdoc />
	protected override void OnInitialize()
	{
		etoApplication = new Eto.Forms.Application();
		etoApplication.Terminating += OnEtoApplicationTerminating;

		editorWindow = new MainWindow(this.WindowingModule);
		editorWindow.Show();

		EditorDocument viewportDocument = editorWindow.DocumentManager.CreateWindow("Viewport");

		viewportDocument.Open();

		var gpu = new WebGpuGraphicsCard(viewportDocument);

		gpu.Activate();
	}

	/// <inheritdoc />
	protected override void OnUpdate()
	{
		etoApplication?.RunIteration();
		base.OnUpdate();
	}

	private void OnEtoApplicationTerminating(object? sender, CancelEventArgs e)
	{
		Exit();
	}


	/// <inheritdoc />
	protected override void OnShutdown()
	{
		etoApplication?.Dispose();
		etoApplication = null;
		editorInstance = null;
	}
}