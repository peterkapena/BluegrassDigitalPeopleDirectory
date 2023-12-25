using Microsoft.AspNetCore.Identity;

namespace BluegrassDigitalPeopleDirectory.Services.TempProc
{
    public partial class TmpProc
    {
        const string ClassName = "Tmp2312";

        [TmpProcAtt(className: ClassName)]
        private void AddDefaultAdminUser()
        {
            UserService.CreateAsync(new Controllers.Auth.RegisterModelIn
            {
                Email = "peterkapenapeter@gmail.com",
                Password = "PPPPPPP@PPPPP"
            });
        }

        [TmpProcAtt(className: ClassName)]
        private void SeedRoles()
        {
            UserService.SeedRoles();
        }
    }
}
