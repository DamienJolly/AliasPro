using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AliasPro.Utilities
{
    public static class Logging
    {
		/// <summary>
		/// Is a debugger currently attached to the server.
		/// </summary>
		public static bool IsDebugEnabled => Debugger.IsAttached;

		public static void Alias(string text, string version)
        {
            Console.Title = text;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                      ______   _      _");
            Console.WriteLine("                     |  __  | | |    |_|  ______    ______");
            Console.WriteLine("                     | |__| | | |     _  |  __  |  |  ____|");
            Console.WriteLine("                     |  __  | | |__  | | | |__| |  |____  |");
            Console.WriteLine(@"                     |_|  |_| |____| |_| |_______\ |______|");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("                                   Build: " + version);
            Console.WriteLine("                             https://DamienJolly.com");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void LoadFiles()
        {
            File.Delete(@"log.alias");
            CreateFile(@"exceptions.alias");
            CreateFile(@"log.alias");
        }

        public static void CreateFile(string fileName)
        {
            var myFile = File.Create(fileName);
            myFile.Close();
        }

		public static void Warn(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[Alias] [Warning] : " + message);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void Error(string information, Exception exception = null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[Alias] [Error] : " + information);
			Console.ForegroundColor = ConsoleColor.Gray;
			if (exception != null)
			{
				string currentText = File.ReadAllText(@"exceptions.alias");
				currentText += "\n\n";
				currentText += "Date: " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
				currentText += "\nEmulator Information: \"" + information;
				currentText += "\nInformation for developer: " + exception.ToString();
				File.WriteAllText(@"exceptions.alias", currentText);
			}
		}

		internal static void Command()
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("[Alias] [Command] : ");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		/*internal static void Message(ClientPacket message, string id)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("[Alias] [Message] [" + id + "] [" + message.Header + "] : " + message.ToString());
			Console.ForegroundColor = ConsoleColor.Gray;
		}*/

		public static void Debug(string debugtext)
		{
			if (IsDebugEnabled)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("[Alias] [Debug] : " + debugtext);
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}

		public static void Info(string information)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("[Alias] [Information] : " + information);
			Console.ForegroundColor = ConsoleColor.Gray;
			if (IsDebugEnabled)
			{
				string currentText = File.ReadAllText(@"log.alias");
				currentText += "\n";
				currentText += information;
				File.WriteAllText(@"log.alias", currentText);
			}
		}

		public static void Exit(string error)
		{
			Logging.Error(error);
			Logging.Info("Press any key to exit.");
			Console.ReadKey();
			Environment.Exit(0);
		}
    }
}
