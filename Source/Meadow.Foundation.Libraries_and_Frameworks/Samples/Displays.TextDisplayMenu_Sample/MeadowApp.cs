﻿using System;
using System.IO;
using System.Reflection;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Menu menu;
    //    St7789 st7789;

        Ssd1309 ssd1309;
        ITextDisplay display;

        IButton next = null;
        IButton previous = null;
        IButton select = null;

        public MeadowApp()
        {
            Initialize();
        }

        void UpdateDisplay()
        {

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            Console.WriteLine("Create Display with SPI...");

            /*
            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            st7789 = new St7789(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D12,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240); */

            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode0);

            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            ssd1309 = new Ssd1309
            (
                device: Device,
                spiBus: bus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            Console.WriteLine("Create GraphicsLibrary...");

            var gl = new GraphicsLibrary(ssd1309)
            {
                CurrentFont = new Font8x12(),
             //   Rotation = GraphicsLibrary.RotationType._270Degrees
            };

            gl.Clear();
            gl.DrawRectangle(1, 1, 40, 40, true, true);
            gl.Show();

            display = gl as ITextDisplay;

            Console.WriteLine("Load menu data...");

            var menuData = LoadResource("menu.json");

            Console.WriteLine($"Data length: {menuData.Length}...");

            Console.WriteLine("Create buttons...");

            Console.WriteLine("Create menu...");

            menu = new Menu(display, menuData, false);

            next = new PushButton(Device, Device.Pins.D10);
            next.Clicked += (s, e) => { menu.OnNext(); };

            select = new PushButton(Device, Device.Pins.D11);
            select.Clicked += (s, e) => { menu.OnSelect(); };

            previous = new PushButton(Device, Device.Pins.D12);
            previous.Clicked += (s, e) => { menu.OnPrevious(); };

            Console.WriteLine("Enable menu...");

            menu.Enable();
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Displays.TextDisplayMenu_Sample.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}