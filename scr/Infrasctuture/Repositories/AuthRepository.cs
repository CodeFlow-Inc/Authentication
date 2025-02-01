using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;
using CodeFlow.Data.Context.Package.Base.Repositories;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure.Repositories;

public class AuthRepository(SqlContext context, ILogger<AuthRepository> logger) : BaseRepository<ApplicationUser, int>(context, logger), IAuthRepository;