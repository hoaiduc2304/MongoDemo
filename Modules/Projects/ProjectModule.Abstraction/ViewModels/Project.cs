using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectModule.Abstraction.ViewModels
{
    public class Project : BaseEntity
    {
        public string id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

       
        public string Author { get; set; }
        public ProjectProposal proposal { get; set; }
    }

    public class ProjectProposal
    {
        public string templateCode { get; set; }
        public string templateVersion { get; set; }
    }
    public class ProjectCollection : BaseEntityCollection<Project> { }
}
