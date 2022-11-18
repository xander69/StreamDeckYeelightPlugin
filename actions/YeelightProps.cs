using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using YeelightAPI;

namespace StreamDeckYeelightPlugin
{
    public class YeelightProps
    {
        public string Hostname { get; set; }
        public string Id { get; set; }
        public bool IsConnected { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public string Power { get; set; }
        public int Brightness { get; set; }
        public int ColorMode { get; set; }
        public int Temperature { get; set; }
        public long RGB { get; set; }
        public long HUE { get; set; }
        public long SAT { get; set; }
        public int Flowing { get; set; }
        public int Delayoff { get; set; }
        public string FlowParams { get; set; }
        public int MusicOn { get; set; }
        public string BackgroundPower { get; set; }
        public int BackgroundFlowing { get; set; }
        public string BackgroundFlowParams { get; set; }
        public int BackgroundTemperature { get; set; }
        // 1: rgb mode / 2: color temperature mode / 3: hsv mode
        public int BackgroundLMode { get; set; }
        public int BackgroundBrightness { get; set; }
        public long BackgroundRGB { get; set; }
        public long BackgroundHUE { get; set; }
        public long BackgroundSAT { get; set; }
        public int NightBrightness { get; set; }
        // 0: daylight mode / 1: moonlight mode (ceiling light only)
        public int ActiveMode { get; set; }

        public void LogProps()
        {
            Log.Verbose($"""
                Hostname: {Hostname}
                Id: {Id}
                Model: {Model}
                Name: {Name}
                Port: {Port}
                IsConnected: {IsConnected}
                Power: {Power}
                Brightness: {Brightness}
                ColorMode: {ColorMode}
                Temperature: {Temperature}
                RGB: {RGB}
                HUE: {HUE}
                SAT: {SAT}
                Flowing: {Flowing}
                Delayoff: {Delayoff}
                FlowParams: {FlowParams}
                MusicOn: {MusicOn}
                BackgroundPower: {BackgroundPower}
                BackgroundFlowing: {BackgroundFlowing}
                BackgroundFlowParams: {BackgroundFlowParams}
                BackgroundTemperature: {BackgroundTemperature}
                BackgroundLMode: {BackgroundLMode}
                BackgroundBrightness: {BackgroundBrightness}
                BackgroundRGB: {BackgroundRGB}
                BackgroundHUE: {BackgroundHUE}
                BackgroundSAT: {BackgroundSAT}
                NightBrightness: {NightBrightness}
                ActiveMode: {ActiveMode}
                """);
        }

        public static YeelightProps Extract(Device device)
        {
            YeelightProps props = new()
            {
                Hostname = device.Hostname,
                Id = device.Id,
                Model = device.Model.ToString(),
                Name = device.Name,
                Port = device.Port,
                IsConnected = device.IsConnected
            };
            if (device.IsConnected)
            {
                props.Power = device.Properties["power"].ToString();
                props.Brightness = Convert.ToInt32(nvl(device.Properties["bright"].ToString(), "0"));
                props.ColorMode = Convert.ToInt32(nvl(device.Properties["color_mode"].ToString(), "0"));
                props.Temperature = Convert.ToInt32(nvl(device.Properties["ct"].ToString(), "0"));
                props.RGB = Convert.ToInt64(nvl(device.Properties["rgb"].ToString(), "0"));
                props.HUE = Convert.ToInt64(nvl(device.Properties["hue"].ToString(), "0"));
                props.SAT = Convert.ToInt64(nvl(device.Properties["sat"].ToString(), "0"));
                props.Flowing = Convert.ToInt32(nvl(device.Properties["flowing"].ToString(), "0"));
                props.Delayoff = Convert.ToInt32(nvl(device.Properties["delayoff"].ToString(), "0"));
                props.FlowParams = device.Properties["flow_params"].ToString();
                props.MusicOn = Convert.ToInt32(nvl(device.Properties["music_on"].ToString(), "0"));
                props.BackgroundPower = device.Properties["bg_power"].ToString();
                props.BackgroundFlowing = Convert.ToInt32(nvl(device.Properties["bg_flowing"].ToString(), "0"));
                props.BackgroundFlowParams = device.Properties["bg_flow_params"].ToString();
                props.BackgroundTemperature = Convert.ToInt32(nvl(device.Properties["bg_ct"].ToString(), "0"));
                props.BackgroundLMode = Convert.ToInt32(nvl(device.Properties["bg_lmode"].ToString(), "0"));
                props.BackgroundBrightness = Convert.ToInt32(device.Properties["bg_bright"].ToString());
                props.BackgroundRGB = Convert.ToInt64(nvl(device.Properties["bg_rgb"].ToString(), "0"));
                props.BackgroundHUE = Convert.ToInt64(nvl(device.Properties["bg_hue"].ToString(), "0"));
                props.BackgroundSAT = Convert.ToInt64(nvl(device.Properties["bg_sat"].ToString(), "0"));
                props.NightBrightness = Convert.ToInt32(nvl(device.Properties["nl_br"].ToString(), "0"));
                props.ActiveMode = Convert.ToInt32(nvl(device.Properties["active_mode"].ToString(), "0"));
            }
            return props;
        }

        private static string nvl(string props, string defaultProps) => string.IsNullOrEmpty(props) ? defaultProps : props;
    }
}
