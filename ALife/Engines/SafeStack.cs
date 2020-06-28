using System.Collections.Generic;

namespace ALife.Engines
{
    internal class SafeBoolStack
    {
        private readonly Stack<bool> stack = new Stack<bool>();

        public void Clear()
        {
            stack.Clear();
        }

        public bool Pop()
        {
            if (stack.Count == 0)
                return true;
            return stack.Pop();
        }

        public void Push(bool value)
        {
            stack.Push(value);
        }
    }

    internal class SafeIntStack
    {
        private readonly Stack<int> stack = new Stack<int>();

        public void Clear()
        {
            stack.Clear();
        }

        public int Pop()
        {
            if (stack.Count == 0)
                return 0;
            return stack.Pop();
        }

        public void Push(int value)
        {
            stack.Push(value);
        }
    }
}