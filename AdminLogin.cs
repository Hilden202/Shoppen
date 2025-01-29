using System;
using System.Text;
using WindowDemo;

namespace ConsoleShoppen
{
        public static class AdminLogin
        {
            public static bool ShowLoginWindow()
            {
                //Console.Clear();
                var loginWindow = new Window("Admin Login", 0, 6, new List<string> { "Ange lösenord:                   " });
                loginWindow.Draw();

                Console.SetCursorPosition(0 + "Ange lösenord:".Length + 2, 7);
                string password = ReadPassword();

                return password == "1234"; // Returnerar true om lösenordet är korrekt
            }

            private static string ReadPassword()
            {
                StringBuilder password = new StringBuilder();
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b"); // Tar bort senaste '*' från konsolen
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        password.Append(key.KeyChar);
                        Console.Write("*"); // Skriver ut '*' istället för lösenordet
                    }
                }
                return password.ToString();
            }
        }
    }