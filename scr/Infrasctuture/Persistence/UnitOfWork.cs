using Authentication.Infrastructure.Repositories;
using CodeFlow.Data.Context.Package.Base.Uow;

namespace Authentication.Infrastructure.Persistence;

/// <summary>
/// Implements the Unit of Work pattern to manage transactions and database changes.
/// </summary>
public class UnitOfWork(SqlContext context, IAuthRepository authRepository) : BaseUnitOfWork(context), IUnitOfWork
{
	public IAuthRepository AuthRepository { get; } = authRepository;
}
