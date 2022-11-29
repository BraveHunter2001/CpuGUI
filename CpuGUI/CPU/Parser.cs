using CPUConsole.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace CPUConsole
{
    public static class Parser
    {

        public static List<object[]> GetCommands(string text)
        {
            List<object[]> comObjs = new List<object[]>();

            var textByLine = text.Split("\r\n");
            foreach (var line in textByLine)
            {
                if (line.Length == 0)
                    continue;
                string commandLine = line;
                string[] commands = commandLine.Split(' ');

                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i].Contains(","))
                        commands[i] = commands[i].Remove(commands[i].Length - 1);
                }

                object[] args = new object[commands.Length];
                args[0] = ParseCommandOP(commands[0]);
                for (int i = 1; i < commands.Length; i++)
                {
                    if (int.TryParse(commands[i], out int res))
                        args[i] = res;
                    else
                        throw new Exception("Parse exp");
                }

                comObjs.Add(args);
            }

            return comObjs;
        }

        private static CommandOP ParseCommandOP(string str)
        {
            if (Enum.TryParse(str, out CommandOP result))
            {
                return result;
            }
            else
                throw new Exception("Error in command parse");
        }
    }
}
