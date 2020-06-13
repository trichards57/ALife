using ALife.Model;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace ALife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Engine engine;
        private readonly Renderer renderer;

        public MainWindow()
        {
            InitializeComponent();

            renderer = new Renderer(MainCanvas);
            engine = new Engine()
            {
                CycleCallback = renderer.DrawBots
            };
            MainCanvas.Width = engine.Field.Size.X;
            MainCanvas.Height = engine.Field.Size.Y;
            DataContext = engine;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            engine.StopCycle();
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