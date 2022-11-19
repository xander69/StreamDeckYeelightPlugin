using StreamDeckLib;
using StreamDeckLib.Messages;
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

            int oldBrightness = yeelight.GetProps().Brightness;
            int newBrightness = Math.Max(1, oldBrightness - SettingsModel.Step);
            await yeelight.SetBrightness(newBrightness);

            await Manager.SetTitleAsync(args.context, $"-{SettingsModel.Step}%");
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await Manager.SetTitleAsync(args.context, $"-{SettingsModel.Step}%");
            await base.OnWillAppear(args);
        }
    }
}
