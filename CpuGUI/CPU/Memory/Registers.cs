using System;
using System.Collections.Generic;

namespace CPUConsole.Memory
{
    public enum FlagsRegister
    {
        Zero = 'Z',
        Carry = 'C',
        Sign = 'S',
        Overflowing = 'O',
        Iterrapt = 'I',
        StepByStep = 'T',
        SuperUser = 'U'
    }
    public class Registers : IDump
    {

        public int[] Integer;
        public float[] Float;

        private int _pc = 0;
        private int _pcCALL = 0;
        private int _pcIterrupt = 0;

        public int ProgrammCounter
        {
            get { return _pc; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Programm counter less zero!");
                else
                    _pc = value;
            }
        }
        public int ProgrammCounterCALL
        {
            get { return _pcCALL; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Programm counter less zero!");
                else
                    _pcCALL = value;
            }
        }
        public int ProgrammCounterInterrupt
        {
            get { return _pcIterrupt; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Programm counter less zero!");
                else
                    _pcIterrupt = value;
            }
        }

        public Dictionary<FlagsRegister, bool> Flags { get; set; }
        public Dictionary<FlagsRegister, bool> FlagsInterrupt { get; set; }

        private Registers(int[] integer, float[] Float, Dictionary<FlagsRegister, bool> flags)
        {
            Integer = integer;
            this.Float = Float;
            this.Flags = flags;
        }

        public Registers(int[] integer, float[] Float)
        {
            Integer = integer;
            this.Float = Float;
            this.Flags = new Dictionary<FlagsRegister, bool> {
                {FlagsRegister.Zero, false }, // Zero 
                {FlagsRegister.Carry, false }, // transition in high digt
                {FlagsRegister.Sign, false }, // Negative
                {FlagsRegister.Overflowing, false }, // overflowing
                {FlagsRegister.Iterrapt, false }, // interrapt
                {FlagsRegister.StepByStep, false }, // step-by-step mode
                {FlagsRegister.SuperUser, false } // superuser
            }; ;
        }

     

        public Dictionary<int, Action> InterruptTable = new Dictionary<int, Action>()
        {
            {0, Interrupts.DividedZeroException},
            {1, Interrupts.RegisterException},
            {2, Interrupts.MemoryException}
        };

        public void Clean()
        {
            for (int i = 0; i < Integer.Length; i++)
                Integer[i] = 0;

            for (int i = 0; i < Float.Length; i++)
                Float[i] = 0;

            foreach (var key in Flags.Keys)
                Flags[key] = false;
            _pc = 0;
        }
    }
}
