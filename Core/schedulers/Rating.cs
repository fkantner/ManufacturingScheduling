namespace Core.Schedulers
{
  using System;

  public class Rating<T> : IComparable
  {
    public T Object { get; private set; }
    public int Value { get; set; }
    public Rating(T obj, int value)
    {
      Object = obj;
      Value = value;
    }

    public int CompareTo(object obj)
    {
      if(obj.GetType() == this.GetType()) { return CompareTo((Rating<T>) obj); }
      return 0;
    }

    public int CompareTo(Rating<T> obj)
    {
      return obj.Value - this.Value;
    }
  }
}