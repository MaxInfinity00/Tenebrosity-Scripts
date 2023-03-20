using System;

namespace Team11.DebugConsole
{
    public class DebugCommandBase
    {
        public string CommandId { get; }

        public string CommandDescription { get; }

        public string CommandFormat { get; }

        protected DebugCommandBase(string id, string description, string format)
        {
            CommandId = id;
            CommandDescription = description;
            CommandFormat = format;
        }
    }

    public class DebugCommand : DebugCommandBase
    {
        private readonly Action _command;
        
        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }
    
    public class DebugCommand<T1> : DebugCommandBase
    {
        private readonly Action<T1> _command;
        
        public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value)
        {
            _command.Invoke(value);
        }
    }
}