using System.Globalization;

namespace MoShaabn.CleanArch.Entities.Base;

public abstract class LocalizableEntity : BaseEntity
{
    public string NameAr {get; set;}
    public string NameEn {get; set;}
    public string GetName() {
        string cultureName = CultureInfo.CurrentUICulture.Name;
        if (cultureName == "en")
        {
            return NameEn;
        }
        else
        {
            return NameAr;
        }
    }
}