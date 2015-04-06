
using IglaClub.ObjectModel.Enums;

namespace IglaClub.Web.Views.Helpers
{
    public class ResultHelper
    {
        public static string GetStringForDoubledContract(ContractDoubled contractDoubled)
        {
            switch (contractDoubled)
            {
                    case ContractDoubled.Doubled:
                        return "x";
                    case ContractDoubled.Redoubled:
                        return "xx";
                    default:
                        return string.Empty;
            }
        }
        public static string GetStringForFinished(object score, PlayedBy playedBy)
        {
            if (playedBy == PlayedBy.DirectorScore)
                return "Ok";
            return score == null
                       ? ""
                       : "Ok";
            
        }
        public static string GetStringForColor(object score =null)
        {
            return "background: yellow";
            //return score == null
            //           ? ""
            //           : "Ok";
            ////: @"<img src=http://icons.iconarchive.com/icons/custom-icon-design/pretty-office-8/16/Accept-icon.png>";
        }
        
        
    }
}