using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"System.Core.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<Entry.MyVec3>
	// System.Action<System.IntPtr,System.Decimal>
	// System.Action<System.IntPtr,System.IntPtr>
	// System.Action<System.IntPtr,UnityEngine.Bounds>
	// System.Action<System.IntPtr,UnityEngine.Color>
	// System.Action<System.IntPtr,UnityEngine.Quaternion>
	// System.Action<System.IntPtr,UnityEngine.Ray2D>
	// System.Action<System.IntPtr,UnityEngine.Ray>
	// System.Action<System.IntPtr,UnityEngine.Vector2>
	// System.Action<System.IntPtr,UnityEngine.Vector3>
	// System.Action<System.IntPtr,UnityEngine.Vector4>
	// System.Action<System.IntPtr,byte>
	// System.Action<System.IntPtr,double>
	// System.Action<System.IntPtr,float>
	// System.Action<System.IntPtr,int,UnityEngine.Bounds>
	// System.Action<System.IntPtr,int,UnityEngine.Color>
	// System.Action<System.IntPtr,int,UnityEngine.Quaternion>
	// System.Action<System.IntPtr,int,UnityEngine.Ray2D>
	// System.Action<System.IntPtr,int,UnityEngine.Ray>
	// System.Action<System.IntPtr,int,UnityEngine.Vector2>
	// System.Action<System.IntPtr,int,UnityEngine.Vector3>
	// System.Action<System.IntPtr,int,UnityEngine.Vector4>
	// System.Action<System.IntPtr,int,object>
	// System.Action<System.IntPtr,int>
	// System.Action<System.IntPtr,long>
	// System.Action<System.IntPtr,object>
	// System.Action<System.IntPtr,sbyte>
	// System.Action<System.IntPtr,short>
	// System.Action<System.IntPtr,uint>
	// System.Action<System.IntPtr,ulong>
	// System.Action<System.IntPtr,ushort>
	// System.Action<System.IntPtr>
	// System.Action<UnityEngine.ParticleSystem.Particle>
	// System.Action<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Action<UnityEngine.Vector4>
	// System.Action<byte>
	// System.Action<double>
	// System.Action<int,float>
	// System.Action<int>
	// System.Action<object,object>
	// System.Action<object>
	// System.ByReference<UnityEngine.Quaternion>
	// System.ByReference<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<Entry.MyVec3>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector4>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<Entry.MyVec3>
	// System.Collections.Generic.Comparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.Comparer<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.Comparer<UnityEngine.Vector4>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<System.IntPtr,object>
	// System.Collections.Generic.Dictionary.Enumerator<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary.Enumerator<float,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<System.IntPtr,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<float,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection<System.IntPtr,object>
	// System.Collections.Generic.Dictionary.KeyCollection<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection<float,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<System.IntPtr,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<float,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection<System.IntPtr,object>
	// System.Collections.Generic.Dictionary.ValueCollection<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection<float,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,object>
	// System.Collections.Generic.Dictionary<System.IntPtr,object>
	// System.Collections.Generic.Dictionary<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.Dictionary<float,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.EqualityComparer<System.IntPtr>
	// System.Collections.Generic.EqualityComparer<XLua.Utils.MethodKey>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<ulong>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Entry.MyVec3>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.IntPtr,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<XLua.Utils.MethodKey,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<float,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,System.Collections.Generic.KeyValuePair<object,object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ICollection<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ICollection<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.ICollection<UnityEngine.Vector4>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<Entry.MyVec3>
	// System.Collections.Generic.IComparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IComparer<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.IComparer<UnityEngine.Vector4>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<Entry.MyVec3>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.IntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<XLua.Utils.MethodKey,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<float,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,System.Collections.Generic.KeyValuePair<object,object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IEnumerable<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector4>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<Entry.MyVec3>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.IntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<XLua.Utils.MethodKey,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<float,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,System.Collections.Generic.KeyValuePair<object,object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IEnumerator<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector4>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<System.IntPtr>
	// System.Collections.Generic.IEqualityComparer<XLua.Utils.MethodKey>
	// System.Collections.Generic.IEqualityComparer<float>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ulong>
	// System.Collections.Generic.IList<Entry.MyVec3>
	// System.Collections.Generic.IList<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IList<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.IList<UnityEngine.Vector4>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<System.IntPtr,object>
	// System.Collections.Generic.KeyValuePair<XLua.Utils.MethodKey,object>
	// System.Collections.Generic.KeyValuePair<float,object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ulong,System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.List.Enumerator<Entry.MyVec3>
	// System.Collections.Generic.List.Enumerator<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector4>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<Entry.MyVec3>
	// System.Collections.Generic.List<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.List<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.List<UnityEngine.Vector4>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<Entry.MyVec3>
	// System.Collections.Generic.ObjectComparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector4>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.IntPtr>
	// System.Collections.Generic.ObjectEqualityComparer<XLua.Utils.MethodKey>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ulong>
	// System.Collections.Generic.Queue.Enumerator<XLua.LuaEnv.GCAction>
	// System.Collections.Generic.Queue<XLua.LuaEnv.GCAction>
	// System.Collections.ObjectModel.ReadOnlyCollection<Entry.MyVec3>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.ParticleSystem.Particle>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector4>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<Entry.MyVec3>
	// System.Comparison<UnityEngine.ParticleSystem.Particle>
	// System.Comparison<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Comparison<UnityEngine.Vector4>
	// System.Comparison<byte>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<System.IntPtr,int,System.Decimal>
	// System.Func<System.IntPtr,int,System.IntPtr>
	// System.Func<System.IntPtr,int,Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>>
	// System.Func<System.IntPtr,int,Unity.Collections.NativeArray<byte>>
	// System.Func<System.IntPtr,int,Unity.Collections.NativeArray<int>>
	// System.Func<System.IntPtr,int,UnityEngine.Bounds>
	// System.Func<System.IntPtr,int,UnityEngine.Color>
	// System.Func<System.IntPtr,int,UnityEngine.Keyframe>
	// System.Func<System.IntPtr,int,UnityEngine.LightBakingOutput>
	// System.Func<System.IntPtr,int,UnityEngine.Matrix4x4>
	// System.Func<System.IntPtr,int,UnityEngine.ParticleSystem.EmitParams>
	// System.Func<System.IntPtr,int,UnityEngine.ParticleSystem.Particle>
	// System.Func<System.IntPtr,int,UnityEngine.ParticleSystem.PlaybackState>
	// System.Func<System.IntPtr,int,UnityEngine.ParticleSystem.Trails>
	// System.Func<System.IntPtr,int,UnityEngine.Quaternion>
	// System.Func<System.IntPtr,int,UnityEngine.Ray2D>
	// System.Func<System.IntPtr,int,UnityEngine.Ray>
	// System.Func<System.IntPtr,int,UnityEngine.SceneManagement.Scene>
	// System.Func<System.IntPtr,int,UnityEngine.Vector2>
	// System.Func<System.IntPtr,int,UnityEngine.Vector3>
	// System.Func<System.IntPtr,int,UnityEngine.Vector4>
	// System.Func<System.IntPtr,int,byte>
	// System.Func<System.IntPtr,int,double>
	// System.Func<System.IntPtr,int,float>
	// System.Func<System.IntPtr,int,int>
	// System.Func<System.IntPtr,int,long>
	// System.Func<System.IntPtr,int,object>
	// System.Func<System.IntPtr,int,sbyte>
	// System.Func<System.IntPtr,int,short>
	// System.Func<System.IntPtr,int,uint>
	// System.Func<System.IntPtr,int,ulong>
	// System.Func<System.IntPtr,int,ushort>
	// System.Func<double,double,double>
	// System.Func<int,object,object>
	// System.Func<object,byte>
	// System.Func<object,int>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.<ConcatIterator>d__59<object>
	// System.Linq.Enumerable.<JoinIterator>d__38<object,object,object,object>
	// System.Linq.Enumerable.<SelectManyIterator>d__23<object,object,object>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,object>
	// System.Linq.Enumerable.WhereSelectListIterator<object,object>
	// System.Linq.EnumerableSorter<object,int>
	// System.Linq.EnumerableSorter<object>
	// System.Linq.GroupedEnumerable<object,object,object>
	// System.Linq.IGrouping<object,object>
	// System.Linq.Lookup.<GetEnumerator>d__12<object,object>
	// System.Linq.Lookup.Grouping.<GetEnumerator>d__7<object,object>
	// System.Linq.Lookup.Grouping<object,object>
	// System.Linq.Lookup<object,object>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<object>
	// System.Linq.OrderedEnumerable<object,int>
	// System.Linq.OrderedEnumerable<object>
	// System.Predicate<Entry.MyVec3>
	// System.Predicate<UnityEngine.ParticleSystem.Particle>
	// System.Predicate<UnityEngine.Rendering.ReflectionProbeBlendInfo>
	// System.Predicate<UnityEngine.Vector4>
	// System.Predicate<byte>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.ReadOnlySpan<UnityEngine.Quaternion>
	// System.ReadOnlySpan<UnityEngine.Vector3>
	// System.Span<UnityEngine.Quaternion>
	// System.Span<UnityEngine.Vector3>
	// UnityEngine.AsyncInstantiateOperation<object>
	// }}

	public void RefMethods()
	{
		// object[] System.Array.Empty<object>()
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Concat<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.ConcatIterator<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<System.Linq.IGrouping<object,object>> System.Linq.Enumerable.GroupBy<object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Join<object,object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,object>,System.Func<object,object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.JoinIterator<object,object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,object>,System.Func<object,object,object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Linq.IOrderedEnumerable<object> System.Linq.Enumerable.OrderBy<object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.SelectMany<object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<object>>,System.Func<object,object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.SelectManyIterator<object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<object>>,System.Func<object,object,object>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<object,object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<object>.Select<object>(System.Func<object,object>)
		// System.IntPtr System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate<object>(object)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,UnityEngine.Transform)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,UnityEngine.Transform,UnityEngine.Vector3,UnityEngine.Quaternion)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,int)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,int,UnityEngine.Transform)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,int,UnityEngine.Transform,System.ReadOnlySpan<UnityEngine.Vector3>,System.ReadOnlySpan<UnityEngine.Quaternion>)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,int,UnityEngine.Transform,UnityEngine.Vector3,UnityEngine.Quaternion)
		// UnityEngine.AsyncInstantiateOperation<object> UnityEngine.Object.InstantiateAsync<object>(object,int,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object[] UnityEngine.Resources.ConvertObjects<object>(UnityEngine.Object[])
		// object[] UnityEngine.Resources.FindObjectsOfTypeAll<object>()
	}
}