using System;

namespace TaskTrackerData
{
    public class LookupItem
    {
        #region properties
        /// <summary>
        /// Item Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Item Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Item English Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// SortOrder
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Item Active Flag
        /// </summary>
        public bool Active { get; set; }
        #endregion
    }
}
