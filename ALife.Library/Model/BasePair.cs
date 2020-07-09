using System;

namespace ALife.Model
{
    public enum BasePairType
    {
        Unknown = 0,
        Basic = 1,
        Store = 2,
        Number = 3,
        StarNumber = 4,
        Flow = 5,
        Condition = 6,
        Boolean = 7
    }

    public enum BasicCommand
    {
        Unknown = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4
    }

    public enum BooleanCommand
    {
        Unknown = 0,
        And = 1,
        Or = 2,
        Xor = 3
    }

    public enum ConditionCommand
    {
        Unknown = 0,
        LessThan = 1,
        GreaterThan = 2,
        Equals = 3,
        NotEquals = 4
    }

    public enum FlowCommand
    {
        Unknown = 0,
        Start = 1,
        Stop = 2,
        Condition = 3
    }

    public enum StoreCommand
    {
        Unknown = 0,
        Store = 1
    }

    public class BasePair
    {
        public BasePair(BasePairType type, int command)
        {
            Type = type;
            Command = command;
        }

        public int Command { get; set; }
        public BasePairType Type { get; set; }

        public BasicCommand GetBasicCommand()
        {
            if (Enum.IsDefined(typeof(BasicCommand), Command))
                return (BasicCommand)Command;

            return BasicCommand.Unknown;
        }

        public ConditionCommand GetConditionCommand()
        {
            if (Enum.IsDefined(typeof(ConditionCommand), Command))
                return (ConditionCommand)Command;

            return ConditionCommand.Unknown;
        }

        public FlowCommand GetFlowCommand()
        {
            if (Enum.IsDefined(typeof(FlowCommand), Command))
                return (FlowCommand)Command;

            return FlowCommand.Unknown;
        }

        public StoreCommand GetStoreCommand()
        {
            if (Enum.IsDefined(typeof(StoreCommand), Command))
                return (StoreCommand)Command;

            return StoreCommand.Unknown;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case BasePairType.Basic:
                    return GetBasicCommand().ToString();

                case BasePairType.Condition:
                    return GetConditionCommand().ToString();

                case BasePairType.Flow:
                    return GetFlowCommand().ToString();

                case BasePairType.Number:
                    return Command.ToString();

                case BasePairType.StarNumber:
                    return $"*{Command}";

                case BasePairType.Store:
                    return GetStoreCommand().ToString();

                default:
                    return "Unknown";
            }
        }

        internal BooleanCommand GetBooleanCommand()
        {
            if (Enum.IsDefined(typeof(BooleanCommand), Command))
                return (BooleanCommand)Command;

            return BooleanCommand.Unknown;
        }
    }
}