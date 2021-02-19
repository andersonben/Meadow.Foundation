using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.ePaper;
using Meadow.Foundation.Graphics;

namespace Displays.ePaper.IL0398_Sample
{
    /* Driver in development */
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Il0398 display;

        public MeadowApp()
        {
            Console.WriteLine("ePaper sample");
            Console.WriteLine("Create Spi bus");

            var spiBus = Device.CreateSpiBus();// Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, 2000);

            Console.WriteLine("Create display driver instance");
            display = new Il0398(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D12,
                dcPin: Device.Pins.D13,
                resetPin: Device.Pins.D14,
                busyPin: Device.Pins.D15,
                width: 400,
                height: 300);

            var graphics = new GraphicsLibrary(display);

            //any color but black will show the ePaper alternate color 
            graphics.DrawRectangle(0, 0, 399, 32, Meadow.Foundation.Color.Red, true);

            display.SetPenColor(Meadow.Foundation.Color.Black);

            graphics.CurrentFont = new Font8x12();
            graphics.DrawText(5, 5, "IL0398");
            graphics.DrawText(5, 20, "Meadow F7");

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(180, 2, "Benjamin Anderson");

            int ySpacing = 6;

            for (int i = 0; i < 2; i++)
            {
                graphics.DrawLine(0, 70 + ySpacing * i, 20, 50 + ySpacing * i, true);
                graphics.DrawLine(20, 50 + ySpacing * i, 40, 70 + ySpacing * i, true);
                graphics.DrawLine(40, 70 + ySpacing * i, 60, 50 + ySpacing * i, true);
                graphics.DrawLine(60, 50 + ySpacing * i, 80, 70 + ySpacing * i, true);
                graphics.DrawLine(80, 70 + ySpacing * i, 100, 50 + ySpacing * i, true);
                graphics.DrawLine(100, 50 + ySpacing * i, 120, 70 + ySpacing * i, true);
            }

            Console.WriteLine("Show");

            graphics.Show();
        }
    }
}