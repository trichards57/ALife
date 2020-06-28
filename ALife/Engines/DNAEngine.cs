using ALife.Model;
using System;

namespace ALife.Engines
{
    public class DNAEngine
    {
        private FlowState currentFlowState;

        private enum FlowState
        {
            Clear = 0,
            Body = 1
        }

        private SafeIntStack IntStack { get; } = new SafeIntStack();

        public void ExecuteDNA(Bot bot)
        {
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
                        {
                            IntStack.Push(bot.Memory[CommandToAddress(basePair.Command)]);
                        }
                        break;

                    case BasePairType.Basic:
                        ExecuteBasicCommand(basePair.GetBasicCommand());
                        break;

                    case BasePairType.Store:
                        ExecuteStoreCommand(bot, basePair.GetStoreCommand());
                        break;

                    case BasePairType.Flow:
                        ExecuteFlowCommand(basePair.GetFlowCommand());
                        break;
                }
            }
        }

        private static int CommandToAddress(int command)
        {
            return Math.Abs(command % SystemVariables.MemoryLength) - 1;
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

        private void ExecuteFlowCommand(FlowCommand flowCommand)
        {
            currentFlowState = FlowState.Clear;
            if (flowCommand == FlowCommand.Start)
                currentFlowState = FlowState.Body;
        }

        private void ExecuteStoreCommand(Bot bot, StoreCommand storeCommand)
        {
            int a, b;

            switch (storeCommand)
            {
                case StoreCommand.Store:
                    b = CommandToAddress(IntStack.Pop());
                    a = IntStack.Pop();
                    bot.Memory[b] = a;
                    break;
            }
        }
    }
}