using CPUConsole.Commands;
using CPUConsole.Commands.Ports;
using CPUConsole.Memory;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CPUConsole
{
    internal class CPU
    {
        public delegate void CPUHandler();
        public event CPUHandler CommandWasExecute;
        public delegate void CommandDumper(IDump dump);
        public event CommandDumper CommandWasDump;

        public Registers registers = new Registers(new int[32], new float[32]);
        public List<Registers> registersStates = new List<Registers>();
        public int CountCommand { get; private set; }
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
            CountCommand = this.commands.Count;
        }
        public void AddCommands(List<object[]> commandsObjs)
        {
            List<Command> commands = new List<Command>();

            foreach (var c in commandsObjs)
            {
                commands.Add(commadFactory.CreateCommand(c));
            }
            this.commands.AddRange(commands);
            CountCommand = this.commands.Count;
        }
        async public void ExecuteCommands()
        {
            for (int i = 0; i < commands.Count; i = registers.ProgrammCounter)
            {
                var curCommand = commands[i];

                registersStates.Add(registers.Clone());

                curCommand.Execute(registers);
                CommandWasExecute?.Invoke();
                CommandWasDump?.Invoke(curCommand);

                await Task.Delay(500);
            }
        }

        public void ExcuteCommandNext()
        {
            if (registers.ProgrammCounter > commands.Count)
                throw new System.Exception("PC more count command");
            var curCommand = commands[registers.ProgrammCounter];

            registersStates[registers.ProgrammCounter] = registers.Clone();

            curCommand.Execute(registers);

            CommandWasExecute?.Invoke();
        }
        public void ExcuteCommandPrev()
        {
            if (registers.ProgrammCounter == 0)
                throw new System.Exception("Programm counter is zero");
            var prevReg = registersStates[registers.ProgrammCounter - 1];
            registers = prevReg;

            CommandWasExecute?.Invoke();
        }

        public void Clear()
        {
            commands.Clear();
            registers.Clean();
        }



    }
}
