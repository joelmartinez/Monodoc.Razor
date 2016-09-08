
using System;
[assembly:CLSCompliant(true)]
namespace My.Sample {
    ///<summary>My.Sample.SomeClass Summary Text</summary>
    [CLSCompliant(true)]
    public class SomeClass {
        ///<remarks>My.Sample.SomeClass.GetName(string) Remarks</remarks>
        public string GetName(string prefix) { return string.Empty; }
    }

    public struct SomeStruct {
        [CLSCompliant(true)]
        public int Value;
    }
}