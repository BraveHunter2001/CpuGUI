
using CPUConsole.Commands.Ports;
using CPUConsole.Memory;


namespace CPUConsole.Commands
{
    public interface IDump
    {
        public string Dump();
    }
    public abstract class Command : IDump
    {
        private readonly int _op = 0;
       
        public int OPcode
        {
            get { return _op; }
        }
        public Command( int opcode)
        {
            _op = opcode;
            
        }
        public abstract void Execute(Registers registers);

        public virtual string Dump()
        {
            return $"OP:{OPcode} ";
        }

 

    }
    public abstract class CommandPort : Command
    {
        protected Port port;
        protected int regAddr;
        protected int pinPort;
        protected CommandPort(int regAddr, int pinPort, Port port, int opcode) : base(opcode)
        {
            this.regAddr = regAddr;
            this.pinPort = pinPort;
            this.port = port;
        }

       
       public override string Dump()
        {
            return $"OP:{OPcode} r{regAddr} ,  p{pinPort}";
       }
    }
    public abstract class CommandMemmory : Command
    {
        protected readonly int registerAddres;
        protected readonly int memmoryAddres;
        protected RAM mem;
        protected CommandMemmory(int regAddr, int memAddr, RAM mem, int opcode) : base(opcode)
        {

            registerAddres = regAddr;
            memmoryAddres = memAddr;
            this.mem = mem;
        }



        public override string Dump()
        {
            return $"OP:{OPcode} r{registerAddres} ,  M{memmoryAddres}";
        }

    }

    public abstract class CommandFormatC : Command
    {
        protected int constant;
        protected CommandFormatC(int constant,int opcode) : base(opcode)
        {
            this.constant= constant;
        }
        public override string Dump()
        {
            return $"OP:{OPcode} Const{constant} ";
        }
    }
    public abstract class CommandFormatRDS : Command
    {
        protected readonly int registerDestination;
        protected readonly int registerSource;
        public CommandFormatRDS(int rd, int rs, int opcode) : base(opcode)
        {
            registerDestination = rd;
            this.registerSource = rs;
        }

        public override string Dump()
        {
            return $"OP:{OPcode} r{registerDestination} <- r{registerSource}";
        }
    }

    /// <summary>
    /// RegisterFormat dest const opcode
    /// </summary>
    public abstract class CommandFormatRDC : Command
    {
        protected readonly int registerDestination;
        protected readonly int constant;
        public CommandFormatRDC(int rd, int constant, int opcode) : base(opcode)
        {
            registerDestination = rd;
            this.constant = constant;
        }

        public override string Dump()
        {
            return $"OP:{OPcode} r{registerDestination} <- {constant}";
        }
    }
    /// <summary>
    /// RegisterFormat dest source constant
    /// </summary>
    public abstract class CommandFormatRDSC : CommandFormatRDC
    {
        protected readonly int registerSource;
        public CommandFormatRDSC(int rd, int rs, int constant, int opcode) : base(rd, constant, opcode)
        {
            registerSource = rs;

        }

        public override string Dump()
        {
            return $"OP:{OPcode} r{registerDestination} <- r{registerSource}, {constant}";
        }
    }
    /// <summary>
    /// RegisterFormat dest source constant
    /// </summary>
    public abstract class CommandFormatRDSS : CommandFormatRDS
    {
        
        protected readonly int registerSourceR;
        public CommandFormatRDSS(int rd, int regLeft, int regRight, int opcode) : base(rd, regLeft, opcode)
        {
            
            registerSourceR = regRight;
        }

        public override string Dump()
        {
            return $"OP:{OPcode} r{registerDestination} <- r{registerSource}, r{registerSourceR}";
        }
    }

    
}
