using StreamDeckLib;
using StreamDeckLib.Messages;
using StreamDeckYeelightPlugin.Models;
using System;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.brightnessDown")]
    public class BrightnessDownAction : BaseStreamDeckActionWithSettingsModel<Models.BrightnessSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            var props = yeelight.GetProps();
            int oldBrightness = props.ActiveMode == 1 ? props.NightBrightness : props.Brightness;
            int newBrightness = Math.Max(BrightnessSettingsModel.MIN_VALUE, oldBrightness - SettingsModel.Step);
            await yeelight.SetBrightness(newBrightness);

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
