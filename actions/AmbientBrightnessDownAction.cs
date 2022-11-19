using StreamDeckLib;
using StreamDeckLib.Messages;
using StreamDeckYeelightPlugin.Models;
using System;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.ambientBrightnessDown")]
    public class AmbientBrightnessDownAction : BaseStreamDeckActionWithSettingsModel<Models.AmbientBrightnessSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            int oldBrightness = yeelight.GetProps().BackgroundBrightness;
            int newBrightness = Math.Max(AmbientBrightnessSettingsModel.MIN_VALUE, oldBrightness - SettingsModel.Step);
            await yeelight.SetAmbientBrightness(newBrightness);

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
