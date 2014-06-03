#region Using Directives



#endregion

namespace DragDropPhoneApp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using Microsoft.Phone.Globalization;

    public class AlphaKeyGroup<T> : List<T>
    {
        #region Constants

        private const string GlobeGroupKey = "\uD83C\uDF10";

        #endregion

        #region Constructors and Destructors

        public AlphaKeyGroup(string key)
        {
            this.Key = key;
        }

        #endregion

        #region Public Properties

        public string Key { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> keySelector, bool sort)
        {
            return CreateGroups(items, Thread.CurrentThread.CurrentCulture, keySelector, sort);
        }

        public static List<AlphaKeyGroup<T>> CreateGroups(
            IEnumerable<T> items,
            CultureInfo cultureInfo,
            Func<T, string> keySelector,
            bool sort)
        {
            SortedLocaleGrouping slg = new SortedLocaleGrouping(cultureInfo);
            List<AlphaKeyGroup<T>> list = CreateDefaultGroups(slg);
            if (items == null)
            {
                return null;
            }

            foreach (T item in items)
            {
                int index;
                {
                    index = slg.GetGroupIndex(keySelector(item));
                }

                if (index >= 0 && index < list.Count)
                {
                    list[index].Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    group.Sort(
                        (c0, c1) => { return cultureInfo.CompareInfo.Compare(keySelector(c0), keySelector(c1)); });
                }
            }

            return list;
        }

        #endregion

        #region Methods

        private static List<AlphaKeyGroup<T>> CreateDefaultGroups(SortedLocaleGrouping slg)
        {
            List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

            foreach (string key in slg.GroupDisplayNames)
            {
                if (key == "...")
                {
                    list.Add(new AlphaKeyGroup<T>(GlobeGroupKey));
                }
                else
                {
                    list.Add(new AlphaKeyGroup<T>(key));
                }
            }

            return list;
        }

        #endregion
    }
}