using Authentication.Domain.Entities;
using CodeFlow.Data.Context.Package.Base.Repositories;

namespace Authentication.Infrastructure.Repositories;

public interface IAuthRepository : IBaseRepository<ApplicationUser, int>;