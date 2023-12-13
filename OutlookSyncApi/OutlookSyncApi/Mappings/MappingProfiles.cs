using AutoMapper;
using Microsoft.Graph.Models;
using OutlookSyncApi.DTOs;

namespace OutlookSyncApi.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Attendee, AttendeeDto>()
                .ForMember(atd => atd.Name, map => map.MapFrom(atd => (atd.EmailAddress != null) ? atd.EmailAddress.Name : ""))
                .ForMember(atd => atd.EmailAdress, map => map.MapFrom(atd => (atd.EmailAddress != null) ? atd.EmailAddress.Address : ""));

            CreateMap<Event, CalendarEventDto>()
                .ForMember(evt => evt.Id, map => map.MapFrom(_event => _event.Id))
                .ForMember(evt => evt.Start, map => map.MapFrom(_event => DateTime.Parse(_event.Start.DateTime)))
                .ForMember(evt => evt.End, map => map.MapFrom(_event => DateTime.Parse(_event.End.DateTime)))
                .ForMember(atd => atd.Subject, map => map.MapFrom(atd => atd.Subject))
                .ForMember(atd => atd.Description, map => map.MapFrom(atd => atd.BodyPreview.Replace(".", "")))
                .ForMember(atd => atd.Organizer, map => map.MapFrom(atd => atd.Organizer))
                .ForMember(atd => atd.OnlineMeetingUrl, map => map.MapFrom(atd => atd.OnlineMeetingUrl))
                .ReverseMap();
        }
    }
}
