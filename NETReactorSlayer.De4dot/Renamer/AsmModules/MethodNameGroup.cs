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

using System.Collections.Generic;

namespace NETReactorSlayer.De4dot.Renamer.AsmModules
{
    public class MethodNameGroup
    {
        public void Add(MMethodDef method) => Methods.Add(method);

        public void Merge(MethodNameGroup other)
        {
            if (this == other)
                return;
            Methods.AddRange(other.Methods);
        }

        public bool HasNonRenamableMethod()
        {
            foreach (var method in Methods)
                if (!method.Owner.HasModule)
                    return true;
            return false;
        }

        public bool HasInterfaceMethod()
        {
            foreach (var method in Methods)
                if (method.Owner.TypeDef.IsInterface)
                    return true;
            return false;
        }

        public bool HasGetterOrSetterPropertyMethod()
        {
            foreach (var method in Methods)
            {
                if (method.Property == null)
                    continue;
                var prop = method.Property;
                if (method == prop.GetMethod || method == prop.SetMethod)
                    return true;
            }

            return false;
        }

        public bool HasAddRemoveOrRaiseEventMethod()
        {
            foreach (var method in Methods)
            {
                if (method.Event == null)
                    continue;
                var evt = method.Event;
                if (method == evt.AddMethod || method == evt.RemoveMethod || method == evt.RaiseMethod)
                    return true;
            }

            return false;
        }

        public bool HasProperty()
        {
            foreach (var method in Methods)
                if (method.Property != null)
                    return true;
            return false;
        }

        public bool HasEvent()
        {
            foreach (var method in Methods)
                if (method.Event != null)
                    return true;
            return false;
        }

        public override string ToString() => $"{Methods.Count} -- {(Methods.Count > 0 ? Methods[0].ToString() : "")}";

        public int Count => Methods.Count;

        public List<MMethodDef> Methods { get; } = new List<MMethodDef>();
    }
}