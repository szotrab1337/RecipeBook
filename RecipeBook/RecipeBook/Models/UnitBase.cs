using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeBook.Models
{
    public static class UnitBase
    {
        private static readonly List<Unit> Units = new List<Unit>()
        {
            new Unit(){ UnitId = 1, Name = "sztuki", ShortName = "szt" },
            new Unit(){ UnitId = 2, Name = "litry", ShortName = "l" },
            new Unit(){ UnitId = 3, Name = "opakowania", ShortName = "op" },
            new Unit(){ UnitId = 4, Name = "kilogramy", ShortName = "kg" },
            new Unit(){ UnitId = 5, Name = "gramy", ShortName = "g" },
            new Unit(){ UnitId = 6, Name = "mililitry", ShortName = "ml" },
            new Unit(){ UnitId = 6, Name = "łyżki", ShortName = "ł" },
            new Unit(){ UnitId = 6, Name = "łyżeczki", ShortName = "łyż" },
            new Unit(){ UnitId = 6, Name = "szklanki", ShortName = "ml" },
            new Unit(){ UnitId = 6, Name = "szczypty", ShortName = "szcz" }
        };

        public static List<Unit> GetUnits()
        {
            return Units;
        }

        public static string GetUnit(int unitId)
        {
            return Units.FirstOrDefault(x => x.UnitId == unitId).ShortName;
        }
    }
}