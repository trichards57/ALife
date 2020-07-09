using ALife.Model;
using System.Collections.Generic;
using System.IO;

namespace ALife.Serializer
{
    public static class DNASerializer
    {
        public static List<BasePair> DeserializeDNA(string filename)
        {
            try
            {
                var fileContents = File.ReadAllLines(filename);
                var result = new List<BasePair>();

                foreach (var line in fileContents)
                {
                    var trimmedLine = line;

                    if (trimmedLine.Contains("'"))
                    {
                        var index = trimmedLine.IndexOf("'");
                        trimmedLine = trimmedLine.Substring(0, index - 1);
                    }

                    trimmedLine = trimmedLine.Trim();

                    if (trimmedLine.Length == 0)
                        continue;

                    var parts = trimmedLine.Split(new[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (var p in parts)
                    {
                        var basePair = ParseBasicCommand(p)
                            ?? ParseStoreCommand(p)
                            ?? ParseFlowCommand(p)
                            ?? ParseConditionCommand(p)
                            ?? ParseBooleanCommand(p)
                            ?? ParseVariable(p);

                        if (basePair != null)
                            result.Add(basePair);
                    }
                }

                return result;
            }
            catch (IOException)
            {
                return new List<BasePair>();
            }
        }

        private static BasePair ParseBasicCommand(string command)
        {
            if (command.Equals("add", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Basic, (int)BasicCommand.Add);
            if (command.Equals("sub", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Basic, (int)BasicCommand.Subtract);
            if (command.Equals("mult", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Basic, (int)BasicCommand.Multiply);
            if (command.Equals("div", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Basic, (int)BasicCommand.Divide);

            return null;
        }

        private static BasePair ParseBooleanCommand(string command)
        {
            if (command.Equals("and", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Boolean, (int)BooleanCommand.And);
            if (command.Equals("or", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Boolean, (int)BooleanCommand.Or);
            if (command.Equals("xor", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Boolean, (int)BooleanCommand.Xor);

            return null;
        }

        private static BasePair ParseConditionCommand(string command)
        {
            if (command.Equals("<", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Condition, (int)ConditionCommand.LessThan);
            if (command.Equals(">", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Condition, (int)ConditionCommand.GreaterThan);
            if (command.Equals("=", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Condition, (int)ConditionCommand.Equals);
            if (command.Equals("=", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Condition, (int)ConditionCommand.Equals);
            if (command.Equals("!=", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Condition, (int)ConditionCommand.NotEquals);

            return null;
        }

        private static BasePair ParseFlowCommand(string command)
        {
            if (command.Equals("start", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Flow, (int)FlowCommand.Start);
            if (command.Equals("stop", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Flow, (int)FlowCommand.Stop);
            if (command.Equals("cond", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Flow, (int)FlowCommand.Condition);

            return null;
        }

        private static BasePair ParseStoreCommand(string command)
        {
            if (command.Equals("store", System.StringComparison.InvariantCultureIgnoreCase))
                return new BasePair(BasePairType.Store, (int)StoreCommand.Store);

            return null;
        }

        private static BasePair ParseVariable(string value)
        {
            var variable = value;
            var type = BasePairType.Number;

            if (variable.StartsWith("*"))
            {
                type = BasePairType.StarNumber;
                variable = variable.Substring(1);
            }

            if (variable.StartsWith("."))
            {
                variable = variable.Substring(1);
                if (SystemVariables.Variables.ContainsKey(variable))
                    return new BasePair(type, SystemVariables.Variables[variable]);
            }

            var result = int.TryParse(variable, out var numberValue);

            if (result)
                return new BasePair(type, numberValue);
            return new BasePair(type, 0);
        }
    }
}