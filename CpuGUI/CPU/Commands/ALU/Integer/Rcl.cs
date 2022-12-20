using CPUConsole.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUConsole.Commands.ALU.Integer
{
    internal class Rcl : CommandFormatRDSS
    {
        public Rcl(int rd, int left, int right) : base(rd, left,right,  9)
        {
        }

        public override void Execute(Registers registers)
        {
            int answer = 0;
            try { answer = checked(registers.Integer[registerSource] << registers.Integer[registerSourceR]); }
            catch (OverflowException) { registers.Flags[FlagsRegister.Overflowing] = true; }

            registers.Flags[FlagsRegister.Zero] = answer == 0;
            registers.Flags[FlagsRegister.Sign] = answer < 0;
            registers.Flags[FlagsRegister.Carry] = registers.Flags[FlagsRegister.Overflowing];

            registers.Integer[registerDestination] = answer;
            registers.ProgrammCounter++;
        }
    }
}
