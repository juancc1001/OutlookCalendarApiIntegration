using Microsoft.Identity.Client;
using OutlookSyncApi.DTOs;

namespace OutlookSyncApi.Services.Interfaces
{
    public interface ICalendarService
    {
        List<CalendarEventDto> GetCalendarEvents();
        Task<CalendarEventDto> UpdateCalendarEvent(CalendarEventDto _event);
        Task DeleteCalendar(string id);
        Task<CalendarEventDto> CreateCalendarEvent(CalendarEventDto calendarEvent);
    }
}
