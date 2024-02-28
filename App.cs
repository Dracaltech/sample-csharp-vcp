using System.IO.Ports;

class App
{
    const string PATH = "COM4";
    const int BAUDRATE = 9600;
    const int INTERVAL = 1000;

    static SerialPort port = new SerialPort(PATH, BAUDRATE);
    static string[]? info_line;
    static int padlen;

    static void Main(string[] args)
    {
        port.Open();
        port.DataReceived += new SerialDataReceivedEventHandler(handleReceivedData);

        port.Write($"POLL {INTERVAL}\r\n");
        Task.Delay(100).Wait();
        port.Write("FRAC 2\r\n");
        Task.Delay(100).Wait();

        port.Write("INFO\r\n");
        Task.Delay(100).Wait();
    }

    static void handleReceivedData(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort port = (SerialPort)sender;
        string data = port.ReadLine();
        string[] split = data.Replace(", ", ",").Split('*')[0].Split(',');

        if (split[0] == "I")
        {
            if (split[1] == "Product ID")
            {
                info_line = split;
                padlen = split.Skip(4).OrderByDescending(s => s.Length).First().Length;
                Console.WriteLine(string.Join(",", info_line));
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
        string[] sensors = GetEveryOther(info_line, 4, 0);
        float[] values = Array.ConvertAll(GetEveryOther(split, 4, 0), float.Parse);
        string[] units = GetEveryOther(split, 4, 1);

        Console.WriteLine(device);
        Console.WriteLine(string.Join(",", sensors));

        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {device}");
        for (int i = 0; i < units.Length; i++)
        {
            Console.WriteLine($"{sensors[i].PadRight(padlen + 2)} {values[i]} {units[i]}");
        }
        Console.WriteLine("\n");
    }

    static string[] GetEveryOther(string[] arr, int start, int offset)
    {
        var list = new System.Collections.Generic.List<string>();
        for (int i = start + offset; i < arr.Length; i += 2)
        {
            list.Add(arr[i]);
        }
        return list.ToArray();
    }
}
