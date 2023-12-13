namespace OutlookSyncApi.DTOs
{
    public record AttendeeDto
    {
        public string Name { get; init; } = string.Empty;
        public string EmailAdress { get; init; } = string.Empty;
    }
}
