using System;
using Renci.SshNet;

class Program
{
    static void ShowLoadingAnimation()
    {
        string[] frames = { "-", "\\", "|", "/" };
        for (int i = 0; i < 20; i++)
        {
            Console.Write("\rBağlanıyor " + frames[i % frames.Length]);
            System.Threading.Thread.Sleep(100);
        }
        Console.Write("\rBağlantı tamamlandı! ✅   \n");
    }

    static void Main(string[] args)
    {
        Console.Title = "YameteLauncher - SSH Bağlantı Terminali";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
 __    __                           __           __                                        __                      
/\ \  /\ \                         /\ \__       /\ \                                      /\ \                     
\ `\`\\/'/  __      ___ ___      __\ \ ,_\    __\ \ \         __     __  __    ___     ___\ \ \___      __   _ __  
 `\ `\ /' /'__`\  /' __` __`\  /'__`\ \ \/  /'__`\ \ \  __  /'__`\  /\ \/\ \ /' _ `\  /'___\ \  _ `\  /'__`\/\`'__\
   `\ \ \/\ \L\.\_/\ \/\ \/\ \/\  __/\ \ \_/\  __/\ \ \L\ \/\ \L\.\_\ \ \_\ \/\ \/\ \/\ \__/\ \ \ \ \/\  __/\ \ \/ 
     \ \_\ \__/.\_\ \_\ \_\ \_\ \____\\ \__\ \____\\ \____/\ \__/.\_\\ \____/\ \_\ \_\ \____\\ \_\ \_\ \____\\ \_\ 
      \/_/\/__/\/_/\/_/\/_/\/_/\/____/ \/__/\/____/ \/___/  \/__/\/_/ \/___/  \/_/\/_/\/____/ \/_/\/_/\/____/ \/_/ 

        ");
        Console.ResetColor();

        string host = "charweel-coding-kyp21kh62ka.ssh.ws-us118.gitpod.io";
        string username = "charweel-coding-kyp21kh62ka";
        string password = "AAAAB3NzaC1yc2EAAAADAQABAAACAQC06TQWx0g4bk1Z9KOj4pJu7AJjhcDOnwE1W/E8v+8xJ7l2Fxg/FbKMonDss39W0E2T/xvaGNpHFXZ9gzL6gzV/8FKiUBjIoNNNKjWwKwn3k0gQElyEyzQ+a1yNganVLodUyIj6PUYPZrIL4cCBOs/7YxfRNUl6gfa5Gzh2/qo1ScWr4rhj/hBXHPN+UiTkJGblrzcQ6/UMvVU1Gv/Byv3YlMbGT/AwgV+A53mEWS5n3FlugeNM8Fi8XZD3+RoPOR9dxXIqNERWiPFjN2NZmFeMLqFIrzdeGg8CAh8zu8NGAsnWPqZPpaGPbw2w1QfGtOtBy6UUakmPHshinUIQuuu9rVeZZbkGEjrtZ/VFZa/mdtv6iB8Z+jTJhlc3MneIyEi6EXgxlFvnxlRGiC2/2elv6fHkXtMck9JtjYNsHPstRUMfwiW+ea1nLMJl1EyXeOWXYvQVNZNbB70A+RwKtKY4L8y9qOuvm5LsUhEihwByJKZdWGYMn37z0RuCAb+NLOu31qIbNiE47iXVFNjQmO4Mc/8NEfdRx21UUL5vQwevTTwaG9NTcpk4iYBUyc8YTcgmWCJoa5BYOWisRm37wn8VUvd40P5eaXUk9S+8ykBtlS9EyPM8cJtgm4dxohmey9rcPWwEQz8kQJrTzQJwkeyEwwYq7GHa3SLXvACwZMMeyQ==";

        int localPort = 25565;
        int remotePort = 25565;

        try
        {
            using (var client = new SshClient(host, username, password))
            {
                Console.WriteLine("🔄 SSH bağlantısı başlatılıyor...");
                ShowLoadingAnimation();

                client.Connect();
                Console.WriteLine("✅ SSH bağlantısı başarılı!");

                var portForward = new ForwardedPortLocal("127.0.0.1", (uint)localPort, "127.0.0.1", (uint)remotePort);
                client.AddForwardedPort(portForward);
                portForward.Start();

                Console.WriteLine($"🚀 Port yönlendirme aktif: localhost:{localPort} -> {host}:{remotePort}");
                Console.WriteLine("Bağlantıyı kapatmak için bir tuşa basın...");
                Console.ReadKey();

                portForward.Stop();
                client.Disconnect();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Hata oluştu: {ex.Message}");
            Console.ResetColor();
        }
    }
}
