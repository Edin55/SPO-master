using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPO.Utilities
{
    public static class Utilities
    {
        public static string IsActive(this HtmlHelper html,
                                      string control,
                                      string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }

        public static string IsActive(this HtmlHelper html,
                                      string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl;

            return returnActive ? "active" : "";
        }

        public static IEnumerable<TSource> DistinctDisunionBy<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> disunionBy, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in disunionBy)
            {
                seenKeys.Add(keySelector(element));
            }
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}