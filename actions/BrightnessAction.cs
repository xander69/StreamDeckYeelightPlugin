using Serilog;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.brightness")]
    public class BrightnessAction : BaseStreamDeckActionWithSettingsModel<Models.BrightnessSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            int percent = Math.Min(100, Math.Max(1, SettingsModel.Percent));
            await yeelight.SetBrightness(percent);
            await Manager.SetTitleAsync(args.context, $"{SettingsModel.Percent}%");
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await Manager.SetTitleAsync(args.context, $"{SettingsModel.Percent}%");
            await base.OnWillAppear(args);
        }
    }
}
