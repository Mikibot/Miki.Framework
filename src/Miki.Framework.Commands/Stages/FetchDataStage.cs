﻿using System;
using System.Threading.Tasks;
using Miki.Discord.Common;
using Miki.Framework.Commands.Pipelines;
using Miki.Framework.Commands.Stages;

namespace Miki.Framework.Commands.Stages
{
    public class FetchDataStage : IPipelineStage
    {
        public static string ChannelArgumentKey = "framework-channel";
        public static string GuildArgumentKey = "framework-guild";

        public async ValueTask CheckAsync(IDiscordMessage data, IMutableContext e, Func<ValueTask> next)
        {
            var channel = await e.GetMessage().GetChannelAsync();
            if(channel == null)
            {
                throw new InvalidOperationException("This channel is not supported");
            }
            e.SetContext(ChannelArgumentKey, channel);
            if(channel is IDiscordGuildChannel gc)
            {
                e.SetContext(GuildArgumentKey, await gc.GetGuildAsync());
            }
            await next();
        }
    }
}

namespace Miki.Framework
{
    public static class FetchDataStageExtensions
    {
        public static IDiscordTextChannel GetChannel(this IContext context)
        {
            return context.GetContext<IDiscordTextChannel>(FetchDataStage.ChannelArgumentKey);
        }

        public static IDiscordGuild GetGuild(this IContext context)
        {
            return context.GetContext<IDiscordGuild>(FetchDataStage.GuildArgumentKey);
        }
    }
}
