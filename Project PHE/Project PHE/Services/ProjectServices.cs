using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.DTOs.Employee;
using Project_PHE.DTOs.Project;
using Project_PHE.Entities;

namespace Project_PHE.Services
{
    public class ProjectServices
    {
        private readonly IProjectRepository _projectRepository;
        private readonly PheDbContext _dbContext;

        public ProjectServices(IProjectRepository projectRepository, PheDbContext dbContext)
        {
            _projectRepository = projectRepository;
            _dbContext = dbContext;
        }

        public IEnumerable<ProjectDto> GetallProject()
        {
            var projects = _projectRepository.GetAll();

            if (!projects.Any()) return Enumerable.Empty<ProjectDto>();

            var getProjects = new List<ProjectDto>();

            foreach (var project in projects)
                getProjects.Add(new ProjectDto
                {
                    Guid = project.Guid,
                    Name = project.Name,
                    Start_Date = project.Start_Date,
                    End_Date = project.End_Date
                });
            return getProjects;
        }

        public ProjectDto CreateProject(ProjectDto ProjectDto)
        {
            // Validasi input atau lakukan validasi bisnis lainnya sesuai kebutuhan

            // Membuat instance dari entitas Project
            var newProject = new Project
            {
                Guid = Guid.NewGuid().ToString(), // Misalnya, menggunakan GUID untuk ID proyek
                Name = ProjectDto.Name,
                Start_Date = ProjectDto.Start_Date,
                End_Date = ProjectDto.End_Date
                // Tambahkan properti lain sesuai kebutuhan
            };

            // Menyimpan proyek baru ke database
            _dbContext.Projects.Add(newProject);
            _dbContext.SaveChanges();

            // Mengembalikan DTO proyek yang baru dibuat
            return new ProjectDto
            {
                Guid = newProject.Guid,
                Name = newProject.Name,
                Start_Date = newProject.Start_Date,
                End_Date = newProject.End_Date
            };
        }

        public ProjectDto UpdateProject(string projectGuid, ProjectDto updateProjectDto)
        {
            // Mendapatkan proyek berdasarkan GUID
            var existingProject = _dbContext.Projects.SingleOrDefault(p => p.Guid == projectGuid);

            if (existingProject == null)
            {
                // Proyek tidak ditemukan, Anda dapat memilih cara menangani kondisi ini
                return null;
            }

            // Validasi input atau lakukan validasi bisnis lainnya sesuai kebutuhan

            // Memperbarui properti proyek yang diperlukan
            existingProject.Name = updateProjectDto.Name;
            existingProject.Start_Date = updateProjectDto.Start_Date;
            existingProject.End_Date = updateProjectDto.End_Date;
            // Memperbarui properti lain sesuai kebutuhan

            // Menyimpan perubahan ke database
            _dbContext.SaveChanges();

            // Mengembalikan DTO proyek yang diperbarui
            return new ProjectDto
            {
                Guid = existingProject.Guid,
                Name = existingProject.Name,
                Start_Date = existingProject.Start_Date,
                End_Date = existingProject.End_Date
            };
        }

        public bool DeleteProject(string projectGuid)
        {
            // Mendapatkan proyek berdasarkan GUID
            var projectToDelete = _dbContext.Projects.SingleOrDefault(p => p.Guid == projectGuid);

            if (projectToDelete == null)
            {
                // Proyek tidak ditemukan, Anda dapat memilih cara menangani kondisi ini
                return false;
            }

            // Menghapus proyek dari database
            _dbContext.Projects.Remove(projectToDelete);
            _dbContext.SaveChanges();

            // Mengembalikan true untuk menunjukkan penghapusan berhasil
            return true;
        }

        public ProjectDto GetProjectByGuid(string Guid)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Guid == Guid);

            if (project == null)
            {
                return null;
            }

            return new ProjectDto
            {
                Guid = project.Guid,
                Name = project.Name,
                Start_Date = project.Start_Date,
                End_Date = project.End_Date
            };
        }
    }
}
