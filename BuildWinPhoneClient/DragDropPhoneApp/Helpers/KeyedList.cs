#region Using Directives



#endregion
namespace DragDropPhoneApp.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public class KeyedList<TKey, TItem> : List<TItem>
    {
        #region Constructors and Destructors

        public KeyedList(TKey key, IEnumerable<TItem> items)
            : base(items)
        {
            this.Key = key;
        }

        public KeyedList(IGrouping<TKey, TItem> grouping)
            : base(grouping)
        {
            this.Key = grouping.Key;
        }

        #endregion

        #region Public Properties

        public TKey Key { get; protected set; }

        #endregion
    }
}