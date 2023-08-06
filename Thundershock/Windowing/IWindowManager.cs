namespace Thundershock.Windowing;

internal interface IWindowManager
{
	internal void AssignToModuleInternal(WindowingModule? module);

	internal void Initialize();
	internal void Shutdown();
	internal void Update();
}