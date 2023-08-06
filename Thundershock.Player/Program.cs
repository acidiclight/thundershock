namespace Thundershock.Player
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using var app = new PlayerApplication();
            app.Run();
        } 
    }
}