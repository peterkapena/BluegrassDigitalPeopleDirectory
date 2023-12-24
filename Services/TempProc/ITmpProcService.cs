using BluegrassDigitalPeopleDirectory.Models;
using BluegrassDigitalPeopleDirectory.Models.TmpProc;
using BluegrassDigitalPeopleDirectory.Services.Bug;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BluegrassDigitalPeopleDirectory.Services.TempProc
{
    public interface ITmpProcService
    {
        public Task<bool> HasAlreadyRun(MethodInfo method);
        public Task SaveTmpTask(MethodInfo method);
        public ITmpProcService Run();

    }

    public partial class TmpProc(DBContext context, IErrorLogService errorLogService, IUserService userService) : ITmpProcService
    {

        #region "constructor and properties"

        public DBContext Context { get; } = context;
        public IErrorLogService ErrorLogService { get; } = errorLogService;
        public IUserService UserService { get; } = userService;
        #endregion

        public ITmpProcService Run()
        {
            foreach (var method in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (method.GetCustomAttributes(typeof(TmpProcAtt), false).Length > 0 && !HasAlreadyRun(method).Result)
                {
                    Action RunStep = (Action)Delegate.CreateDelegate(typeof(Action), this, method);
                    try
                    {
                        RunStep();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogService.RegisterError(new Exception($"{method.Name} Failed to run", ex));
                    }
                    _ = SaveTmpTask(method);
                }
            }
            return this;
        }

        public async Task<bool> HasAlreadyRun(MethodInfo method)
        {
            try
            {
                CreateTmpTaskTable();
                return await Context.TmpTasks
                    .Where(t => t.MethodName == method.Name && t.ClassName == method.GetCustomAttribute<TmpProcAtt>().ClassName)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                _ = ErrorLogService.RegisterError(new Exception("Temp proc - HasAlreadyRun", ex));
                return false;
            }
        }

        public void CreateTmpTaskTable()
        {
            try
            {
                var SQL = @$"
                        CREATE TABLE IF NOT EXISTS ""TmpTask"" (
                        ""TmpTaskId"" INTEGER NOT NULL CONSTRAINT ""PK_TmpTask"" PRIMARY KEY AUTOINCREMENT,
                        ""MethodName"" TEXT NULL,
                        ""ClassName"" TEXT NULL,
                        ""WhenRun"" TEXT NOT NULL
                        );
                        CREATE UNIQUE INDEX IF NOT EXISTS ""IX_TmpTask_WhenRun"" ON ""TmpTask"" (""WhenRun"");";

                Context.Database.ExecuteSqlRaw(SQL);
            }
            catch (Exception e)
            {
                ErrorLogService.RegisterError(e);
            }
        }
        public async Task SaveTmpTask(MethodInfo method)
        {
            var tmpProcAtt = method.GetCustomAttribute<TmpProcAtt>();
            var tmpProc = new TmpTask { ClassName = tmpProcAtt.ClassName, MethodName = method.Name, WhenRun = DateTime.Now };
            await Context.TmpTasks.AddAsync(tmpProc);
            await Context.SaveChangesAsync();
        }
    }
}
