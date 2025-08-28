using HotelApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Repositories
{
   
    public class StudyLogRepository : RepositoryBase<StudyLog, StudyLogEf>, IStudyLogRepository
    {

        public StudyLogRepository(AppDbContext context, IEntityEfMapper<StudyLog, StudyLogEf> mapper)
          : base(context, mapper) { }


    }
}
    