using System;

namespace ALife.Library.Model
{
    public class CachedValue<TIn1, TIn2, TOut>
    {
        private readonly Func<TIn1> getInput1;
        private readonly Func<TIn2> getInput2;
        private readonly Func<TIn1, TIn2, TOut> getOutput;
        private TIn1 lastInput1;
        private TIn2 lastInput2;
        private TOut lastOutput;

        public CachedValue(Func<TIn1> getInput1, Func<TIn2> getInput2, Func<TIn1, TIn2, TOut> getOutput)
        {
            this.getInput1 = getInput1;
            this.getInput2 = getInput2;
            this.getOutput = getOutput;
        }

        public static implicit operator TOut(CachedValue<TIn1, TIn2, TOut> value)
        {
            return value.GetValue();
        }

        public TOut GetValue()
        {
            var currentInput1 = getInput1();
            var currentInput2 = getInput2();

            if (!currentInput1.Equals(lastInput1) || !currentInput2.Equals(lastInput2))
            {
                lastInput1 = currentInput1;
                lastInput2 = currentInput2;
                lastOutput = getOutput(currentInput1, currentInput2);
            }

            return lastOutput;
        }
    }
}