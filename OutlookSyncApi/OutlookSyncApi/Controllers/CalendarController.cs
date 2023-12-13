using Microsoft.AspNetCore.Mvc;
using OutlookSyncApi.DTOs;
using OutlookSyncApi.Services.Interfaces;

[ApiController]
[Route("/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private readonly ILogger<CalendarController> _logger;
    private readonly IHttpContextAccessor _context;
    public CalendarController(
        ILogger<CalendarController> logger,
        ICalendarService calendarService,
        IHttpContextAccessor ctx)
    {
        _logger = logger;
        _calendarService = calendarService;
        _context = ctx;
    }

    [HttpGet("/GetWeekEvents")]
    public ActionResult GetCalendarEvents()
    {
        var token = _context.HttpContext.Session.GetString("_accessToken");
        if (token == null) 
        {
            return Unauthorized();
        }

        var calendarEvents =  _calendarService.GetCalendarEvents();
        if (calendarEvents == null)
            return BadRequest("Calendar request failed!");
        return Ok(calendarEvents);
    }

    [HttpPut("/Event")]
    public async Task<ActionResult> EditCalendarEvent([FromBody] CalendarEventDto calendarEvent)
    {
        return Ok(await _calendarService.UpdateCalendarEvent(calendarEvent));
    }

    [HttpPost("/Event")]
    public async Task<ActionResult> CreateCalendarEvent([FromBody] CalendarEventDto calendarEvent)
    {
        return Ok(await _calendarService.CreateCalendarEvent(calendarEvent));
    }

    [HttpDelete("/Event")]
    public async Task<ActionResult> DeleteCalendarEvent(string id)
    {
        await _calendarService.DeleteCalendar(id);
        return NoContent();
    }
}