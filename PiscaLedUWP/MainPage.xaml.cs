using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PiscaLedUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int Pino = 5; //Aqui defino qual o pino da Raspberry que irei utilizar

        private GpioPin _pin;
        private GpioPinValue _pinValue;
        private readonly SolidColorBrush _redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private readonly SolidColorBrush _grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();

            var timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
            timer.Tick += Timer_Tick;

            InitGpio();

            timer.Start();
        }

        /// <summary>
        /// Efetuo a inicialização do pino GPIO selecionado da Raspberry Pi
        /// </summary>
        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                _pin = null;
                GpioStatus.Text = "Não existe um controlador GPIO nesse disposito!!";

                return;
            }

            _pin = gpio.OpenPin(Pino); //Abro uma conexão ao pino selecionado
            _pinValue = GpioPinValue.High; //Seto seu valor para nível lógico alto (1)
            _pin.Write(_pinValue); //Escrevo o valor do pino
            _pin.SetDriveMode(GpioPinDriveMode.Output); //Defino o modo do pino como saída por ser um LED. Exemplo de entrada é se fosse um botão!
            
            GpioStatus.Text = $"O pino {Pino} GPIO foi inicializado corretamente!";
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_pinValue == GpioPinValue.High)
            {
                _pinValue = GpioPinValue.Low;
                _pin.Write(_pinValue);
                LED.Fill = _redBrush;
            }
            else
            {
                _pinValue = GpioPinValue.High;
                _pin.Write(_pinValue);
                LED.Fill = _grayBrush;
            }
        }
    }
}
