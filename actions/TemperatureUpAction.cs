using StreamDeckLib;
using StreamDeckLib.Messages;
using StreamDeckYeelightPlugin.Models;
using System;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.temperatureUp")]
    public class TemperatureUpAction : BaseStreamDeckActionWithSettingsModel<Models.TemperatureSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            int oldTemperature = yeelight.GetProps().Temperature;
            int newTemperature = Math.Min(TemperatureSettingsModel.MAX_VALUE, oldTemperature + SettingsModel.Step);
            await yeelight.SetTemperature(newTemperature);

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
