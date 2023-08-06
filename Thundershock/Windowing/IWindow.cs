namespace Thundershock.Windowing;

public interface IWindow
{
	int Width { get; set; }
	int Height { get; set; }
	bool IsOpen { get; }
	string? Title { get; set; }

	void Open();
	void Close();
}