using ProjectModule.Abstraction.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectModule.Abstraction.Repository
{
    public interface IProjectRepository
    {
        Task AddProject(Project prject, CancellationToken cancellation);
        Task UpdateProject(string id, Project prject, CancellationToken cancellation);
        Task<Project> getProjcectByID(string id, CancellationToken cancellation);
        Task<Project> GetByTitle(string Title, CancellationToken cancellation);
        Task<ProjectCollection> GetAllWithPagingAsync(string keywork, int? page, int? pageSize);
    }
}
