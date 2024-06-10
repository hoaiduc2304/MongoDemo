using ProjectModule.Abstraction.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModule.Abstraction.Repository
{
    public interface IProjectRepository
    {
        Task AddProject(Project prject);
        Task UpdateProject(string id, Project prject);
        Task<Project> getProjcectByID(string id);
        Task<Project> GetAgentByTitle(string Title);
        Task<ProjectCollection> GetAllWithPagingAsync(string keywork, int? page, int? pageSize);
    }
}
