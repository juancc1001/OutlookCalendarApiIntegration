namespace OutlookSyncApi.DTOs
{
    public record CalendarEventDto
    {
        public string Id { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? Subject { get; init; }
        public string? Timezone { get; init; }
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }
        public IEnumerable<AttendeeDto>? Attendees { get; init; }
        public AttendeeDto? Organizer { get; init; }
        public string? OnlineMeetingUrl { get; init; }
    }
}
