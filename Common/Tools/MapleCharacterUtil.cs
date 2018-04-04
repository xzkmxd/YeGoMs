using Chloe;
using Common.Client.SQL;
using System.Data;
using System.Text.RegularExpressions;

namespace Common.Tools
{
    public class MapleCharacterUtil
    {
        public static Regex regex = new Regex("^(?!_)(?!.*?_$)[a-zA-Z0-9_一-龥]+$");

        public static bool canCreateChar(string name, bool gm)
        {
            return (getIdByName(name)) && (isEligibleCharName(name, gm));
        }

        public static bool getIdByName(string Name)
        {
            int ret = Sql.MySqlFactory.GetFactory.Query<CCharacter>().Where(a => a.Name.Equals(Name)).Count();

            return ret == 0;
        }

        public static bool isEligibleCharName(string Name, bool gm)
        {
            return regex.IsMatch(Name);
        }
    }
}
