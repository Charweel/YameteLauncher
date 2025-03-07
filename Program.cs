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

        string host = "";
        string username = "";
        string password = "";

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
