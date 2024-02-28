using System;
using System.IO.Ports;
using System.Threading.Tasks;

class App
{
    static SerialPort port;
    static string[] info_line;
    static int padlen;

    static void Main(string[] args)
    {
        string path = "COM4";
        int baudRate = 9600;

        port = new SerialPort(path, baudRate);
        port.Open();

        port.DataReceived += new SerialDataReceivedEventHandler(handleReceivedData);

        port.Write("INFO\r\n");
        Task.Delay(100).Wait();
        port.Write("POLL 1000\r\n");
        Task.Delay(100).Wait();
        port.Write("FRAC 2\r\n");
        Task.Delay(100).Wait();
    }

    static void handleReceivedData(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort port = (SerialPort)sender;
        string data = port.ReadLine();
        string[] split = data.Replace(", ", ",").Split('*')[0].Split(',');

        Console.WriteLine(data);
        Console.WriteLine(split);

        if (split[0] == "I")
        {
            if (split[1] == "Product ID")
            {
                info_line = split;
                padlen = GetMaxLength(split, 4);
                Console.WriteLine(info_line);
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

        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {device}");
        for (int i = 0; i < units.Length; i++)
        {
            Console.WriteLine($"{sensors[i].PadRight(padlen + 2)} {values[i]} {units[i]}");
        }
        Console.WriteLine("\n");
    }

    static int GetMaxLength(string[] arr, int start)
    {
        int max = 0;
        for (int i = start; i < arr.Length; i += 2)
        {
            if (arr[i].Length > max)
            {
                max = arr[i].Length;
            }
        }
        return max;
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
