using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BagOfTricks.Core
{
    internal static class Achievement
    {
        internal static Dictionary<string, Tuple<string, string>> invalidAchievementLookup = new()
        {
            { "Choose Reef-Talon for position", new("The Bastard Tribe", "Install Reef-Talon in a position of authority over the Bastard's Wound") },
            { "Choose Wagstaff for position", new("Prince of Tidecasters", "Place Wagstaff in a position of authority over the Bastard's wound") },
            { "Choose Jaspos for position", new("Set in Stone", "Appoint Jaspos to a position of authority over the Bastard's Wound") },
            { "Kill Oldwalls trespassers", new("Bastard Wounder", "Slay all who would willfully trespass the Oldwalls in the Bastard's Wound") },
            { "Complete Barik's Companion Quest", new("Change of Carapace", "Complete Barik's Companion Quest") },
            { "Complete Lantry's companion quest", new("Truth and Reconciliation", "Complete Lantry's Companion Quest") },
            { "Complete Verse's Companion Quest", new("Unfinished Business", "Complete Verse's Companion Quest") },
            { "Trick Reef-Talon", new("They Sort of Deserve It...", "Trick Reef-Talon into inadvertently wiping out her loved ones in Bastard's Wound") },
            { "Kill all Sleepless in BWound", new("Requiem for the Somnombulists", "Slay all of the Sleepless in Bastard's Wound") },
            { "Defeat 3 encounters with sleepless ", new("Sleepless in Three Battles", "Defeat three groups of Sleepless below Bastard's Wound") },
            { "Resolve BWound without killing resident havoc", new("Let Sleeping Abominations Lie", "Resolve the troubles of the Bastard's Wound without slaying the resident havoc") },
            { "Distract guards by murdering bystander", new("Well Done", "Wantonly murder a bystander to distract some easily-defeated guards") },
            { "Complete all BWound Quests and Sidequests", new("Troubleshooter", "Complete all available quests and sidequests in the Bastard's Wound") },
            { "End with Barik trapped", new("Sloppy Seconds", "Complete the game with Barik freed out of his armor but trapped in a cage of abuse") },
            { "Feed Verse to Nerat", new("Let the Next Verse Begin", "Complete Verse's Side-quest and have her usurp the Voices of Nerat") },
            { "Kill a foe with slab of meat", new("Nice to Meat You", "Slay a foe with a slab of meat") },
            { "End the game peacefully", new("Apocalypse Later", "End the conquest of the Tiers with a peaceful surrender") },
            { "Complete BWound quests but walk away", new("Complicit Justice", "Learn of the infighting in Bastard's Wound, and do nothing to resolve it") },
            { "Huge Miss Steak", new("I've Made a Huge Miss Steak", "Perform a Critical Miss with a slab of meat") }
        };

        public static void Parse(ref string name, ref string descr)
        {
            string pattern = @"\[.*?\]";
            name = Regex.Replace(name, pattern, "");
            descr = Regex.Replace(descr, pattern, "");
            descr = descr.Remove(0, 1); // Remove the space at the start after removing [base] or [bwound]
        }

        public static string HighlightSearch(string input, string substring)
        {
            string startTag = $"<color=#{Styles.Colors.GreenHex}>";
            string endTag = "</color>";

            substring = Regex.Escape(substring);

            var regexText = new Regex(substring, RegexOptions.IgnoreCase);
            return regexText.Replace(input, $"{startTag}{substring}{endTag}");
        }
    }
}
