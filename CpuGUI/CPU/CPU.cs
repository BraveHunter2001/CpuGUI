using CPUConsole.Commands;
using CPUConsole.Commands.Ports;
using CPUConsole.Memory;
using System.Collections.Generic;

namespace CPUConsole
{
    internal class CPU
    {
        public Registers registers = new Registers(new int[4], new float[3]);

        RAM mem = new RAM(10);
        Port port = new USB();
        public CommadFactory commadFactory;

        List<Command> commands = new List<Command>();
        

        public CPU()
        {
            commadFactory = new CommadFactory(mem,port);
        }
       
        public void AddCommands(List<Command> commands)
        {
            this.commands.AddRange(commands);
        }

        public void ExecuteCommands()
        {
            for (int i = 0; i < commands.Count; i = registers.ProgrammCounter)
            {
                var curCommand = commands[i];
                //curCommand.Dump();
                curCommand.Execute(registers);


               
            }
        }



    }
}
