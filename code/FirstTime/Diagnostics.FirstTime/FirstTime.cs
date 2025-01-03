/*
    FirstTime Utility Test
    
    Copyright (C) 2009-2024 by Sergey A Kryukov
    http://www.SAKryukov.org
*/
namespace SA.Univeral.Utilities.Diagnostics {
    using Type = System.Type;
    using MethodBase = System.Reflection.MethodBase;
    using StackTrace = System.Diagnostics.StackTrace;
    using StackFrame = System.Diagnostics.StackFrame;
    using Debug = System.Diagnostics.Debug;
    using ReaderWriterLockSlim = System.Threading.ReaderWriterLockSlim;
    using LockRecursionPolicy = System.Threading.LockRecursionPolicy;
    using Cardinal = System.UInt64;
    using CodeLocationDictionary = System.Collections.Generic.Dictionary<CodeLocationKey, System.UInt64>;

    /// <summary>
    /// Utility used to guarantee that some point of code is reached only once per run-time of the Application domain.
    /// </summary>
    /// <remarks>
    /// The look up for number of method "Here" calls is performed per unique location in the calling code taking into
    /// account unique method and a unique code location inside method.
    /// </remarks>
    public static class FirstTime {

        /// <summary>
        /// // Usage:<para/>
        ///
        ///  if (FirstTime.Here) ThisWillHappenButOnlyOnce();<para/>
        ///
        /// </summary>
        /// <remarks>
        /// // Advanced usage (e.g. for debugging purposes):<para/>
        ///
        ///     Cardinal VisitNumber = FirstTime.Here; //using Cardinal = System.UInt64;<para/>
        ///     System.Diagnostics.Trace.WriteLine(string.Format("Activation number: {0}", activationNumber));
        /// </remarks>
        public static CodeLocationData Here { get { return GetCodeLocationData(CodeLocationDictionary, Lock); } }

        public struct CodeLocationData {
            internal CodeLocationData(Cardinal visitNumber) { this.VisitNumber = visitNumber; }
            public static implicit operator bool(CodeLocationData data) { return data.VisitNumber < 1; } //true if this is a first visit
            public static implicit operator Cardinal(CodeLocationData data) { return data.VisitNumber; } //visit number
            Cardinal VisitNumber;
        } //struct CodeLocationData 

        /// <summary>
        /// Utility used to guarantee that some point of code is reached only once per run-time of the Application domain per instance.<para/>
        /// This is a non-static version of <seealso cref="FirstTime"/>.
        /// A user's instace is supposed to obtain and own the intance of Instance.<para/><para/>
        /// 
        /// Usage:<para/>
        /// 
        ///  if (FirstTime.Here) ThisWillHappenButOnlyOnce();<para/>
        /// ...<para/>
        /// FirstTime.Instance FirstTime = new FirstTime.Instance();
        /// </summary>
        /// <remarks>
        /// Advanced usage (e.g. for debugging purposes):<para/>
        ///
        /// Cardinal VisitNumber = FirstTime.Here; //using Cardinal = System.UInt64;<para/>
        /// System.Diagnostics.Trace.WriteLine(string.Format("Activation number: {0}", activationNumber));<para/>
        /// ...<para/>
        /// FirstTime.Instance FirstTime = new FirstTime.Instance();
        /// </remarks>
        public class Instance {
            public CodeLocationData Here { get { return GetCodeLocationData(CodeLocationDictionary, Lock); } }
            CodeLocationDictionary CodeLocationDictionary = new CodeLocationDictionary();
            ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        } //class Instance

        #region implementation

        static CodeLocationData GetCodeLocationData(CodeLocationDictionary dictionary, ReaderWriterLockSlim dictionaryLock) {
            StackTrace stackTrace = new StackTrace();
            int count = stackTrace.FrameCount;
            for (int level = 0; level < count; level++) {
                StackFrame frame = stackTrace.GetFrame(level);
                MethodBase method = frame.GetMethod();
                Type declaringType = method.DeclaringType;
                if (ThisType == null) //lazy
                    ThisType = declaringType;
                if (declaringType == ThisType) continue;
                CodeLocationKey key = new CodeLocationKey(method.MethodHandle.Value, frame.GetNativeOffset());
                Cardinal visitNumber = 0;
                dictionaryLock.EnterUpgradeableReadLock();
                try {
                    bool alreadyVisited = dictionary.TryGetValue(key, out visitNumber);
                    if (alreadyVisited) {
                        unchecked { visitNumber++; }
                        dictionary[key] = visitNumber;
                    } else {
                        dictionaryLock.EnterWriteLock();
                        try {
                            dictionary.Add(key, visitNumber);
                        } finally {
                            dictionaryLock.ExitWriteLock();
                        } //try write lock
                    } //if alreadyVisited
                    return new CodeLocationData(visitNumber);
                } finally {
                    dictionaryLock.ExitUpgradeableReadLock();
                } //try upgradeable read lock
            } //loop
            Debug.Assert("FirstTime.Here method should always find stack frame of the caller" == null);
            return default(CodeLocationData); //ha-ha! will never get here anyway
        } //GetCodeLocationData

        static Type ThisType = null; //obtained from lazy evaluation; simple typeof(FirstTime) would work but not used to make it all rename-safe and obfuscation-safe
        static CodeLocationDictionary CodeLocationDictionary = new CodeLocationDictionary();
        static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        #endregion implementation

    } //class FirstTime

} //namespace SA.Univeral.Utilities.Diagnostics