using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Test
{
    public class UserTest
    {
        [Fact]
        public void Give_me_success_when_user_is_manager()
        {
            var user = new User(UserPositionEnum.Manager);
            Assert.True(user.IsManager());
        }

        [Fact]
        public void Give_me_false_when_user_is_not_manager()
        {
            var user = new User(UserPositionEnum.Employee);
            Assert.False(user.IsManager());
        }
    }
}