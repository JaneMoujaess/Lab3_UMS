namespace Lab3.Domain.Models;

public class GenderStatistics
{
    public string Gender { set; get; }
    public List<CourseStatistics> CourseStatisticsList { set; get; }
}