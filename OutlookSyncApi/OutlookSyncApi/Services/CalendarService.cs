using System;
using AutoMapper;
using Graph.Authentication;
using GraphHelper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using OutlookSyncApi.DTOs;
using OutlookSyncApi.Services.Interfaces;

namespace User.Calendar.API.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly string[] _scopes;
        private readonly string _appId;
        private readonly string _numberCalendarDays;
        private DeviceCodeAuthProvider _authProvider;
        private readonly IMapper _mapper;

        public CalendarService(IConfiguration appConfiguration, IMapper mapper, IHttpContextAccessor context)
        {
            _mapper = mapper;
            string scopes = appConfiguration.GetValue<string>("AzureAd:Scopes");
            _scopes = scopes.Split(";");
            _appId = appConfiguration.GetValue<string>("AzureAd:ClientId");
            _numberCalendarDays = "14";
            _authProvider = new DeviceCodeAuthProvider(_appId, _scopes, context);
        }

        public List<CalendarEventDto> GetCalendarEvents()
        {
            GraphHelper.GraphCalendarHelper.Initialize(_authProvider, _mapper);

            var user = GraphHelper.GraphCalendarHelper.GetMeAsync().Result;

            var events = ListCalendarEvents(
                user.MailboxSettings?.TimeZone ?? "America/Argentina/Buenos_Aires",
                System.Convert.ToInt16(_numberCalendarDays));

            return events;
        }

        public List<CalendarEventDto> ListCalendarEvents(
            string userTimeZone,
            int numberOfDays)
        {
            var events = GraphCalendarHelper
                .GetCalendarViewAsync(
                    DateTime.Today,
                    userTimeZone,
                    numberOfDays)
                .Result?.ToList();

            List<CalendarEventDto> resultado = new List<CalendarEventDto>();
            events?.ForEach(_event => resultado.Add(this.MapEventToDto(_event)));

            return resultado;
        }

        public async Task<CalendarEventDto> UpdateCalendarEvent(CalendarEventDto _event)
        {
            //GraphCalendarHelper
            return this.MapEventToDto(await GraphCalendarHelper.EditCalendarEvent(_event));
        }

        public async Task<CalendarEventDto> CreateCalendarEvent(CalendarEventDto calendarEvent)
        {
            var response =  await GraphCalendarHelper.CreateCalendarEvent(calendarEvent);
            return this.MapEventToDto(response);
        }

        public async Task DeleteCalendar(string id)
        {
            await GraphCalendarHelper.DeleteCalendarEvent(id);
        }

        public CalendarEventDto MapEventToDto(Event _event){

            List<AttendeeDto>? attendees = null;

            if (_event.Attendees != null) attendees = _mapper.Map<List<AttendeeDto>>(_event.Attendees);

            return new CalendarEventDto()
            {
                Id = _event.Id,
                Attendees = attendees,
                Description = _event.BodyPreview.Replace(".",""),
                Subject = _event.Subject,
                Start = DateTime.Parse(_event.Start?.DateTime),
                End = DateTime.Parse(_event.End?.DateTime),
                Timezone = _event.Start.TimeZone,
                Organizer = new AttendeeDto()
                {
                    Name = _event.Organizer?.EmailAddress?.Name ?? "",
                    EmailAdress = _event.Organizer?.EmailAddress?.Address ?? ""
                },
                OnlineMeetingUrl = _event.OnlineMeetingUrl,
            };
        }
    }
}