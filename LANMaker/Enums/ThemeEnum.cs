using System.ComponentModel.DataAnnotations;

namespace LANMaker.Enums
{
	public enum ThemeEnum
    {
        [Display(Name = "Default", ShortName = "default")]
        Default,
        [Display(Name = "Neon Pink", ShortName = "neon-pink")]
        NeonPink,
    }
}
