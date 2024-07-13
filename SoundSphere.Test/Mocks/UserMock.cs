using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using static SoundSphere.Test.Mocks.AuthorityMock;
using static SoundSphere.Test.Mocks.RoleMock;

namespace SoundSphere.Test.Mocks
{
    public class UserMock
    {
        private UserMock() { }

        public static IList<User> GetUsers() => [GetUser1(), GetUser2()];

        public static IList<UserDto> GetUserDtos() => GetUsers().Select(ToDto).ToList();

        public static User GetUser1() => new()
        {
            Id = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
            Name = "user_name1",
            Email = "user_email1@email.com",
            Password = "#User1_password!",
            Mobile = "+40700000000",
            Address = "user_address1",
            Birthday = new DateOnly(2000, 1, 1),
            Avatar = "https://user1-avatar.jpg",
            Role = GetRole1(),
            Authorities = GetAuthoritiesAdmin(),
            CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static User GetUser2() => new()
        {
            Id = Guid.Parse("7eb88892-549b-4cae-90be-c52088354643"),
            Name = "user_name2",
            Email = "user_email2@email.com",
            Password = "#User2_password!",
            Mobile = "+40700000001",
            Address = "user_address2",
            Birthday = new DateOnly(2000, 1, 2),
            Avatar = "https://user2-avatar.jpg",
            Role = GetRole2(),
            Authorities = GetAuthoritiesModerator(),
            CreatedAt = new DateTime(2024, 7, 2, 0, 0, 0),
            UpdatedAt = null,
            DeletedAt = null
        };

        public static UserDto GetUserDto1() => ToDto(GetUser1());

        public static UserDto GetUserDto2() => ToDto(GetUser2());

        public static UserDto ToDto(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.Address,
            Birthday = user.Birthday,
            Avatar = user.Avatar,
            RoleId = user.Role.Id,
            AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt
        };
    }
}