namespace Efteling.Website.Actions
{
    using Sitecore.Rules;

    public class LowercaseDisplayname<T> : RenamingDisplayNameAction<T>
        where T : RuleContext
    {
        /// <summary>
        ///     Action implementation.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        public override void Apply(T ruleContext)
        {
            var newName = ruleContext.Item.Appearance.DisplayName.ToLower();

            if (ruleContext.Item.Appearance.DisplayName != newName)
            {
                RenameItemDisplayName(ruleContext.Item, newName);
            }
        }
    }
}