using ALife.Engines;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ALife.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private readonly SimulationEngine engine;
        private readonly Renderer renderer;

        public MainPage()
        {
            InitializeComponent();

            renderer = new Renderer(MainCanvas);
            renderer.SelectedBotChanged += SelectedBotChanged;
            engine = new SimulationEngine(async a => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a()))
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

        public BotHolder SelectedBot { get; set; }

        private void Engine_CyclePerSecondChanged(object sender, int e)
        {
            CyclesPerSecond = e;
            var _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CyclesPerSecond))));
        }

        private void SelectedBotChanged(object sender, EventArgs e)
        {
            SelectedBot = renderer.SelectedBot;
            var _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBot))));
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