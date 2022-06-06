using DotNetCoreScaffoldingSqlServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Repositories
{
    public interface IUserRepository : IRepositoryBase<User> { }
    public class UserRepository : RepositoryBase<User>, IUserRepository { public UserRepository() : base() { } }

    public interface IMenuPermissionRepository : IRepositoryBase<MenuPermission> { }
    public class MenuPermissionRepository : RepositoryBase<MenuPermission>, IMenuPermissionRepository { public MenuPermissionRepository() : base() { } }
    public interface IDesignationRepository : IRepositoryBase<Designation> { }
    public class DesignationRepository : RepositoryBase<Designation>, IDesignationRepository { public DesignationRepository() : base() { } }
    public interface IRoleRepository : IRepositoryBase<Role> { }
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository { public RoleRepository() : base() { } }
    public interface IRoleAssignRepository : IRepositoryBase<RoleAssign> { }
    public class RoleAssignRepository : RepositoryBase<RoleAssign>, IRoleAssignRepository { public RoleAssignRepository() : base() { } }
    public interface IMenuRepository : IRepositoryBase<Menu> { }
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository { public MenuRepository() : base() { } }
    public interface ICommonCodeRepository : IRepositoryBase<CommonCode> { }
    public class CommonCodeRepository : RepositoryBase<CommonCode>, ICommonCodeRepository { public CommonCodeRepository() : base() { } }
    public interface IRoleWiseMenuAssignRepository : IRepositoryBase<RoleWiseMenuAssign> { }
    public class RoleWiseMenuAssignRepository : RepositoryBase<RoleWiseMenuAssign>, IRoleWiseMenuAssignRepository { public RoleWiseMenuAssignRepository() : base() { } }
    public interface IGroupRepository : IRepositoryBase<Group> { }
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository { public GroupRepository() : base() { } }
    public interface IUnitRepository : IRepositoryBase<Unit> { }
    public class UnitRepository : RepositoryBase<Unit>, IUnitRepository { public UnitRepository() : base() { } }
    public interface IDepartmentRepository : IRepositoryBase<Department> { }
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository { public DepartmentRepository() : base() { } }
    public interface IUserSessionRepository : IRepositoryBase<UserSession> { }
    public class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository { public UserSessionRepository() : base() { } }
    public interface IHeaderRepository : IRepositoryBase<Header> { }
    public class HeaderRepository : RepositoryBase<Header>, IHeaderRepository { public HeaderRepository() : base() { } }
    public interface ITransactionRepository : IRepositoryBase<Transaction> { }
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository { public TransactionRepository() : base() { } }
    public interface IUserWiseUnitPermissionRepository : IRepositoryBase<UserWiseUnitPermission> { }
    public class UserWiseUnitPermissionRepository : RepositoryBase<UserWiseUnitPermission>, IUserWiseUnitPermissionRepository { public UserWiseUnitPermissionRepository() : base() { } }

    public interface IDocumentCategoryRepository : IRepositoryBase<DocumentCategory> { }
    public class DocumentCategoryRepository : RepositoryBase<DocumentCategory>, IDocumentCategoryRepository { public DocumentCategoryRepository() : base() { } }

}
