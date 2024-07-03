using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class AuthorityMappingExtensions
    {
        public static IList<AuthorityDto> ToDtos(this IList<Authority> authorities) => authorities.Select(authority => authority.ToDto()).ToList();

        public static IList<Authority> ToEntities(this IList<AuthorityDto> authorityDtos) => authorityDtos.Select(authorityDto => authorityDto.ToEntity()).ToList();

        public static AuthorityDto ToDto(this Authority authority) => new AuthorityDto { Id = authority.Id, Type = authority.Type, CreatedAt = authority.CreatedAt };

        public static Authority ToEntity(this AuthorityDto authorityDto) => new Authority { Id = authorityDto.Id, Type = authorityDto.Type, CreatedAt = authorityDto.CreatedAt };
    }
}