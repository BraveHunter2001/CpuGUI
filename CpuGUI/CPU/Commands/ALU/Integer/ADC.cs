using CPUConsole.Commands;
using CPUConsole.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpuGUI.CPU.Commands.ALU.Integer
{
    internal class ADC : CommandFormatRDSS
    {
        public ADC(int rd, int regLeft, int regRight) : base(rd, regLeft, regRight, 49)
        {
        }

        public override void Execute(Registers registers)
        {
            int answer = 0;
            try { answer = checked(registers.Integer[registerSource] + 
                    registers.Integer[registerSourceR]) + 
                    (registers.Flags[FlagsRegister.Carry] ? 1 : 0); }
            catch (OverflowException) { registers.Flags[FlagsRegister.Overflowing] = true; }

            registers.Flags[FlagsRegister.Zero] = answer == 0;
            registers.Flags[FlagsRegister.Sign] = answer < 0;
            registers.Flags[FlagsRegister.Carry] = registers.Flags[FlagsRegister.Overflowing];


            registers.Integer[registerDestination] = answer;
            registers.ProgrammCounter++;
        }
    }
}
