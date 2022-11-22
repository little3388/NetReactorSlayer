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

using dnlib.DotNet;

namespace NETReactorSlayer.De4dot
{
    public class DeobfuscatorContext : IDeobfuscatorContext
    {
        private static ITypeDefOrRef GetNonGenericTypeRef(ITypeDefOrRef typeRef)
        {
            var ts = typeRef as TypeSpec;
            if (ts == null)
                return typeRef;
            var gis = ts.TryGetGenericInstSig();
            return gis?.GenericType == null ? typeRef : gis.GenericType.TypeDefOrRef;
        }

        public TypeDef ResolveType(ITypeDefOrRef type)
        {
            if (type == null)
                return null;
            type = GetNonGenericTypeRef(type);

            switch (type)
            {
                case TypeDef typeDef:
                    return typeDef;
                case TypeRef tr:
                    return tr.Resolve();
                default:
                    return null;
            }
        }
    }
}