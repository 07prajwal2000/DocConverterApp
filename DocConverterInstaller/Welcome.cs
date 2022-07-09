using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal class Welcome : IInstallationStep
    {
        private readonly string _title;
        private readonly string _owner;
        readonly DriveInfo[] driveInfo;
        int index = 0;
        bool success = true;

        public Welcome()
        {
            var stream1 = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("DocConverterInstaller.Resources.Owner.txt")!;
            
            var stream2 = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("DocConverterInstaller.Resources.Title_ASCII.txt")!;
            
            var reader1 = new StreamReader(stream1);
            var reader2 = new StreamReader(stream2);
            _owner = reader1.ReadToEndAsync().Result;
            _title = reader2.ReadToEndAsync().Result;
            driveInfo = DriveInfo.GetDrives();
        }

        public async Task StartTask(object? _) => await Task.Run(() =>
        {
            MainDisplay();
            ChooseDirectory();
            DisplaySelectedDirectory();
        });

        private void DisplaySelectedDirectory()
        {
            Console.WriteLine(success ? $"Selected Local Drive " + driveInfo[index].Name : "Exiting the application.");
        }

        private void MainDisplay()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(_title);
            Console.WriteLine(_owner);
            Console.WriteLine("Github : https://github.com/07prajwal2000");
        }

        private void ChooseDirectory()
        {
            ShowMenu();
            ConsoleKeyInfo pressedKey = Console.ReadKey();
            var i = 0;

            while (pressedKey.Key != ConsoleKey.Enter)
            {
                if(pressedKey.Key == ConsoleKey.DownArrow) i++;
                else if(pressedKey.Key == ConsoleKey.UpArrow)
                {
                    i--;
                    i = i <= 0 ? driveInfo.Length: i;
                }
                index = i % (driveInfo.Length);
                ShowMenu();
                pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.Escape) break;
            }
            if (pressedKey.Key == ConsoleKey.Escape) success = false;

        }

        private void ShowMenu()
        {
            Console.Clear();

            MainDisplay();
            Console.WriteLine("For Installation, 50-60 MB of storage is required.");
            for (var i = 0; i < driveInfo.Length; i++)
            {
                if (index == i)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(">> " + driveInfo[i].Name);
                    Console.ResetColor();
                    continue;
                }
                Console.WriteLine(driveInfo[i].Name);
            }
        }

        public void ChooseNextStep(ref InstallationSteps nextStep, ref object? data)
        {
            nextStep = InstallationSteps.ExtractFiles;
            data = driveInfo[index];
            if (!success)
            {
                nextStep = InstallationSteps.Close;
            }
            
        }
    }
}
