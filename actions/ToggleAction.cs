using Serilog;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.toggle")]
    public class ToggleAction : BaseStreamDeckActionWithSettingsModel<Models.MainSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight == null)
            {
                await Manager.ShowAlertAsync(args.context);
                return;
            }

            await yeelight.Toggle();
            await SetState(yeelight, args);

            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            var yeelight = Yeelight.GetInstance(SettingsModel.Address);
            if (yeelight != null)
            {
                await SetState(yeelight, args);
            }
            await base.OnWillAppear(args);
        }

        private async Task<bool> SetState(Yeelight yeelight, StreamDeckEventPayload args)
        {
            var props = yeelight.GetProps();
            Log.Verbose(props.IsConnected.ToString());
            if (props.IsConnected)
            {
                Log.Verbose(props.Power);
                if (props.Power.Equals("on"))
                {
                    await Manager.SetStateAsync(args.context, 0);
                }
                else
                {
                    await Manager.SetStateAsync(args.context, 1);
                }
                return true;
            }
            return false;
        }
    }
}
