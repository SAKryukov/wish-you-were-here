/*
    FirstTime Utility
    
    Copyright (C) 2009-2024 by Sergey A Kryukov
    http://www.SAKryukov.org
*/
namespace SA.Univeral.Utilities {
    using MethodHandle = System.IntPtr;
    using Offset = System.Int32;
    using Debug = System.Diagnostics.Debug;

    internal struct CodeLocationKey {

        internal CodeLocationKey(MethodHandle methodHandle, Offset offset) {
            this.MethodHandle = methodHandle;
            this.Offset = offset;
        } //CodeLocationKey

        public override int GetHashCode() {
            return MethodHandle.GetHashCode() ^ Offset.GetHashCode(); 
        } //GetHashCode

        public override bool Equals(object obj) {
            Debug.Assert(obj != null, "CodeLocationKey is not designed to compare with null object");
            Debug.Assert(obj.GetType() == this.GetType(), "CodeLocationKey is not designed to compare with objects of other types");
            CodeLocationKey other = (CodeLocationKey)obj;
            return other.MethodHandle == this.MethodHandle && other.Offset == this.Offset;
        } //Equals
        
        MethodHandle MethodHandle;
        Offset Offset;

    } //class CodeLocationKey

} //namespace SA.Univeral.Utilities