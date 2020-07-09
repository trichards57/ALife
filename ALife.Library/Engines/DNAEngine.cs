using ALife.Model;

namespace ALife.Engines
{
    public class DNAEngine
    {
        private FlowState currentFlowState;

        private enum FlowState
        {
            Clear = 0,
            Body = 1,
            Condition = 2
        }

        private SafeBoolStack BoolStack { get; } = new SafeBoolStack();
        private SafeIntStack IntStack { get; } = new SafeIntStack();

        public void ExecuteDNA(Bot bot)
        {
            BoolStack.Clear();
            IntStack.Clear();

            currentFlowState = FlowState.Clear;

            foreach (var basePair in bot.DNA)
            {
                switch (basePair.Type)
                {
                    case BasePairType.Number:
                        if (currentFlowState != FlowState.Clear)
                            IntStack.Push(basePair.Command);
                        break;

                    case BasePairType.StarNumber:
                        if (currentFlowState != FlowState.Clear)
                            IntStack.Push(bot.Memory[SystemVariables.NormaliseAddress(basePair.Command)]);
                        break;

                    case BasePairType.Basic:
                        if (currentFlowState != FlowState.Clear)
                            ExecuteBasicCommand(basePair.GetBasicCommand());
                        break;

                    case BasePairType.Store:
                        if (currentFlowState == FlowState.Body)
                            ExecuteStoreCommand(bot, basePair.GetStoreCommand());
                        break;

                    case BasePairType.Flow:
                        ExecuteFlowCommand(basePair.GetFlowCommand());
                        break;

                    case BasePairType.Condition:
                        if (currentFlowState != FlowState.Clear)
                            ExecuteConditionCommand(basePair.GetConditionCommand());
                        break;
                }
            }
        }

        private void ExecuteBasicCommand(BasicCommand command)
        {
            int a, b;

            switch (command)
            {
                case BasicCommand.Add:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    IntStack.Push(a + b);
                    break;

                case BasicCommand.Divide:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    if (b == 0)
                        IntStack.Push(0);
                    IntStack.Push(a / b);
                    break;

                case BasicCommand.Multiply:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    IntStack.Push(a * b);
                    break;

                case BasicCommand.Subtract:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    IntStack.Push(a - b);
                    break;
            }
        }

        private void ExecuteConditionCommand(ConditionCommand command)
        {
            int a, b;

            switch (command)
            {
                case ConditionCommand.LessThan:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    BoolStack.Push(a < b);
                    break;

                case ConditionCommand.GreaterThan:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    BoolStack.Push(a > b);
                    break;

                case ConditionCommand.Equals:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    BoolStack.Push(a == b);
                    break;

                case ConditionCommand.NotEquals:
                    b = IntStack.Pop();
                    a = IntStack.Pop();
                    BoolStack.Push(a != b);
                    break;
            }
        }

        private void ExecuteFlowCommand(FlowCommand flowCommand)
        {
            switch (flowCommand)
            {
                case FlowCommand.Start:
                    if (currentFlowState != FlowState.Condition || BoolStack.Summarise())
                        currentFlowState = FlowState.Body;
                    break;

                case FlowCommand.Condition:
                    currentFlowState = FlowState.Condition;
                    BoolStack.Clear();
                    break;

                default:
                    currentFlowState = FlowState.Clear;
                    break;
            }
        }

        private void ExecuteStoreCommand(Bot bot, StoreCommand storeCommand)
        {
            int a, b;

            switch (storeCommand)
            {
                case StoreCommand.Store:
                    b = SystemVariables.NormaliseAddress(IntStack.Pop());
                    a = IntStack.Pop();
                    if (SystemVariables.IsAddressWritable(b))
                        bot.Memory[b] = a;
                    break;
            }
        }
    }
}