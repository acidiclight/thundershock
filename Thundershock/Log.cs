#nullable enable

using System.Runtime.CompilerServices;

namespace Thundershock;

/// <summary>
///		Provides a simple API for debug logging.
/// </summary>
public static class Log
{
	public struct LogMessage
	{
		public string Category;
		public string Text;
		public string? StackTrace;
		public DateTime TimeStamp;
	}

	public delegate void LogMessageDelegate(in LogMessage message);

	public static event LogMessageDelegate? OnMessageLogged;

	private static LogMessage nextMessage = new LogMessage();
	private static readonly Stack<string> categoryStack = new Stack<string>();
	

	public static void Message(string text)
	{
		nextMessage.Text = text;
		Submit();
	}

	public static void PushCategory(string category)
	{
		categoryStack.Push(category);
	}

	public static void PopCategory()
	{
		if (categoryStack.Count > 0)
			categoryStack.Pop();
	}
	
	private static void Submit()
	{
		if (categoryStack.Count == 0)
			nextMessage.Category = "Engine";
		else
			nextMessage.Category = categoryStack.Peek();
		
		nextMessage.TimeStamp = DateTime.UtcNow;
		
		OnMessageLogged?.Invoke(nextMessage);
	}
}