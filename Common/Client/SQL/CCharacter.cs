using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.SQL
{
    [Table("character")]
    public class CCharacter
    {
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public short Ap { get; set; }
        public short Sp { get; set; }
        public int Userid { get; set; }
        public int World { get; set; }
        public int Exp { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Luk { get; set; }
        public short Int_ { get; set; }
        public short Hp { get; set; }
        public short Mp { get; set; }
        public short Maxhp { get; set; }
        public short Maxmp { get; set; }
        public short Job { get; set; }
        public int Skin { get; set; }
        public short Fame { get; set; }
        public int Hair { get; set; }
        public int Face { get; set; }
        public int MapId { get; set; }
        public int Gm { get; set; }
        public int Party { get; set; }
        public byte Spawnpoint { get; set; }
    }
}
