using Project_PHE.DTOs.Employee;

namespace Project_PHE.DTOs.Project
{
    public class ProjectDto
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }

        public static implicit operator Entities.Project(ProjectDto projectDto)
        {
            return new Entities.Project
            {
                Guid = projectDto.Guid,
                Name = projectDto.Name,
                Start_Date = projectDto.Start_Date,
                End_Date = projectDto.End_Date,
            };
        }

        public static explicit operator ProjectDto(Entities.Project project)
        {
            return new ProjectDto
            {
                Guid = project.Guid,
                Name = project.Name,
                Start_Date = project.Start_Date,
                End_Date = project.End_Date,
            };
        }
    }
}
