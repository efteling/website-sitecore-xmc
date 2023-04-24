namespace Efteling.Website.Actions
{
    using System;
    using System.Collections.Generic;

    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Rules;
    using Sitecore.Rules.Actions;

    public abstract class RenamingDisplayNameAction<T> : RuleAction<T>
      where T : RuleContext
    {
        private static readonly List<ID> renamingActions = new List<ID>();

        /// <summary>
        /// Rename the item, unless it is a standard values item 
        /// or the start item for any of the managed Web sites.
        /// </summary>
        /// <param name="item">The item to rename.</param>
        /// <param name="newName">The new name for the item.</param>
        protected void RenameItemDisplayName(Item item, string newName)
        {
            if (item.Database.Name != "master")
            {
                return;
            }

            if (item.Template.StandardValues != null && item.ID == item.Template.StandardValues.ID)
            {
                return;
            }

            if (renamingActions.Contains(item.ID))
            {
                return;
            }

            renamingActions.Add(item.ID);

            try
            {
                foreach (var site in Sitecore.Configuration.Factory.GetSiteInfoList())
                {
                    if (String.Compare(site.RootPath + site.StartItem, item.Paths.FullPath, true) == 0)
                    {
                        return;
                    }
                }

                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    using (new EditContext(item))
                    {
                        using (new Sitecore.Data.Events.EventDisabler())
                        {
                            item.Appearance.DisplayName = newName;
                        }
                    }
                }
            }
            finally
            {
                if (renamingActions.Contains(item.ID))
                {
                    renamingActions.Remove(item.ID);
                }
            }
        }
    }
}
