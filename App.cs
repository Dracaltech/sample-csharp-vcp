using System.IO.Ports;

class App
{
    const string PATH = "COM4";
    const int BAUDRATE = 9600;
    const int INTERVAL = 1000;

    static SerialPort port = new(PATH, BAUDRATE, Parity.None, 8, StopBits.One);
    static string[]? info_line;
    static int padlen;
    
    static void Main(string[] args)
    {
        Console.CancelKeyPress += (s, e) => { Environment.Exit(0); };
        try
        {
            port.Open();

            port.Write($"POLL {INTERVAL}\r\n");
            Task.Delay(100).Wait();
            port.Write("FRAC 2\r\n");
            Task.Delay(100).Wait();
            port.Write("INFO\r\n");
            Task.Delay(100).Wait();

            // NOTE: while the standard approach would be to use Event handler `port.DataReceived`, it has
            // proven unable to receive non-input driven readout data thus far.
            while (port.IsOpen)
            {
                handleReceivedData();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static void handleReceivedData()
    {
        string data = port.ReadLine();
        string[] split = data.Replace(", ", ",").Split('*')[0].Split(',');

        if (split[0] == "I")
        {
            if (split[1] == "Product ID")
            {
                info_line = split;
                padlen = split.Skip(4).OrderByDescending(s => s.Length).First().Length;
                Console.WriteLine(string.Join(",", split));
            }
            else
            {
                Console.WriteLine(split[3]);
            }
            return;
        }
        if (info_line == null)
        {
            Console.WriteLine("Awaiting info line...");
            return;
        }

        string device = $"{split[1]} {split[2]}";
        string[] sensors = info_line[4..].Where((v, i) => i % 2 < 1).ToArray();
        string[] values = split[4..].Where((v, i) => i % 2 < 1)/*.Select(double.Parse)*/.ToArray();
        string[] units = split[4..].Where((v, i) => i % 2 > 0).ToArray();

        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {device}");
        for (int i = 0; i < units.Length; i++)
        {
            Console.WriteLine($"{sensors[i].PadRight(padlen + 2)} {values[i]} {units[i]}");
        }
        Console.WriteLine("\n");
    }
}
