using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Entities;
using Uply.Infrastructure.Database.Repositories.Abstract;

namespace Uply.Infrastructure.Database.Repositories;

public class TaskReportRepository(AppDbContext dbContext) : CrudRepository<TaskReport>(dbContext), ITaskReportRepository;

