namespace Thundershock.Editor;

public class ProjectDatabase : IEngineModule
{
	public Project? CurrentProject { get; private set; }
	
	/// <inheritdoc />
	public void Initialize()
	{
	}

	/// <inheritdoc />
	public void RunOneUpdate()
	{
	}

	/// <inheritdoc />
	public void Shutdown()
	{
	}

	public void OpenProject(Project project)
	{
		if (CurrentProject != null)
			CloseCurrentProject();
	}

	public void CloseCurrentProject()
	{
		if (CurrentProject == null)
			return;

		
		
		CurrentProject = null;
	}
}