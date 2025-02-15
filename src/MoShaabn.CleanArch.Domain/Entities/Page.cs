using MoShaabn.CleanArch.Entities.Base;

namespace MoShaabn.CleanArch.Entities
{
    public class Page : BaseEntity
    {
        public Page(string slug, string titleAr, string titleEn, string descriptionAr, string descriptionEn)
        {
            Slug = slug;
            TitleAr = titleAr;
            TitleEn = titleEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
        }
        public string Slug { get; set; } // e.g., privacy_policy, terms_and_conditions, about_us
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public string GetTitle() {
            return TitleAr;
        }
        public string GetDescription() {
            return DescriptionAr;
        }
    }
}