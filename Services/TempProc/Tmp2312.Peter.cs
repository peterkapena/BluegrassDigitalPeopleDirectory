namespace BluegrassDigitalPeopleDirectory.Services.TempProc
{
    public partial class TmpProc
    {
        const string ClassName = "Tmp2312";

        [TmpProcAtt(className: ClassName)]
        private void AddDefaultAdminUser()
        {
            UserService.CreateAsync(new XUser
            {
                Email = "peterkapenapeter@gmail.com",
                Password = "PPPPPPP@PPPPP"
            });
        }
    }
}
