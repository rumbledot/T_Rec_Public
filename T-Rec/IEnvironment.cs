using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace T_Rec
{
    public interface IEnvironment
    {
        Theme GetOperatingSystemTheme();
        Task<Theme> GetOperatingSystemThemeAsync();
    }

    public enum Theme { Light, Dark }
}
