using CPUConsole.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUConsole.Commands.ALU.Integer
{
    internal class Cmp : CommandFormatRDS
    {
        public Cmp(int rd, int rs) : base(rd, rs, 4)
        {
        }

        public override void Execute(Registers registers)
        {
            int answer = 0;
            try { answer = checked(registers.Integer[registerDestination] - registers.Integer[registerSource]); }
            catch (OverflowException) { registers.Flags[FlagsRegister.Overflowing] = true; }

            registers.Flags[FlagsRegister.Zero] = answer == 0;
            registers.Flags[FlagsRegister.Sign] = answer < 0;
            registers.Flags[FlagsRegister.Carry] = registers.Flags[FlagsRegister.Overflowing];

            registers.ProgrammCounter++;
        }
    }

}
