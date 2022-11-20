﻿using Serilog;
using StreamDeckLib;
using StreamDeckLib.Messages;
using StreamDeckYeelightPlugin.Models;
using System;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.preset")]
    public class PresetAction : BaseStreamDeckActionWithSettingsModel<Models.PresetSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            int brightness = Math.Min(BrightnessSettingsModel.MAX_VALUE, Math.Max(BrightnessSettingsModel.MIN_VALUE, SettingsModel.Brightness));
            await yeelight.SetBrightness(brightness);

            int temperature = Math.Min(TemperatureSettingsModel.MAX_VALUE, Math.Max(TemperatureSettingsModel.MIN_VALUE, SettingsModel.Temperature));
            await yeelight.SetTemperature(temperature);

            if (SettingsModel.Ambient == 1)
            {
                await yeelight.ToggleAmbient();
            }

            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await base.OnWillAppear(args);
        }
    }
}
