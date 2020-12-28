using CommandLine;
using CommandLine.Text;
using Serilog;
using System;
using System.Collections.Generic;

namespace console_input_output
{


    class Options
    {
        [Option("config", Required = false, HelpText = "Specify config filename.")]
        public string ConfigFile { get; set; }

        [Option('c', "command", Required = false, HelpText = "Select a command.")]
        public string Command { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Show all messages.", Default = false)]
        public bool Verbose { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
            {
                log.Information("Serilog Console Sink");
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var options = new Options();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            Log.Logger.Debug("Argument Parse Successful!");
            // Show all console Colors
            var consoleColors = Enum.GetValues(typeof(ConsoleColor)) as ConsoleColor[];
            Console.WriteLine("List of available Console Colors:");
            foreach (var color in consoleColors)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(color);
            }

            Console.ReadLine();
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Log.Logger.Debug("Argument Parse Error!");
            WriteError("Error!");
        }

        static void WriteError(string msg)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = temp;
        }
    }
}
