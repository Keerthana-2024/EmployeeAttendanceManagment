using EmployeeAttendanceManagment; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        var key = GenerateSecretKey(32); // Generate a key of 256 bits (32 bytes)
        var base64EncodedKey = Convert.ToBase64String(key);
        Console.WriteLine(base64EncodedKey);

    }
    public static byte[] GenerateSecretKey(int sizeInBytes)
    {
        byte[] key = new byte[sizeInBytes];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        return key;
    }


    // This method sets up the web host builder
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)  // Creates a default host builder
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();  // Configures the web host with Startup class
            });
}
