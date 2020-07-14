using ALife.Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ALife.Model
{
    public class Bot : INotifyPropertyChanged
    {
        public const float EyeAngle = 10 * (float)Math.PI / 360;
        public const int EyeCount = 3;
        public const int VisionLimit = 200;

        private readonly Func<Action, Task> invokeEvent;
        private Color color;
        private IReadOnlyList<float> eyeDistances = new float[EyeCount];
        private Vector2 force;
        private bool isFixed = false;
        private float mass = 10;
        private float orientation;
        private Vector2 position;
        private float radius = 10;
        private Vector2 speed;

        public Bot(Func<Action, Task> invokeEvent = null)
        {
            this.invokeEvent = invokeEvent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Color Color { get => color; set { color = value; RaisePropertyChanged(); } }
        public IList<BasePair> DNA { get; set; }
        public IReadOnlyList<float> EyeDistances { get => eyeDistances; internal set { eyeDistances = value; RaisePropertyChanged(); } }
        public IReadOnlyList<IList<EyeEntry>> Eyes { get; } = new List<IList<EyeEntry>>(Enumerable.Range(0, EyeCount).Select(i => new List<EyeEntry>()));
        public Vector2 Force { get => force; set { force = value; RaisePropertyChanged(); } }
        public bool IsFixed { get => isFixed; set { isFixed = value; RaisePropertyChanged(); } }
        public float Mass { get => mass; set { mass = value; RaisePropertyChanged(); } }
        public IList<int> Memory { get; } = new int[SystemVariables.MemoryLength];
        public float Orientation { get => orientation; set { orientation = value; RaisePropertyChanged(); } }
        public Vector2 Position { get => position; set { position = value; RaisePropertyChanged(); } }
        public IReadOnlyList<int> PreviousMemory { get; private set; }
        public float Radius { get => radius; set { radius = value; RaisePropertyChanged(); } }
        public Vector2 Speed { get => speed; set { speed = value; RaisePropertyChanged(); } }

        public int GetFromMemory(MemoryAddresses address)
        {
            return Memory[(int)address];
        }

        public void SetMemory(MemoryAddresses address, int value)
        {
            Memory[(int)address] = value;
        }

        public void SetMemory(MemoryAddresses address, float value)
        {
            Memory[(int)address] = (int)Math.Round(value);
        }

        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            if (invokeEvent != null)
                invokeEvent(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)));
            else
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
