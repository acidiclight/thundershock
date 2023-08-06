namespace Thundershock;

public interface IEngineModule
{
	void Initialize();
	void RunOneUpdate();
	void Shutdown();
}