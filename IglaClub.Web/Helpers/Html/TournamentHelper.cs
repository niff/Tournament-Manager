using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Enums;
using IglaClub.Web.Extensions;

namespace IglaClub.Web.Helpers.Html
{
    public static class TournamentHelper
    {
        public static IHtmlString GetSubscribtionStatus(bool subscribed, long id)
        {
            if (subscribed)
            {
                return new HtmlString(@"<span class='glyphicon glyphicon-ok success' 
                        title='You are subscribed to this tournament'></span>
                        <span class='hidden-xs'>Joined</span>");
            }
            return
                new HtmlString(string.Format("<a href='Details/{0}'><i class='glyphicon glyphicon-plus'></i><span class=\"hidden-xs\">Join</span></a>",
                                             id));
        }

        public static IHtmlString GetTournamentStatusIcon(TournamentStatus status)
        {
            bool isStarted = status == TournamentStatus.Started;
            bool isPlanned = status == TournamentStatus.Planned;
            var statusLabelClass = isPlanned ? "planned" : (isStarted ? "ongoing" : "past");
            var statusLabel = isPlanned ? "not started" : (isStarted ? "started" : "finished");
            string statusIcon = string.Format("<div class='tournament-status {0}'><span class='tournament-status-text'>{1}</span></div>", statusLabelClass,
                                           statusLabel);
           
            return new HtmlString(statusIcon);
        }

        public static IHtmlString ContractColorsDropDown(string ddId, ContractColors selected)
        {
            const string imgPath = "../../Content/img/cards/";
            var dropdown = new StringBuilder();

            dropdown.Append(string.Format("<select id=\"{0}\">", ddId));
            dropdown.Append(string.Format("<option value=\"{0}\" ></option>",(int)ContractColors.Unknown));
            var contractColors = Enum.GetValues(typeof (ContractColors)).Cast<ContractColors>().Where(c => c != ContractColors.Unknown);
            foreach (var color in contractColors)
            {
                dropdown.Append(
                    string.Format(
                        "<option value=\"{0}\" data-imagesrc=\"{1}{2}.png\" {3} data-description=\"\"></option>",
                        ((int) color).ToString(), imgPath, color.ToString().ToLowerInvariant(),
                        color == selected ? "selected=\"selected\"" : string.Empty));
            }

            dropdown.Append("</select>");

            return new HtmlString(dropdown.ToString());
        }        
        
        public static SelectList ContractDoubleSelectList(ContractDoubled selected)
        {
            var items = Enum.GetValues(typeof (ContractDoubled)).Cast<ContractDoubled>().Select(doubled => new SelectListItem
                {
                    Text = doubled.GetDisplayAttributeFrom(typeof (ContractDoubled)), 
                    Value = ((int) doubled).ToString(), 
                    Selected = (int) doubled == (int) selected
                }).ToList();

            return new SelectList(items, "Value", "Text", (int)selected); ;
        }
    }
}