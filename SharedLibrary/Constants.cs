using System.Security.Claims;

namespace SharedLibrary
{
    public sealed class SqlErrorCodes
    {
        public const int UniqueVoilation = 2601;

        public const int ForeignKeyVoilation = 547;

        public const int PrimaryKeyVoilation = 262778;
    }


    public sealed class AppClaimTypes
    {
        public const string UserId = nameof(UserId);


        public const string Role = ClaimTypes.Role;

        public const string Name = nameof(Name);

        public const string UserName = nameof(UserName);

        public const string Email = nameof(Email);

        public const string ImagePath = nameof(ImagePath);

        public const string DepartmentId = nameof(DepartmentId);

        public const string PhoneNo = nameof(PhoneNo);

        public const string SemesterId = nameof(SemesterId);

        public const string Permission = nameof(Permission);

        public const string RequiredPermission = nameof(RequiredPermission);
    }
}


//public const string UserId = nameof(UserId);
//public const string UserName = nameof(UserName);
//public const string UserEmail = nameof(UserEmail);
//public const string PhoneNo = nameof(PhoneNo);
//public const string UserRole = nameof(UserRole);