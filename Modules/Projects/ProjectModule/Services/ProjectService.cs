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
        Task AddProject(Project prject);
        Task UpdateProject(string id, Project prject);
        Task<Project> getProjcectByID(string id);
        Task<Project> GetAgentByTitle(string Title);
        Task<ProjectCollection> GetPaging(string keyword,int? page = 1, int? pageSize = 10);
    }
    public class ProjectService : IProjectService
    {
        IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task AddProject(Project prject)
        {
             await _projectRepository.AddProject(prject);
        }

        public async Task<Project> GetAgentByTitle(string Title)
        {
            return await _projectRepository.GetAgentByTitle(Title);
        }

        public async Task<ProjectCollection> GetPaging(string keyword, int? page = 1, int? pageSize = 10)
        {
            return await _projectRepository.GetAllWithPagingAsync(keyword, page, pageSize);
        }

        public async Task<Project> getProjcectByID(string id)
        {
            return await _projectRepository.getProjcectByID(id);
        }

        public async Task UpdateProject(string id, Project prject)
        {
             await _projectRepository.UpdateProject(id, prject);
        }
    }
}
