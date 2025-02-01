using Authentication.Infrastructure.Repositories;
using CodeFlow.Data.Context.Package.Base.Uow;

namespace Authentication.Infrastructure.Persistence;

public interface IUnitOfWork : IBaseUnitOfWork
{
	IAuthRepository AuthRepository { get; }
}