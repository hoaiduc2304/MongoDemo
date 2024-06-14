using MongoDB.Driver;
using ProjectModule.Abstraction.Repository;
using ProjectModule.Abstraction.ViewModels;
using ProjectModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModule.Services
{
    public interface IProjectService
    {
        Task AddProject(Project prject, CancellationToken cancellation);
        Task UpdateProject(string id, Project prject, CancellationToken cancellation);
        Task<Project> getProjcectByID(string id, CancellationToken cancellation);
        Task<Project> GetByTitle(string Title, CancellationToken cancellation);
        Task<ProjectCollection> GetPaging(string keyword, int? page = 1, int? pageSize = 10);
    }
    public class ProjectService : IProjectService
    {
        IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task AddProject(Project prject, CancellationToken cancellation)
        {
            await _projectRepository.AddProject(prject, cancellation);
        }

        public async Task<Project> GetByTitle(string Title, CancellationToken cancellation)
        {
            return await _projectRepository.GetByTitle(Title, cancellation);
        }

        public async Task<ProjectCollection> GetPaging(string keyword, int? page = 1, int? pageSize = 10)
        {
            return await _projectRepository.GetAllWithPagingAsync(keyword, page, pageSize);
        }

        public async Task<Project> getProjcectByID(string id, CancellationToken cancellation)
        {
            return await _projectRepository.getProjcectByID(id, cancellation);
        }

        public async Task UpdateProject(string id, Project prject, CancellationToken cancellation)
        {
            await _projectRepository.UpdateProject(id, prject, cancellation);
        }
    }
}
