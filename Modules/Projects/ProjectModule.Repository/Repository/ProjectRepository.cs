using DNH.Storage.MongoDB.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ProjectModule.Abstraction.Repository;
using ProjectModule.Abstraction.ViewModels;
using ProjectModule.Repository.Collections;
using ProjectModule.Repository.Collections.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectModule.Repository.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        IMongoRepository<ProjectDocument> _db;
        IServiceProvider _service;

        public ProjectRepository(IServiceProvider service, IMongoRepository<ProjectDocument> db)
        {
            _service = service;
            _db = db;

        }
        public async Task<Project> getProjcectByID(string id, CancellationToken cancellation)
        {
            var document = await getByID(id, cancellation);
            return ToProject(document);
        }
        public async Task<Project> GetByTitle(string Title, CancellationToken cancellation)
        {
            var document = await _db.GetOneAsync(x => x.Title == Title, cancellation);
            return ToProject(document);
        }
        public async Task AddProject(Project prject, CancellationToken cancellation)
        {
            var prjDocument = ToProjectDocument(prject);
            await _db.AddAsync(prjDocument, cancellation);
            prject.id = prjDocument.Id;
        }
        public async Task UpdateProject(string id, Project prject, CancellationToken cancellation)
        {
            var document = ToProjectDocument(prject);
            document.UpdatedAt = DateTime.UtcNow;
            await _db.UpdateOneAsync(id, document, cancellation);
        }


        public async Task<ProjectCollection> GetAllWithPagingAsync(string keywork, int? page, int? pageSize)
        {
            Expression<Func<ProjectDocument, bool>> query = (x) => (x.Title.Contains(keywork));
            var collection = await _db.GetAllWithPagingAsync(query, page, pageSize);
            ProjectCollection projects = new ProjectCollection();

            projects.AddRange(collection.Select(ToProject));

            return projects;
        }

        private async Task<ProjectDocument> getByID(string id, CancellationToken cancellation)
        {
            return await _db.GetByIdAsync(id, cancellation);
        }
        private Project ToProject(ProjectDocument projectDocument)
        {
            Project project = new Project
            {
                id = projectDocument.Id, // Assuming Id is a property in MongoBaseEntity or BaseEntity
                Title = projectDocument.Title,
                Description = projectDocument.Description,
                Author = projectDocument.Author,
                proposal = new ProjectProposal
                {
                    templateCode = projectDocument.Proprosal.templateCode,
                    templateVersion = projectDocument.Proprosal.templateVersion
                }
            };

            return project;
        }
        private ProjectDocument ToProjectDocument(Project project)
        {
            ProjectDocument projectDocument = new ProjectDocument
            {
                Id = project.id, // Assuming Id is a property in MongoBaseEntity or BaseEntity
                Title = project.Title,
                Description = project.Description,
                Author = project.Author,
                Proprosal = new ProjectProposalElement
                {
                    templateCode = project.proposal.templateCode,
                    templateVersion = project.proposal.templateVersion
                }
            };

            return projectDocument;
        }
    }
}
