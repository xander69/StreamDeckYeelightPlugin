﻿using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StreamDeckYeelightPlugin
{
    [ActionUuid(Uuid = "com.xander.yeelight.DefaultPluginAction")]
    public class YeelightPluginAction : BaseStreamDeckActionWithSettingsModel<Models.CounterSettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            SettingsModel.Counter++;
            await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
            //update settings
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
            await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await base.OnWillAppear(args);
            await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
        }

    }
}
