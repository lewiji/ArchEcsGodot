using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace ArchEcsGodot.WorldState;

public class ElementStorage
{
   static readonly Dictionary<Type, IElementStorageType> _elementStorage = new();
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static void AddElement<T>(T element)
   {
      _elementStorage.Add(typeof(T), new ElementStorageType<T>(element));
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static void AddOrReplaceElement<T>(T element) 
   {
      if (HasElement<T>())
      {
         _elementStorage[typeof(T)] = new ElementStorageType<T>(element);
      }
      else
      {
         AddElement(element);
      }
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static void RemoveElement<T>()
   {
      _elementStorage.Remove(typeof(T));
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static T GetElement<T>()
   {
      if (_elementStorage[typeof(T)] is ElementStorageType<T> elementStorageType)
      {
         return elementStorageType.Element;
      }
      throw new MissingMemberException();
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static bool TryGetElement<T>(out T? element)
   {
      if (HasElement<T>())
      {
         element = GetElement<T>();
         return true;
      }
      element = default;
      return false;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static bool HasElement<T>()
   {
      return _elementStorage.ContainsKey(typeof(T));
   }
}

public interface IElementStorageType { }

public struct ElementStorageType<T>(T element) : IElementStorageType
{
   public readonly T Element = element;
}