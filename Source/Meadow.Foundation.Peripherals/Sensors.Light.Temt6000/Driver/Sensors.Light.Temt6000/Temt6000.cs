﻿using Meadow.Hardware;
using System.Threading.Tasks;

namespace Meadow.Foundation.Sensors.Light
{
    public class Temt6000
    {
        

        /// <summary>
        ///     Analog port connected to the sensor.
        /// </summary>
        private readonly IAnalogInputPort sensor;

        /// <summary>
        ///     Voltage being output by the sensor.
        /// </summary>
        public Task<float> GetVoltage()
        {
            return sensor.Read();
        }

        

        

        /// <summary>
        ///     Create a new light sensor object using a static reference voltage.
        /// </summary>
        /// <param name="pin">AnalogChannel connected to the sensor.</param>
        public Temt6000(IIODevice device, IPin pin)
        {
            sensor = device.CreateAnalogInputPort(pin);
        }

        
    }
}