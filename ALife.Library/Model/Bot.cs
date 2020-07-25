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
        public const int EyeCount = 7;
        public const int VisionLimit = 200;

        private readonly CachedValue<Vector2?, float, Vector2> focussedBotRelativeSpeed;
        private readonly Func<Action, Task> invokeEvent;
        private readonly CachedValue<Vector2, float, Vector2> relativeSpeed;
        private Color color;
        private IReadOnlyList<float> eyeDistances = new float[EyeCount];
        private Bot focussedBot;
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
            relativeSpeed = new CachedValue<Vector2, float, Vector2>(() => speed, () => orientation, (s, o) =>
            {
                var rotation = Matrix3x2.CreateRotation(-o);
                return Vector2.Transform(s, rotation);
            });
            focussedBotRelativeSpeed = new CachedValue<Vector2?, float, Vector2>(() => focussedBot == null ? (Vector2?)null : focussedBot.Speed - Speed, () => orientation, (s, o) =>
            {
                if (s.HasValue)
                {
                    var rotation = Matrix3x2.CreateRotation(-o);
                    return Vector2.Transform(s.Value, rotation);
                }
                return new Vector2(0, 0);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Color Color { get => color; set { color = value; RaisePropertyChanged(); } }
        public IList<BasePair> DNA { get; set; } = new List<BasePair>();
        public IReadOnlyList<float> EyeDistances { get => eyeDistances; internal set { eyeDistances = value; RaisePropertyChanged(); } }
        public int EyeRefCount => DNA.Count(bp => bp.Type == BasePairType.StarNumber && bp.Command >= (int)MemoryAddresses.EyeFirst && bp.Command <= (int)MemoryAddresses.EyeLast);
        public IReadOnlyList<IList<EyeEntry>> Eyes { get; } = new List<IList<EyeEntry>>(Enumerable.Range(0, EyeCount).Select(i => new List<EyeEntry>()));
        public int FocusEye { get; } = EyeCount / 2;
        public Bot FocussedBot { get => focussedBot; set { focussedBot = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(FocussedBotRelativeSpeed)); } }
        public Vector2 FocussedBotRelativeSpeed => focussedBotRelativeSpeed;
        public Vector2 Force { get => force; set { force = value; RaisePropertyChanged(); } }
        public bool IsFixed { get => isFixed; set { isFixed = value; RaisePropertyChanged(); } }
        public float Mass { get => mass; set { mass = value; RaisePropertyChanged(); } }
        public IList<int> Memory { get; } = new int[SystemVariables.MemoryLength];
        public float Orientation { get => orientation; set { orientation = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(RelativeSpeed)); RaisePropertyChanged(nameof(FocussedBotRelativeSpeed)); } }
        public Vector2 Position { get => position; set { position = value; RaisePropertyChanged(); } }
        public IReadOnlyList<int> PreviousMemory { get; private set; }
        public float Radius { get => radius; set { radius = value; RaisePropertyChanged(); } }
        public Vector2 RelativeSpeed => relativeSpeed;
        public Vector2 Speed { get => speed; set { speed = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(RelativeSpeed)); RaisePropertyChanged(nameof(FocussedBotRelativeSpeed)); } }

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

        public void SetMemory(MemoryAddresses address, int offset, int value)
        {
            Memory[(int)address + offset] = value;
        }

        public void SetMemory(MemoryAddresses address, int offset, float value)
        {
            Memory[(int)address + offset] = (int)Math.Round(value);
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
