using AutoMapper;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.VisualStudio.Services.Common;
using OutlookSyncApi.DTOs;
using TimeZoneConverter;

namespace GraphHelper
{
    public class GraphCalendarHelper
    {
        private static GraphServiceClient _graphClient;
        private static IMapper _mapper;


        public static void Initialize(IAuthenticationProvider authProvider, IMapper mapper)
        {
            _graphClient = new GraphServiceClient(authProvider);
            _mapper = mapper;
        }

        public static async Task<Microsoft.Graph.Models.User> GetMeAsync()
        {
            try
            {
                return await _graphClient.Me
                    .GetAsync();

            }
            catch (ServiceException ex)
            {
                return null;
            }
        }

        public static async Task<IEnumerable<Event>?> GetCalendarViewAsync(
            DateTime today,
            string timeZone,
            int numberOfDays)
        {
            if (numberOfDays > 31)
                return null;

            var startOfWeek = GetUtcStartOfWeekInTimeZone(today, timeZone);
            var endOfWeek = startOfWeek.AddDays(numberOfDays);

            try
            {
                var events = await _graphClient.Me
                    .CalendarView
                    .GetAsync(
                    requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.StartDateTime = startOfWeek.ToString("o");
                        requestConfiguration.QueryParameters.EndDateTime = endOfWeek.ToString("o");
                        requestConfiguration.Headers.Add("Prefer", $"outlook.timezone=\"{timeZone}\"");
                        requestConfiguration.QueryParameters.Top = 50;
                        requestConfiguration.QueryParameters.Orderby = new[] { "start/dateTime" };
                    });

                return events?.Value?.ToList();
            }
            catch (ServiceException ex)
            {
                return null;
            }
        }

        public static async Task<Event> EditCalendarEvent(CalendarEventDto _event)
        {
            var eventOld = await _graphClient.Me.Events[_event.Id].GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "subject", "bodyPreview", "organizer", "attendees", "start", "end", "onlineMeetingUrl" };
                requestConfiguration.Headers.Add("Prefer", "outlook.timezone=\"Pacific Standard Time\"");
            });

            if(eventOld == null)
            {
                throw new ArgumentException("evento invalido");
            }

            List<Attendee> attendees = new List<Attendee>();
            _event.Attendees.ForEach(atd => new Attendee() { EmailAddress = new EmailAddress(){
                Address = atd.EmailAdress,
                Name = atd.Name,
            }});

            eventOld.BodyPreview = _event.Description;
            eventOld.Start.DateTime = _event.Start?.ToString("o");
            eventOld.Start.TimeZone = "America/Argentina/Buenos_Aires";
            eventOld.End.TimeZone = "America/Argentina/Buenos_Aires";
            eventOld.End.DateTime = _event.End?.ToString("o");
            eventOld.BodyPreview = _event.Description;
            eventOld.Subject = _event.Subject;
            eventOld.Attendees = attendees;
            eventOld.Organizer.EmailAddress.Address = _event.Organizer.EmailAdress;
            eventOld.Organizer.EmailAddress.Name = _event.Organizer.Name;
            eventOld.OnlineMeetingUrl = _event.OnlineMeetingUrl;


            return await _graphClient.Me
                .Events[_event.Id]
                .PatchAsync(eventOld);
        }

        public static async Task<Event> CreateCalendarEvent(CalendarEventDto calendarEventDto)
        {
            List<Attendee> attendees = new List<Attendee>();
            calendarEventDto.Attendees.ToList().ForEach(atd => attendees.Add(new Attendee()
            {
                EmailAddress = new EmailAddress()
                {
                    Address = atd.EmailAdress,
                    Name = atd.Name
                }
            }));

            var _event = new Event()
            {
                Attendees = attendees,
                Organizer = new Recipient()
                {
                    EmailAddress = new EmailAddress() { Address = calendarEventDto.Organizer.EmailAdress, Name = calendarEventDto.Organizer.Name }
                },
                Subject = calendarEventDto.Subject,
                BodyPreview = calendarEventDto.Description,
                Start = new DateTimeTimeZone(){
                    DateTime = calendarEventDto.Start?.ToString("o"),
                    TimeZone = calendarEventDto.Timezone
                },
                End = new DateTimeTimeZone(){
                    DateTime = calendarEventDto.End?.ToString("o"),
                    TimeZone = calendarEventDto.Timezone
                },
                OnlineMeetingUrl = calendarEventDto.OnlineMeetingUrl
            };


            var response  = await _graphClient.Me.Events.PostAsync(_event);
            return response;
        }

        public static async Task DeleteCalendarEvent(string id)
        {
            await _graphClient.Me.Events[id].Cancel.PostAsync(new Microsoft.Graph.Me.Events.Item.Cancel.CancelPostRequestBody());
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today,
            string timeZoneId)
        {
            TimeZoneInfo userTimeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
            int diff = System.DayOfWeek.Sunday - today.DayOfWeek;
            var unspecifiedStart = DateTime.SpecifyKind(
                today.AddDays(diff), DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, userTimeZone);
        }
    }
}