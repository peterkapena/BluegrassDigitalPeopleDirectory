using System;

namespace BluegrassDigitalPeopleDirectory.Services.TempProc
{
    public class TmpProcAtt : Attribute
    {
        public TmpProcAtt(string className)
        {
            ClassName = className;
        }

        public string ClassName { get; }
    }
}
