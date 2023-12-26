using Microsoft.AspNetCore.Identity;

namespace BluegrassDigitalPeopleDirectory.Services.TempProc
{
    public partial class TmpProc
    {
        const string ClassName = "Tmp2312";

        [TmpProcAtt(className: ClassName)]
        private async Task AddDefaultAdminUser()
        {
            await UserService.CreateAsync(new Controllers.Auth.RegisterModelIn
            {
                Email = "peterkapenapeter@gmail.com",
                Password = "PPPPPPP@PPPPP",
                Role = "Admin"
            });
        }

        [TmpProcAtt(className: ClassName)]
        private async Task SeedRoles()
        {
            await UserService.SeedRoles();
        }
        [TmpProcAtt(className: ClassName)]
        private async Task GenerateAndSavePersonsAsync()
        {
            await PersonService.GenerateAndSavePersonsAsync();
        }
    }
}
