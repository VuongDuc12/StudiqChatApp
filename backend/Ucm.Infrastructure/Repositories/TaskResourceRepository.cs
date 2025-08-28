using HotelApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.Enums;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Repositories
{
    public class TaskResourceRepository : RepositoryBase<TaskResource, TaskResourceEf>, ITaskResourceRepository
    {
      
        public TaskResourceRepository(AppDbContext context, IEntityEfMapper<TaskResource, TaskResourceEf> mapper)
          : base(context, mapper) { }

     

      
    }
}