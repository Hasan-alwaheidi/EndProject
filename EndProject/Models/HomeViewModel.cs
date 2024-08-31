using EndProject.Models;
using FootballApiProject.Models.DTO_s.MatchsDto;
using Newtonsoft.Json.Linq;
using SharedDtos.DTO_s.NewsDto;
using System.Collections.Generic;

public class HomeViewModel
{
    public List<MatchDto> Matches { get; set; }
    public List<string> SlideshowImages { get; set; } 

    public List<NewsDtoo> NewsItems { get; set; }
    public JArray LiveScores { get; set; }


}
