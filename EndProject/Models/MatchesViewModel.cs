using FootballApiProject.Models.DTO_s.MatchsDto;

namespace EndProject.Models
{
    public class MatchesViewModel
    {
        public List<MatchDto> OldMatches { get; set; }
        public List<MatchDto> TodayMatches { get; set; }
        public List<MatchDto> TomorrowMatches { get; set; }
        public List<MatchDto> UpcomingMatches { get; set; }
    }
}
