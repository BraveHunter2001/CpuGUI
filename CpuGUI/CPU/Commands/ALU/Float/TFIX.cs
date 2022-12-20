using CPUConsole.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUConsole.Commands.ALU.Float
{
    internal class TFIX : CommandFormatRDS
    {
        public TFIX(int integerRd, int floatRs) : base(integerRd,  floatRs, 17)
        {
        }

        public override void Execute(Registers registers)
        {
            
           int answer = 0;
            try { answer = checked((int)registers.Float[registerSource]); }
            catch (OverflowException) { registers.Flags[FlagsRegister.Overflowing] = true; }

            registers.Flags[FlagsRegister.Zero] = answer == 0;
            registers.Flags[FlagsRegister.Sign] = answer < 0;
            registers.Flags[FlagsRegister.Carry] = registers.Flags[FlagsRegister.Overflowing];

            registers.Integer[registerDestination] = answer;
            registers.ProgrammCounter++;
        }
    }
}
