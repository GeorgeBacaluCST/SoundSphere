using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Test.Mocks
{
    public class AuthorityMock
    {
        private AuthorityMock() { }

        public static IList<Authority> GetAuthoritiesAdmin() => [GetAuthority1(), GetAuthority2(), GetAuthority3(), GetAuthority4()];

        public static IList<Authority> GetAuthoritiesModerator() => [GetAuthority1(), GetAuthority2(), GetAuthority3()];

        public static IList<Authority> GetAuthoritiesListener() => [GetAuthority1()];

        public static IList<AuthorityDto> GetAuthorityDtosAdmin() => GetAuthoritiesAdmin().Select(ToDto).ToList();

        public static IList<AuthorityDto> GetAuthorityDtosModerator() => GetAuthoritiesModerator().Select(ToDto).ToList();

        public static IList<AuthorityDto> GetAuthorityDtosListener() => GetAuthoritiesListener().Select(ToDto).ToList();

        public static Authority GetAuthority1() => new() { Id = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd"), Type = AuthorityType.Create, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 0) };

        public static Authority GetAuthority2() => new() { Id = Guid.Parse("362b20cf-3636-49ed-9489-d2700339efce"), Type = AuthorityType.Read, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 1) };

        public static Authority GetAuthority3() => new() { Id = Guid.Parse("3cc47e2d-b14e-472f-9868-fbb90b15f18e"), Type = AuthorityType.Update, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 2) };

        public static Authority GetAuthority4() => new() { Id = Guid.Parse("59525baa-6eaa-42d8-b213-9094af0d604b"), Type = AuthorityType.Delete, CreatedAt = new DateTime(2024, 7, 1, 0, 0, 3) };

        public static AuthorityDto GetAuthorityDto1() => ToDto(GetAuthority1());

        public static AuthorityDto GetAuthorityDto2() => ToDto(GetAuthority2());

        public static AuthorityDto GetAuthorityDto3() => ToDto(GetAuthority3());

        public static AuthorityDto GetAuthorityDto4() => ToDto(GetAuthority4());

        public static AuthorityDto ToDto(Authority authority) => new() { Id = authority.Id, Type = authority.Type, CreatedAt = authority.CreatedAt };
    }
}