
using System;
[assembly:CLSCompliant(true)]
namespace My.Sample {
    ///<summary>My.Sample.SomeClass Summary Text</summary>
    [CLSCompliant(true)]
    public class SomeClass {
        public virtual TParam GenericMethod<TParam>(TParam genericParameter) { return genericParameter; }
        ///<remarks>My.Sample.SomeClass.GetName(string) Remarks</remarks>
        public string GetName(string prefix) { return string.Empty; }
    }

    ///<summary>Check out <see cref="T:My.Sample.SomeEnum" />.</summary>
    public struct SomeStruct {
        [CLSCompliant(true)]
        public int Value;
    }

    public enum SomeEnum {
        A,B,C
    }
}