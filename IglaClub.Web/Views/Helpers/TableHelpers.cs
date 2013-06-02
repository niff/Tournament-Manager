using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace IglaClub.Web.Views.Helpers
{
    /// <summary>
    /// Creates an Html table based on the collection 
    /// which has a maximum numer of rows (expands horizontally)
    /// </summary>

    public static class TableHelpers
    {
        public static HelperResult TableHorizontal<T>(this HtmlHelper html,
                                                      IEnumerable<T> collection,
                                                      int maxRows,
                                                      Func<T, HelperResult> template) where T : class
        {
            return new HelperResult(writer =>
                {
                    var items = collection.ToArray();
                    var itemCount = items.Count();
                    var maxCols = Convert.ToInt32(Math.Ceiling(items.Count()/Convert.ToDecimal(maxRows)));
                    //construct a grid first
                    var grid = new T[maxCols,maxRows];
                    var current = 0;
                    for (var x = 0; x < maxCols; x++)
                        for (var y = 0; y < maxRows; y++)
                            if (current < itemCount)
                                grid[x, y] = items[current++];
                    WriteTable(grid, writer, maxRows, maxCols, template);
                });
        }

        /// <summary>
        /// Creates an Html table based on the collection 
        /// which has a maximum number of cols (expands vertically)
        /// </summary>
        public static HelperResult TableVertical<T>(this HtmlHelper html,
                                                    IEnumerable<T> collection,
                                                    int maxCols,
                                                    Func<T, HelperResult> template) where T : class
        {
            return new HelperResult(writer =>
                {
                    var items = collection.ToArray();
                    var itemCount = items.Count();
                    var maxRows = Convert.ToInt32(Math.Ceiling(items.Count()/Convert.ToDecimal(maxCols)));
                    //construct a grid first
                    var grid = new T[maxCols,maxRows];
                    var current = 0;
                    for (var y = 0; y < maxRows; y++)
                        for (var x = 0; x < maxCols; x++)
                            if (current < itemCount)
                                grid[x, y] = items[current++];
                    WriteTable(grid, writer, maxRows, maxCols, template);
                });
        }

        /// <summary>
        /// Writes the table markup to the writer based on the item
        /// input and the pre-determined grid of items
        /// </summary>
        private static void WriteTable<T>(T[,] grid,
                                          TextWriter writer,
                                          int maxRows,
                                          int maxCols,
                                          Func<T, HelperResult> template) where T : class
        {
            //create a table based on the grid
            writer.Write("<table>");
            for (var y = 0; y < maxRows; y++)
            {
                writer.Write("<tr>");
                for (var x = 0; x < maxCols; x++)
                {
                    writer.Write("<td>");
                    var item = grid[x, y];
                    if (item != null)
                    {
                        //if there's an item at that grid location, call its template
                        template(item).WriteTo(writer);
                    }
                    writer.Write("</td>");
                }
                writer.Write("</tr>");
            }
            writer.Write("</table>");
        }

        
    }
}