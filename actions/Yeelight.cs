using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using YeelightAPI;

namespace StreamDeckYeelightPlugin
{
    public class Yeelight
    {

        private static readonly Dictionary<string, Yeelight> instances = new();
        private static readonly object syncRoot = new();
        private readonly Device device;

        private Yeelight(Device device)
        {
            this.device = device;
        }

        public YeelightProps GetProps()
        {
            return YeelightProps.Extract(device);
        }


        public async Task<bool> Toggle()
        {
            Log.Debug("Toggle");
            return await device.Toggle();
        }

        public async Task<bool> ToggleNightLight()
        {
            int activeMode = Convert.ToInt32(device.Properties["active_mode"]);
            if (activeMode == 1)
            {
                Log.Debug("Toggle to daylight");
                int ct = Convert.ToInt32(device.Properties["ct"]);
                return await device.SetColorTemperature(ct);
            }
            else
            {
                Log.Debug("Toggle to nightlight");
                return await device.SetPower(true, 0, YeelightAPI.Models.PowerOnMode.Night);
            }
        }

        public async Task<bool> ToggleAmbient()
        {
            Log.Debug("Toggle ambient light");
            return await device.BackgroundToggle();
        }

        public async Task<bool> SetBrightness(int brightness)
        {
            Log.Debug($"Set brightness to {brightness}%");
            return await device.SetBrightness(brightness);
        }

        public async Task<bool> SetAmbientBrightness(int brightness)
        {
            Log.Debug($"Set ambient light brightness to {brightness}%");
            return await device.BackgroundSetBrightness(brightness);
        }

        public async Task<bool> SetTemperature(int temperature)
        {
            Log.Debug($"Set temperature to {temperature}K");
            return await device.SetColorTemperature(temperature);
        }

        public static Yeelight? GetInstance(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                Log.Error("Address is empty");
                return null;
            }
            if (!instances.ContainsKey(address))
            {
                lock (syncRoot)
                {
                    Log.Debug("Connect to " + address + "...");
                    Device device = new(address);
                    Yeelight yeelight = new(device);
                    Task<bool> task = device.Connect();
                    int attempts = 0;
                    while (!device.IsConnected)
                    {
                        Log.Verbose($"Attempts #{attempts}");
                        Thread.Sleep(100);
                        attempts++;
                        if (attempts == 10)
                        {
                            break;
                        }
                    }
                    if (device.IsConnected)
                    {
                        instances.Add(address, yeelight);
                        Log.Debug("Connected");
                    }
                    else
                    {
                        Log.Error("Cannot connect");
                    }
                }
            }
            return instances[address];
        }
    }
}
