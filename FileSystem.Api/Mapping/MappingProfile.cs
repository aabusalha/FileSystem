
using AutoMapper;
using FileSystem.Api.Helpers;
using FileSystem.Api.Resources;

using FileSystem.Core.Models;

namespace FileSystem.Api.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {

            //Domain to Resources

            CreateMap<File, FileRes>();


            //End


            //Resources to Domain

            CreateMap<SavefileRes, File>();
            CreateMap<EditFileRes, File>();

            CreateMap<AttachmentRes, Attachment>();
            CreateMap<Attachment, AttachmentRes>()
                .ForMember(u => u.DownloadURL,
                opt => opt.MapFrom(ur => AppHttpContex.AppBaseUrl + "/Storage/" + ur.Id));



        }
    }
}