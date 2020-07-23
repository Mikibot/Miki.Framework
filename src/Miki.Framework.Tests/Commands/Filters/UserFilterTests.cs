﻿using Miki.Framework.Commands.Filters;
using System.Threading.Tasks;
using Miki.Discord.Common;
using Moq;
using Xunit;

namespace Miki.Framework.Tests.Commands.Filters
{
    public class UserFilterTests
    {
        private readonly UserFilter userFilter;

        private const long BannedUser = 0L;
        private const long ValidUser = 1L;

        public UserFilterTests()
        {
            userFilter = new UserFilter();
            userFilter.Users.Add(0L);
        }

        [Theory]
        [InlineData(BannedUser, false)]
        [InlineData(ValidUser, true)]
        public async Task UserIsAllowedTestAsync(long userId, bool allowed)
        {
            var isAllowed = await userFilter.CheckAsync(NewContext(userId));
            Assert.Equal(allowed, isAllowed);
        }

        private IContext NewContext(long val)
        {
            var author = new Mock<IDiscordUser>();
            author.Setup(x => x.Id)
                .Returns((ulong)val);

            var message = new Mock<IDiscordMessage>();
            message.Setup(x => x.Author)
                .Returns(author.Object);

            var context = new Mock<IContext>();
            context.Setup(x => x.GetContext("framework-message"))
                .Returns(message.Object);

            return context.Object;
        }
    }
}
