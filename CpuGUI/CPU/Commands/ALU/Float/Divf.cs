using CPUConsole.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUConsole.Commands.ALU.Float
{
    internal class Divf : CommandFormatRDSS
    {
        public Divf(int rd, int regLeft, int regRight) : base(rd, regLeft, regRight, 14)
        {
        }

        public override void Execute(Registers registers)
        {
            if (registers.Float[registerSourceR] == 0)
                registers.InterruptTable[0]();

            float answer = 0;
            try { answer = checked(registers.Float[registerSource] / registers.Float[registerSourceR]); }
            catch (OverflowException) { registers.Flags[FlagsRegister.Overflowing] = true; }

            registers.Flags[FlagsRegister.Zero] = answer == 0;
            registers.Flags[FlagsRegister.Sign] = answer < 0;
            registers.Flags[FlagsRegister.Carry] = registers.Flags[FlagsRegister.Overflowing];

            registers.Float[registerDestination] = answer;
            registers.ProgrammCounter++;
        }
    }
}
