using ALife.Engines;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace ALife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly SimulationEngine engine;
        private readonly Renderer renderer;

        public MainWindow()
        {
            InitializeComponent();

            renderer = new Renderer(MainCanvas);
            engine = new SimulationEngine()
            {
                CycleCallback = renderer.DrawBots,
                AddBotCallback = renderer.AddBot
            };
            engine.CyclePerSecondChanged += Engine_CyclePerSecondChanged;
            MainCanvas.Width = engine.Field.Size.X;
            MainCanvas.Height = engine.Field.Size.Y;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CyclesPerSecond { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            engine.StopCycle();
        }

        private void Engine_CyclePerSecondChanged(object sender, int e)
        {
            CyclesPerSecond = e;
            Dispatcher.Invoke(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CyclesPerSecond)));
            });
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Task.Run(engine.RunCycle);
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            engine.StopCycle();
        }
    }
}