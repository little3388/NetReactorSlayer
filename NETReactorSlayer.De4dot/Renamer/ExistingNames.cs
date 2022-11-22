/*
    Copyright (C) 2021 CodeStrikers.org
    This file is part of NETReactorSlayer.
    NETReactorSlayer is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    NETReactorSlayer is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with NETReactorSlayer.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace NETReactorSlayer.De4dot.Renamer {
    public class ExistingNames {
        public void Add(string name) => _allNames[name] = true;

        public bool Exists(string name) => _allNames.ContainsKey(name);

        public string GetName(UTF8String oldName, INameCreator nameCreator) =>
            GetName(UTF8String.ToSystemStringOrEmpty(oldName), nameCreator);

        public string GetName(string oldName, INameCreator nameCreator) => GetName(oldName, nameCreator.Create);

        public string GetName(UTF8String oldName, Func<string> createNewName) =>
            GetName(UTF8String.ToSystemStringOrEmpty(oldName), createNewName);

        public string GetName(string oldName, Func<string> createNewName) {
            string prevName = null;
            while (true) {
                var name = createNewName();
                if (name == prevName)
                    throw new ApplicationException($"Could not rename symbol to {Utils.ToCsharpString(name)}");

                if (!Exists(name) || name == oldName) {
                    _allNames[name] = true;
                    return name;
                }

                prevName = name;
            }
        }

        public void Merge(ExistingNames other) {
            if (this == other)
                return;
            foreach (var key in other._allNames.Keys)
                _allNames[key] = true;
        }

        private readonly Dictionary<string, bool> _allNames = new(StringComparer.Ordinal);
    }
}