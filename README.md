# sample-csharp-vcp
Dracal // SDK code sample for C# on VCP

## Assumptions

Running this repository requires you to have installed:
- .NET (version >= `8.0`)
- Visual Studio (version >= 2022)


## Simple usage

Run by
- Using the **Play** button (Visual Studio)
- Build and run using the command line:

```
dotnet run sample-csharp-vcp.sln
```



## Sample output
<img src="https://github.com/Dracaltech/sample-node-vcp/assets/1357711/305ff9ae-2d98-4485-99a6-d09c02523d1e" width=400 />

```
Awaiting info line...
Awaiting info line...
Printing 2 fractional digits
I,Product ID,Serial Number,Message,MS5611 Pressure,Pa,SHT31 Temperature,C,SHT31 Relative Humidity,%,
2024-03-04 15:37:40 VCP-PTH450-CAL E24638
MS5611 Pressure           101905 Pa
SHT31 Temperature         21.64 C
SHT31 Relative Humidity   52.32 %


2024-03-04 15:37:41 VCP-PTH450-CAL E24638
MS5611 Pressure           101909 Pa
SHT31 Temperature         21.62 C
SHT31 Relative Humidity   52.30 %


2024-03-04 15:37:42 VCP-PTH450-CAL E24638
MS5611 Pressure           101907 Pa
SHT31 Temperature         21.62 C
SHT31 Relative Humidity   52.34 %


2024-03-04 15:37:43 VCP-PTH450-CAL E24638
MS5611 Pressure           101910 Pa
SHT31 Temperature         21.65 C
SHT31 Relative Humidity   52.29 %



C:\dev\dracal\sample-csharp-vcp\bin\Debug\net8.0\sample-csharp-vcp.exe (process 12276) exited with code 0.
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .
```
