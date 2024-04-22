using AutoMapper;
using UserAuth.Dtos;
using UserAuth.Models;

namespace UserAuth.Helpers
{
	public class MappingProfiles: Profile
	{
        public MappingProfiles()
        {
            CreateMap<Users, UsersDto>();
			//POST and PUT
			CreateMap<UsersDto, Users>();
		}
    }
}
