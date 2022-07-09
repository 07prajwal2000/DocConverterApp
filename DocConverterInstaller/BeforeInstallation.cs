using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    public class BeforeInstallation
    {
        private readonly DirectoryInfo installedDirectory;
        private readonly string[] menuItems = { "Continue Installation", "Uninstall the Application"};
        private int selectedMenuItem;

        public BeforeInstallation(string filePath)
        {
            installedDirectory = new DirectoryInfo(filePath);
            Console.WriteLine(installedDirectory.Exists);
        }

        /// <summary>
        /// Starts the execution
        /// </summary>
        /// <returns>Returns if Installation can be continued</returns>
        public bool Start()
        {
            Console.WriteLine("Application already installed");
            MenuLoop();
            if (selectedMenuItem == -1)
            {
                Console.WriteLine("Selected Exit. Press any key to close.");
                Console.ReadLine();
                return false;
            }
            Console.WriteLine("Selected - " + menuItems[selectedMenuItem]);
            if(selectedMenuItem == 1)
            {
                StartUninstallProcess();
                Console.WriteLine("Press any key to close.");
                Console.ReadLine();
            }
            else if(selectedMenuItem == 0)
            {
                installedDirectory.Delete(true);
            }
            return selectedMenuItem == 0;
        }
        
        private void MenuLoop(int idx = 0)
        {
            for (var i = 0; i < menuItems.Length; i++)
            {
                var selectedIdx = idx % menuItems.Length;
                if (selectedIdx == i)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(">> " + menuItems[selectedIdx]);
                    Console.ResetColor();
                    continue;
                }
                Console.WriteLine(menuItems[i]);
            }
            var key = Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Press ↑ arrow or , ↓ arrow to select menu, Press escape to exit.\n");
            if (key.Key == ConsoleKey.Enter)
            {
                selectedMenuItem = idx;
                return;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                selectedMenuItem = -1;
                return;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                idx++;
                if (idx >= menuItems.Length)
                {
                    idx = 0;
                }
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                idx--;
                if (idx < 0)
                {
                    idx = menuItems.Length - 1;
                }
            }
            MenuLoop(idx);
        }

        private async void StartUninstallProcess()
        {
            Console.WriteLine("Please wait while we Uninstall the Application...");
            Console.WriteLine("Application Location : " + installedDirectory.FullName);
            var flag = true;
            await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Uninstalling ");
                    var i = 0;
                    while (i < 10)
                    {
                        Console.Write('.');
                        i++;
                        Task.Delay(300);
                    }
                    Console.WriteLine();
                    if (installedDirectory.Exists)
                        installedDirectory.Delete(true);
                    DeleteMainKey();
                    DeleteWordDocxToPdfRegistryKey();
                    DeleteCompressRegistryKey();
                    DeletePdfToDocRegistryKey();
                }
                catch (Exception e)
                {
                    flag = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error occurred : " + e.Message);
                    Console.ResetColor();
                    throw;
                }
            });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(flag ? "Successfully Uninstalled the Application" : "");
            Console.ResetColor();
        }

        private void DeleteMainKey()
        {
            using var wordKey = Registry.ClassesRoot.OpenSubKey(@"*", true);
            using var temp = Registry.ClassesRoot.OpenSubKey(@"*\DocConverterApp", true);
            if (temp is null) return;
            wordKey.DeleteSubKey("DocConverterApp");
        }

        private void DeleteWordDocxToPdfRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            using var temp = Registry.ClassesRoot.OpenSubKey(@"*\shell\Word(docx) to PDF", true);
            if (temp is null) return;
            wordKey.DeleteSubKeyTree("Word(docx) to PDF");
        }

        private void DeleteCompressRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            using var temp = Registry.ClassesRoot.OpenSubKey(@"*\shell\PdfCompress", true);
            if (temp is null) return;
            wordKey.DeleteSubKeyTree("PdfCompress");
        }

        private void DeletePdfToDocRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            using var temp = Registry.ClassesRoot.OpenSubKey(@"*\shell\PdfToWord", true);
            if (temp is null) return;
            wordKey.DeleteSubKeyTree("PdfToWord");
        }

    }
}